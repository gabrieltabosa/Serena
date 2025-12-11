using System.ComponentModel.DataAnnotations;

namespace Serena.Models.DTOs
{
    public class DenunciaDto
    {
        [Required]
        public string Descricao { get; set; } = string.Empty;
        public int? UsuarioId { get; set; }
        public int TipoViolencia { get; set; }
        public EnderecoViewModel? Endereco { get; set; }
    }
}
