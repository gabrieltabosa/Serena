using System.ComponentModel.DataAnnotations;

namespace DominioSerena
{
    public class ApoioViewModel
    {
        public int Id { get; set; }

        
        public string Nome { get; set; }

        [Display(Name = "Número")]
        public string Telefone { get; set; }
    }
}
