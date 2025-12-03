using DominioSerena;
using InfrastructureSerena;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSerena
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly AppDbContext _context; // Para Unit of Work

        public UserService(IGenericRepository<User> repository, AppDbContext context)
        {
            _userRepository = repository;
            _context = context;
        }

        public async Task AddAsync(User entity)
        {
            // RB1: Validação de Email único antes de salvar
            if (!await IsEmailUniqueAsync(entity.Email))
            {
                throw new InvalidOperationException($"O email '{entity.Email}' já está em uso.");
            }
            // RB2: Validação de CPF único (opcional, mas recomendado)
            if (await GetUserByCpfAsync(entity.Cpf) != null)
            {
                throw new InvalidOperationException($"O CPF '{entity.Cpf}' já está cadastrado.");
            }

            // RB3: Implementação de segurança (Hashing de Senha, não incluído aqui)
            // entity.Password = HashPassword(entity.Password);

            await _userRepository.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Métodos de leitura simples herdados
        public async Task<User?> GetByIdAsync(int id) => await _userRepository.GetByIdAsync(id);
        public async Task<IEnumerable<User>> GetAllAsync() => await _userRepository.GetAllAsync();

        // Implementação do restante do CRUD
        public async Task UpdateAsync(User entity)
        {
            await _userRepository.UpdateAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // O Repositório faz a busca e exclusão; o Service apenas orquestra.
            await _userRepository.DeleteAsync(id);
        }

        // O método FindAllByForeignKeyAsync é herdado e pode ser usado se User
        // fosse uma entidade filha, mas para Ativo é melhor usar o IAtivoService.
        public Task<IEnumerable<User>> FindAllByForeignKeyAsync(string foreignKeyName, object foreignKeyValue)
        {
            // Este método provavelmente não é útil para User, mas deve ser implementado por herança
            throw new NotImplementedException("Consulta de FK não é aplicável diretamente à entidade User.");
        }


        // --- Implementação das Regras de Negócio Específicas ---

        public async Task<User?> GetUserByCredentialsAsync(string email, string password)
        {
            // *Nota: Idealmente, usaríamos um Repositório Customizado para esta consulta.
            // Aqui, usamos o GetAllAsync (não ideal para produção em tabelas grandes)
            // ou injetamos o DbSet diretamente (não é o padrão do Repositório),
            // ou adicionamos um método FindByPredicateAsync ao Repositório Genérico.*

            // Usando GetAllAsync + LINQ (Solução simples para demonstração):
            var users = await _userRepository.GetAllAsync();

            var user = users.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password); // *Idealmente, verificaríamos o hash aqui*

            return user;
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            // Novamente, idealmente um método FindByPredicateAsync
            var users = await _userRepository.GetAllAsync();

            return !users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<User?> GetUserByCpfAsync(string cpf)
        {
            var users = await _userRepository.GetAllAsync();

            return users.FirstOrDefault(u => u.Cpf == cpf);
        }

    }
}
