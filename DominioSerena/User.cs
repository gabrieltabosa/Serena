using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace DominioSerena
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string NomeCompleto { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }


        
        public Endereco Endereco { get; set; }

       
        public DateTime DataNascimento { get; set; }

        [Required]
        [MaxLength(14)]
        public string Cpf { get; set; }
        
        public string Telefone { get; set; }
        
        public EstadoCivil EstadoCivil { get; set; }
        
        public string Rg { get; set; }

        public int ?apoios { get; set; }

        public ICollection<Ativo> Ativos { get; private set; } = new List<Ativo>();

        public User()
        {

        }
        public User(string nomeCompleto, string email, string password, Endereco endereco, DateTime data, string cpf, string telefone, EstadoCivil estadoCivil, string rg)
        {
            NomeCompleto = nomeCompleto;
            Email = email;
            Password = password;
            Endereco = endereco;
            DataNascimento = data;
            Cpf = cpf;
            Telefone = telefone;
            EstadoCivil = estadoCivil;
            Rg = rg;
            
        }

    }
}
