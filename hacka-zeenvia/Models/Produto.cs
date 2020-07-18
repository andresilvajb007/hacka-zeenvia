﻿using System;
using System.Collections.Generic;

namespace hacka_zeenvia.Models
{
    public class Produto
    {
        public int ProdutoId { get; set; }

        public string Nome { get; set; }

        public ICollection<FeiranteProduto> FeiranteProdutos { get; private set; }

        public Produto()
        {
        }
    }
}
