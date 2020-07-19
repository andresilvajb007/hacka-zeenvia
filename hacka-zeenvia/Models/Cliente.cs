using System;
using System.Collections.Generic;

namespace hacka_zeenvia.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        public string Nome { get; set; }

        public string Celular { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }

        public Cliente()
        {
        }
    }
}
