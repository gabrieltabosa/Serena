using Serena.Models;
using Serena.Models.DTOs;

namespace Serena.Service
{
    public interface IUserApiClient
    {
        Task<UserViewModel?> AuthenticateAsync(UserViewModel dto);
        Task<bool> ResetPasswordAsync(UserViewModel dto);
        Task<UserViewModel?> GetByIdAsync(int id);
        Task<UserViewModel?> CreateAsync(UserViewModel dto);
        Task<UserViewModel?> UpdateAsync(int id, UserViewModel dto);
        Task<bool> DeleteAsync(int id);
    }
}
