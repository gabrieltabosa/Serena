using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominioSerena
{
    public class Localizacao
    {
        public int Id { get; set; }
        public string Nome { get; set; } // Ex: "Sala 10", "TI", "Almoxarifado"

        public ICollection<Ativo> Ativos { get; set; }

        public Localizacao()
        {
            
        }
        public Localizacao(string nome, Ativo ativo) {
            Nome = nome;
            Ativos = new List<Ativo> { ativo };
        }
    }
}
