using System.ComponentModel.DataAnnotations;




namespace DominioSerena.DTOs
{
    public class DenunciaDto
    {
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; } = string.Empty;
        public int? UsuarioId { get; set; }
        public string? NomeDenunciante { get; set; }
        public string? Cpf { get; set; }
        public string TipoViolencia { get; set; }
        public EnderecoDTO? Endereco { get; set; }
    }
}
