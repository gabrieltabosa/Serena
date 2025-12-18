namespace Serena.Models
{
    public class DenunciaViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public EnderecoViewModel Endereco { get; set; }
        public string Status { get; set; } = string.Empty;
        public string TipoViolencia { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
