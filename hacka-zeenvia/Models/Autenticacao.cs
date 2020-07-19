using System;
using System.Collections.Generic;
using System.Linq;

namespace hacka_zeenvia.Models
{
    public class Autenticacao
    {
        public int AtenticacaoId { get; set; }

        public string Codigo { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public DateTime Data { get; set; }

        public Autenticacao()
        {
            
        }

        public void GerarCodigo()
        {
            if (string.IsNullOrEmpty(Codigo))
            {
                Random random = new Random();
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                Codigo = new string(Enumerable.Repeat(chars, 6)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            }
        }
    }
}
