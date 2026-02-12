using DominioSerena;

namespace DominioSerena
{
    public class DenunciaViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int? UsuarioId { get; set; }
        public string? NomeDenunciante { get; set; }
        public string? Cpf { get; set; }
        public EnderecoViewModel Endereco { get; set; }
        public string Status { get; set; } = string.Empty;
        public TipoViolenciaViewModel TipoViolencia { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
