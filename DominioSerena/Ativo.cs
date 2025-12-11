using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominioSerena
{
    public class Ativo
    {
        
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public decimal Valor { get; set; }
        public string observacoes { get; set; }
        public Categoria? Categoria { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public int? LocalizacaoId { get; set; }
        public Localizacao? Localizacao { get; set; }

        public int? ResponsavelId { get; set; }
        public Responsavel? Responsavel { get; set; }

        public EstadoAtivo Estado { get; set; } = EstadoAtivo.Disponivel;

        public DateTime DataAquisicao { get; set; }
        public string? NumeroDeSerie { get; set; }

       
        public Ativo(string nome, int categoriaId, DateTime dataAquisicao, string numeroDeSerie, int userId, decimal valor)
        {
            Nome = nome;
            CategoriaId = categoriaId;
            DataAquisicao = dataAquisicao;
            NumeroDeSerie = numeroDeSerie;
            UserId = userId;
            Valor = valor;
        }

    }
}
