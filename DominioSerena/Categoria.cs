using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominioSerena
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ICollection<Ativo> Ativos { get; set; }


        public Categoria(string nome)
        {
            Nome = nome;
            
        }
    }
}
