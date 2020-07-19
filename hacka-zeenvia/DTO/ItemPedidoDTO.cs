using System;
using hacka_zeenvia.Models;

namespace hacka_zeenvia.DTO
{
    public class ItemPedidoDTO
    {
        public int ItemPedidoId { get; set; }

        public int FeiranteProdutoId { get; set; }

        public int Quantidade { get; set; }

        public decimal Valor { get; set; }

        public int PedidoId { get; set; }

        public ItemPedidoDTO()
        {
        }

        public ItemPedido ToItemPedido()
        {
            var itemPedido = new ItemPedido();
            itemPedido.FeiranteProdutoId = FeiranteProdutoId;
            itemPedido.Quantidade = Quantidade;
            itemPedido.Valor = Valor;

            return itemPedido;
        }
    }
}
