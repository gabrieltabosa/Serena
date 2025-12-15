using System.ComponentModel.DataAnnotations;

namespace Serena.Models.DTOs
{
    public class DenunciaDto
    {
        [Required]
        public string Descricao { get; set; } = string.Empty;
        public int? UsuarioId { get; set; }
        public string Violencia { get; set; }
        public EnderecoDTO? Endereco { get; set; }
    }
}
