using System;
using System.Collections.Generic;

namespace hacka_zeenvia.Models
{
    public class ItemPedido
    {
        public int ItemPedidoId { get; set; }

        public int FeiranteProdutoId { get; set; }
        public FeiranteProduto FeiranteProduto { get; set; }

        public int Quantidade { get; set; }

        public decimal Valor { get; set; }

        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        

        public ItemPedido()
        {
        }
    }
}
