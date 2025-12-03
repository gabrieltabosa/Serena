namespace Serena.Models
{
    public class AtivoViewModel
    {
        public string Nome { get; set; } = string.Empty;
        public int CategoriaId { get; set; }

        public string CategoriaName { get; set; }
        public string Observacoes { get; set; }

        public string? LocalizacaoName { get; set; }
    }
}
