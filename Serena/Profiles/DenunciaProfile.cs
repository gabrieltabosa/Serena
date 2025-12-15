using Serena.Models.DTOs;
using AutoMapper;
using Serena.Models;
namespace Serena.Profiles
{
    public class DenunciaProfile : Profile
    {
        public DenunciaProfile()
        {
            CreateMap<DenunciaDto, DenunciaViewModel>();
            CreateMap<DenunciaViewModel, DenunciaDto>();
            CreateMap<EnderecoDTO, EnderecoViewModel>();
            CreateMap<EnderecoViewModel, EnderecoDTO>();
                



        }
        
    


    }
    
}
