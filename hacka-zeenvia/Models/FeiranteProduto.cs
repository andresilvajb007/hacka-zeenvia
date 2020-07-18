using System;
namespace hacka_zeenvia.Models
{
    public class FeiranteProduto
    {
        public int FeiranteProdutoId { get; set; }

        public int FeiranteId { get; set; }
        public virtual Feirante Feirante { get; set; }

        public int ProdutoId { get; set; }
        public virtual Produto Produto { get; set; }

        public decimal Preco { get; set; }

        public FeiranteProduto()
        {
        }
    }
}
