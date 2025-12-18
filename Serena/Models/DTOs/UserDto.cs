using System.ComponentModel.DataAnnotations;

namespace Serena.Models.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome completo")]
        public string Name { get; set; }

        public string Email { get; set; }

        public string? Rg { get; set; }
        public string? Cpf { get; set; }

        public string? Password { get; set; }


        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [Display(Name = "Data de nascimento")]
       
        public DateTime? DataNascimento { get; set; }

        public EnderecoViewModel Endereco { get; set; }
        public ICollection<ApoioViewModel>? NumerosDeApoio { get; set; }
    }
}
