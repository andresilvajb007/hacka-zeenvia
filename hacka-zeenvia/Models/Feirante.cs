﻿using System;
using System.Collections.Generic;

namespace hacka_zeenvia.Models
{
    public class Feirante
    {
        public int FeiranteId { get; set; }

        public string Nome { get; set; }

        public string Celular { get; set; }

        public List<Produto> Produtos { get; set; }

        public Feirante()
        {
        }
    }
}
