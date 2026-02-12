using System.ComponentModel.DataAnnotations;

namespace DominioSerena
{
    public class EnderecoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O logradouro (rua) é obrigatório.")]
        [StringLength(150, ErrorMessage = "O logradouro deve ter no máximo 150 caracteres.")]
        [Display(Name = "Rua/Logradouro")]
        public string Rua { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número é obrigatório.")]
        [Range(1, 999999, ErrorMessage = "Informe um número válido.")]
        [Display(Name = "Nº")]
        public int Numero { get; set; }

        [StringLength(100, ErrorMessage = "O complemento deve ter no máximo 100 caracteres.")]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [StringLength(100, ErrorMessage = "O bairro deve ter no máximo 100 caracteres.")]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        [StringLength(100, ErrorMessage = "A cidade deve ter no máximo 100 caracteres.")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estado (UF) é obrigatório.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Use a sigla do estado (ex: SP, RJ).")]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "O estado deve conter apenas 2 letras maiúsculas.")]
        public string Estado { get; set; } = string.Empty;

    }
}
