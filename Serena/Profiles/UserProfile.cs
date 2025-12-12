using AutoMapper;
using Serena.Models;
using Serena.Models.DTOs;

namespace Serena.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserViewModel>();
            CreateMap<UserViewModel, UserDto>();

        }

        private static string MaskCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf) || cpf.Length < 11)
                return cpf;
            // ex: 12345678901 -> ***.***.890-1 (exemplo simples)
            return $"***.***.{cpf.Substring(6, 3)}-{cpf.Substring(9)}";
        }
    }
    
}
