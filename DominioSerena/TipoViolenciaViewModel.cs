using System.ComponentModel.DataAnnotations;

namespace DominioSerena
{
    public enum TipoViolenciaViewModel
    {
        [Display(Name ="Violencia Fisica")]
        violenciaFisica = 1,
        [Display(Name = "Violencia Psicologica")]
        violenciaPsicologica = 2,
        [Display(Name = "Violencia Sexual")]
        violenciaSexual = 3,
        [Display(Name = "Violencia Patrimonial")]
        violenciaPatrimonial = 4,
        [Display(Name = "Violencia Moral")]
        violenciaMoral = 5,
    }
}
