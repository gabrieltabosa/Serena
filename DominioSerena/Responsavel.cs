using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominioSerena
{
    public class Responsavel
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public string Email { get; set; }
        public ICollection<Ativo> Ativos { get; set; } = new List<Ativo>();

        public Responsavel(int id,string nome, string email)
        {
            Nome = nome;
            Email = email;
        }
    }
}
