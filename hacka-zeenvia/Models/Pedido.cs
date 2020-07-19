using System;
using System.Collections.Generic;

namespace hacka_zeenvia.Models
{
    public class Pedido
    {
        public int PedidoId { get; set; }

        public int FeiranteId { get; set; }
        public Feirante Feirante { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public List<ItemPedido> ItensPedido { get; set; }

        public decimal Total { get; set; }

        public DateTime DataPedido { get; set; }

        public Pedido()
        {
            ItensPedido = new List<ItemPedido>();
        }
    }
}
