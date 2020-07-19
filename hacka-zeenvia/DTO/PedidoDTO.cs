using System;
using System.Collections.Generic;
using System.Linq;
using hacka_zeenvia.Models;

namespace hacka_zeenvia.DTO
{
    public class PedidoDTO
    {
        public int PedidoId { get; set; }

        public int FeiranteId { get; set; }

        public int ClienteId { get; set; }

        public List<ItemPedidoDTO> ItensPedidoDTO { get; set; }

        public decimal Total { get; set; }

        public DateTime DataPedido { get; set; }

        public PedidoDTO()
        {
            ItensPedidoDTO = new List<ItemPedidoDTO>();
        }

        public Pedido ToPedido()
        {
            var pedido = new Pedido();
            pedido.ClienteId = ClienteId;
            pedido.FeiranteId = FeiranteId;
            pedido.DataPedido = DateTime.Now;

            foreach (var itemPedido in ItensPedidoDTO)
            {
                pedido.ItensPedido.Add(itemPedido.ToItemPedido());
            }

            pedido.Total = pedido.ItensPedido.Sum(x => x.Valor * x.Quantidade);

            return pedido;
        }
    }
}
