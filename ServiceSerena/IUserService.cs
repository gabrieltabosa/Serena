using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DominioSerena;

namespace ServiceSerena
{
    public interface IUserService: IGenericService<User>
    {
        // RB1: Método essencial para login/autenticação
        Task<User?> GetUserByCredentialsAsync(string email, string password);

        // RB2: Método de validação (Garantir que o email é único)
        Task<bool> IsEmailUniqueAsync(string email);

        // RB3: Busca por uma propriedade única, fora do ID
        Task<User?> GetUserByCpfAsync(string cpf);
    }
}
