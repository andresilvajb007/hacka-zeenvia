using System;
namespace hacka_zeenvia.Models
{
    public class Produto
    {
        public int ProdutoId { get; set; }

        public string Nome { get; set; }

        public int FeiranteId { get; set; }

        public Feirante Feirante { get; set; }

        public Produto()
        {
        }
    }
}
