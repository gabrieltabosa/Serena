using Serena.Models;

namespace Serena.Service
{
    
    public interface IUserApiClient
    {
        
        Task<UserViewModel?> AuthenticateAsync(UserViewModel dto);

        
        Task<bool> ResetPasswordAsync(UserViewModel dto);

        
        Task<UserViewModel?> GetByIdAsync(int id, string sessionId);

       
        Task<UserViewModel?> CreateAsync(UserViewModel dto);

        
        Task<UserViewModel?> UpdateAsync(int id, UserViewModel dto, string sessionId);

       
        Task<bool> DeleteAsync(int id,string sessionId);
    }
}

