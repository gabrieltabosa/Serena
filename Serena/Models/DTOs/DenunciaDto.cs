using System.ComponentModel.DataAnnotations;

namespace Serena.Models.DTOs
{
    public class DenunciaDto
    {
        [Required]
        public string Descricao { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public string TipoViolencia { get; set; }
        public EnderecoDTO? Endereco { get; set; }
    }
}
