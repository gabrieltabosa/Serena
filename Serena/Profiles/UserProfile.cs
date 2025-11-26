using AutoMapper;
using Serena.Models;


namespace Serena.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<UserViewModel, UserViewModel>().ReverseMap();
            CreateMap<EnderecoViewModel, EnderecoViewModel>().ReverseMap();
        }
    }
}
