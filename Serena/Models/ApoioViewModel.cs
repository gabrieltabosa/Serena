using System.ComponentModel.DataAnnotations;

namespace Serena.Models
{
    public class ApoioViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de apoio")]
        public string Tipo { get; set; }

        [Display(Name = "Número")]
        public string Numero { get; set; }
    }
}
