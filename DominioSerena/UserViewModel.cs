using System.ComponentModel.DataAnnotations;

namespace DominioSerena
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        [Display(Name = "Nome completo")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "A senha deve conter letras maiúsculas, minúsculas e números.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um endereço de email válido.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Formato de CPF inválido. Use 000.000.000-00")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O RG é obrigatório.")]
        [Display(Name = "RG")]
        public string Rg { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Formato de telefone inválido.")]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de nascimento")]
        public DateTime? DataNascimento { get; set; }

        public EnderecoViewModel? Endereco { get; set; }

        public ICollection<ApoioViewModel>? NumerosDeApoio { get; set; }

        [Display(Name = "Lembrar-me")]
        public bool RememberMe { get; set; }

    }
}
