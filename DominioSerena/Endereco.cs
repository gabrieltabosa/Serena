using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominioSerena
{
    public class Endereco
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public string CEP { get; set; }

        public Endereco()
        {

        }

        public Endereco(string rua,int numero, string cidade, string estado, string cep )
        {
            Rua = rua;
            Numero = numero;
            Cidade = cidade;
            Estado = estado;
            CEP = cep;
        }

    }
}
