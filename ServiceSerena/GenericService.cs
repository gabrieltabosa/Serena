using InfrastructureSerena;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSerena
{
    public class GenericService<T>: IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly AppDbContext _context; // Gerencia o Unit of Work

        public GenericService(IGenericRepository<T> repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        // --- Operações de Escrita (Com Unit of Work) ---

        public async Task AddAsync(T entity)
        {
            // O repositório rastreia a entidade
            await _repository.AddAsync(entity);
            // O Service executa o Unit of Work
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // O Repositório genérico com a lógica de "Buscar e Remover" deve ser usado aqui.
            // Assumimos que o Repositório genérico faz o SaveChangesAsync.
            // Se o Repositório APENAS remover, a chamada seria:
            // await _repository.DeleteAsync(id);
            // await _context.SaveChangesAsync();

            // Para consistência com a lógica de exclusão segura que você aprovou:
            var entityToDelete = await _repository.GetByIdAsync(id);
            if (entityToDelete == null)
            {
                throw new KeyNotFoundException($"Entidade do tipo {typeof(T).Name} com ID {id} não encontrada para exclusão.");
            }

            // Repositório marca para remoção (se tivermos refatorado o repo para não salvar)
            // Se o repositório salva, usamos o método do repositório:
            await _repository.DeleteAsync(id);

            // OBS: Se você usa o Repositório que salva no Delete, remova este bloco.
            // Caso contrário, use:
            // await _repository.DeleteAsync(entityToDelete);
            // await _context.SaveChangesAsync(); 
        }

        // --- Operações de Leitura (Sem Unit of Work) ---

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // --- Consulta Dinâmica (FK) ---

        public async Task<IEnumerable<T>> FindAllByForeignKeyAsync(string foreignKeyName, object foreignKeyValue)
        {
            // Repassa a chamada para o Repositório, que usa Expression Trees.
            return await _repository.FindAllByForeignKeyAsync(foreignKeyName, foreignKeyValue);
        }
    }
}
