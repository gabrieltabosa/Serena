namespace Serena.Models
{
    public class DenunciaViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public EnderecoViewModel? Endereco { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Violencia { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; }
    }
}
