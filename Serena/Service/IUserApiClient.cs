using Serena.Models;
using Serena.Models.DTOs;

namespace Serena.Service
{
    public interface IUserApiClient
    {
        Task<UserDto?> AuthenticateAsync(UserViewModel dto, CancellationToken ct = default);
        Task<bool> ResetPasswordAsync(UserViewModel dto, CancellationToken ct = default);
        Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<UserDto?> CreateAsync(UserViewModel dto, CancellationToken ct = default);
        Task<UserDto?> UpdateAsync(int id, UserViewModel dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
