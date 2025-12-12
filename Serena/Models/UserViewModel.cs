using System.ComponentModel.DataAnnotations;

namespace Serena.Models
{
    public class UserViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Nome completo")]
        public string Name { get; set; }
        public string? Password { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Display(Name = "RG")]
        public string Rg { get; set; }

        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [Display(Name = "Data de nascimento")]
        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; }

        public EnderecoViewModel? Endereco { get; set; }

        public ICollection<ApoioViewModel>? NumerosDeApoio { get; set; }

        [Display(Name = "Lembrar-me")]
        public bool RememberMe { get; set; }

    }
}
