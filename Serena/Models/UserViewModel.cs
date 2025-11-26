using System.ComponentModel.DataAnnotations;

namespace Serena.Models
{
    public class UserViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string NomeCompleto { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
        public enum EstadoCivilEnum
        {
            Solteiro,
            Casado,
            Divorciado,
            Viuvo,
            UniaoEstavel
        }
        [Required]
        public EnderecoViewModel Endereco { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public string Cpf { get; set; }
        [Required]
        public int Telefone { get; set; }
    }
}
