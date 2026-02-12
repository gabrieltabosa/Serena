using DominioSerena.DTOs;
using AutoMapper;
using DominioSerena;
namespace Serena.Profiles
{
    public class DenunciaProfile : Profile
    {
        public DenunciaProfile()
        {


            CreateMap<DenunciaDto, DenunciaViewModel>()
                .ForMember(dest => dest.TipoViolencia, opt => opt.MapFrom(src =>
                    Enum.Parse<TipoViolenciaViewModel>(src.TipoViolencia, true)));

            CreateMap<DenunciaViewModel, DenunciaDto>()
                .ForMember(dest => dest.TipoViolencia, opt => opt.MapFrom(src =>
                    src.TipoViolencia.ToString()));

            CreateMap<EnderecoDTO, EnderecoViewModel>().ReverseMap();




        }
        
    


    }
    
}
