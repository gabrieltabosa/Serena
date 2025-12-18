namespace Serena.Models
{
    public class EnderecoViewModel
    {
        public int Id { get; set; }
        public string Rua { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public string Bairro {  get; set; }
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        
    }
}
