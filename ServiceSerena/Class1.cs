using DominioSerena;
using InfrastructureSerena;

namespace ServiceSerena
{
    public class UserService
    {
        private readonly IRepositoryGeneric<User> _userRepository;
        private readonly IRepositoryGeneric<Ativo> _ativoRepository;

        public UserService(IRepositoryGeneric<User> userRepository, IRepositoryGeneric<Ativo> ativoRepository)
        {
            _userRepository = userRepository;
            _ativoRepository = ativoRepository;
        }

        public async Task<User?> GetById(int id) 
        {
            return await _userRepository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Ativo?>> GetAllAtivosUser(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return Enumerable.Empty<Ativo?>();
            }
            string propriedade = null;
            if (typeof(Ativo).GetProperty("UserId") != null)
            {
                propriedade = "UserId";
            }

            var ativos = await _ativoRepository.GetAllAsync(userId,propriedade);
            return ativos.Where(a => a.UserId == userId);
        }

    }
}
