using InfrastructureSerena;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DominioSerena;
using System.Linq.Expressions;

namespace ServiceSerena
{
    public class AtivoService: IAtivoService
    {
        private readonly IGenericRepository<Ativo> _ativoRepository;
        private readonly AppDbContext _context;

        public AtivoService(IGenericRepository<Ativo> ativoRepository, AppDbContext context)
        {
            _ativoRepository = ativoRepository;
            _context = context;
        }

        public async Task AddAsync(Ativo entity)
        {
            // RB: Validação antes de persistir
            if (entity.DataAquisicao > DateTime.Now)
            {
                throw new InvalidOperationException("A data de aquisição não pode ser futura.");
            }

            // 1. Repositório rastreia a entidade
            await _ativoRepository.AddAsync(entity);

            // 2. Service executa o Unit of Work
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ativo entity)
        {
            // 1. Repositório marca como modificado
            await _ativoRepository.UpdateAsync(entity);

            // 2. Service executa o Unit of Work
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Implementação do padrão seguro: Buscar antes de deletar
            var entityToDelete = await _ativoRepository.GetByIdAsync(id);

            if (entityToDelete == null)
            {
                throw new KeyNotFoundException($"Ativo com ID {id} não encontrado para exclusão.");
            }

            // 1. Repositório marca como removido
            // Note: O repositório genérico que você forneceu já faz o SaveChangesAsync.
            // Para seguir o padrão UoW 100%, você teria que garantir que o Repositório
            // apenas chame _dbSet.Remove(), e o Service chame o SaveChangesAsync, assim:
            // await _ativoRepository.DeleteAsync(entityToDelete);
            // await _context.SaveChangesAsync();

            // Usando a implementação atual do seu Repositório (que salva no Delete):
            await _ativoRepository.DeleteAsync(id);
        }

        // --- Operações de Leitura Básicas ---

        public async Task<Ativo?> GetByIdAsync(int id)
        {
            return await _ativoRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Ativo>> GetAllAsync()
        {
            return await _ativoRepository.GetAllAsync();
        }

        // --- Operação Genérica de Busca por FK (Herdada) ---

        public async Task<IEnumerable<Ativo>> FindAllByForeignKeyAsync(string foreignKeyName, object foreignKeyValue)
        {
            // Repassa a chamada para o Repositório, útil para consultas dinâmicas.
            return await _ativoRepository.FindAllByForeignKeyAsync(foreignKeyName, foreignKeyValue);
        }

        // --- Regras de Negócio Específicas (Encapsulando a lógica de FK) ---

        public async Task<IEnumerable<Ativo>> GetAtivosByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("ID de Usuário inválido.", nameof(userId));
            }

            // Encapsula o método genérico, usando a tipagem e o 'nameof' para segurança.
            return await _ativoRepository.FindAllByForeignKeyAsync(
                foreignKeyName: nameof(Ativo.UserId),
                foreignKeyValue: userId
            );
        }

        public async Task<IEnumerable<Ativo>> GetAtivosByCategoriaIdAsync(int categoriaId)
        {
            if (categoriaId <= 0)
            {
                throw new ArgumentException("ID de Categoria inválido.", nameof(categoriaId));
            }

            // Encapsula o método genérico, usando a tipagem e o 'nameof' para segurança.
            return await _ativoRepository.FindAllByForeignKeyAsync(
                foreignKeyName: nameof(Ativo.CategoriaId),
                foreignKeyValue: categoriaId
            );
        }

        // --- Regra de Negócio Pura (Lógica de Filtragem Complexa) ---

        public async Task<IEnumerable<Ativo>> GetAtivosDisponiveisParaTransferenciaAsync()
        {
            // RB: Um ativo está disponível se:
            // 1. O estado for 'Disponivel'.
            // 2. Foi adquirido há mais de 30 dias (exemplo de regra).

            var todosAtivos = await _ativoRepository.GetAllAsync();

            var ativosDisponiveis = todosAtivos
                .Where(a =>
                    a.Estado == EstadoAtivo.Disponivel &&
                    a.DataAquisicao < DateTime.Now.AddDays(-30)) // Ex: 30 dias de uso mínimo
                .OrderBy(a => a.Nome)
                .ToList();

            return ativosDisponiveis;
        }

        public async Task<IEnumerable<Ativo>> GetAtivosByCategoriaIdAndUserIdAsync(int categoriaId, int userId)
        {
            // 1. Validação de Argumentos
            if (categoriaId <= 0)
            {
                throw new ArgumentException("ID de Categoria inválido.", nameof(categoriaId));
            }
            if (userId <= 0)
            {
                throw new ArgumentException("ID de Usuário inválido.", nameof(userId));
            }

            // 2. Construção da Expressão Combinada (e => e.CategoriaId == categoriaId && e.UserId == userId)
            Expression<Func<Ativo, bool>> predicate =
                a => a.CategoriaId == categoriaId && a.UserId == userId;

            // 3. Chamada ao Repositório para executar o filtro no banco de dados
            // Isso é seguro e eficiente (um único SELECT com dois WHERE clauses).
            return await _ativoRepository.GetByConditionAsync(predicate);
        }
    }
}
