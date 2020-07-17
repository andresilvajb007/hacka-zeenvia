using System;
namespace hacka_zeenvia.Models
{
    public class MensagemZAP
    {
        public int MensagemZAPId { get; set; }
        
        public string From { get; set; }
        
        public string To { get; set; }
        
        public string Direction { get; set; }
        
        public string Channel { get; set; }
        
        public string VisitorFullName { get; set; }
        
        public string Conteudo { get; set; }

        public MensagemZAP()
        {
        }
    }
}
