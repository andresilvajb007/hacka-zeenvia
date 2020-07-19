using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using hacka_zeenvia.Models;
using hacka_zeenvia.Models.SendMessageZenvia;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace hacka_zeenvia.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClienteController : ControllerBase
    {

        private readonly Context _context;        

        private readonly ILogger<Cliente> _logger;

        public ClienteController(ILogger<Cliente> logger,
                                 Context context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Incluir([FromBody] Cliente model)
        {

            var json = JsonConvert.SerializeObject(model);
            _logger.LogInformation($"Acessando POST {nameof(ClienteController)} {nameof(model)}: {json}");
          
            _context.Cliente.Add(model);
            _context.SaveChanges();


            return Ok();
           

        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Excluir(int id)
        {
            _logger.LogInformation($"Acessando DELETE Cliente/{id}");

            var Cliente =  _context.Cliente.Find(id);

            if (Cliente == null)
            {
                return NotFound();
            }

            _context.Cliente.Remove(Cliente);
            _context.SaveChanges();

            return Ok();
        }


        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  IActionResult Get([FromQuery] String nome, int? clienteId)
        {
            _logger.LogInformation($"Acessando GET  Cliente {nameof(nome)}: {nome} , {nameof(clienteId)}: {clienteId}");

            var Clientes = _context.Cliente
                                      .Where(x => (clienteId == null || x.ClienteId == clienteId) &&
                                                  (string.IsNullOrEmpty(nome) || x.Nome.ToLower().Contains(nome.ToLower()))
                                             ).ToList();

            if (Clientes == null || Clientes.Count() == 0)
            {
                return NotFound();
            }

            return Ok(Clientes);
        }

        [HttpPost("mensagem-enviada")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult MensagemEnviada([FromBody] EventHook eventHook)
        {
            var jsonEventHook = JsonConvert.SerializeObject(eventHook);
            _logger.LogInformation($"Acessando POST mensagem-enviada {nameof(ClienteController)} {nameof(eventHook)}: {jsonEventHook}");

            foreach (var conteudo in eventHook.Message.Contents.Where(x => x.Type == "text"))
            {
                var mensagem = new MensagemZAP
                {
                    From = eventHook.Message.From,
                    Channel = eventHook.Message.Channel,
                    Direction = eventHook.Message.Direction,
                    To = eventHook.Message.To,
                    Conteudo = conteudo.Text,
                    VisitorFullName = eventHook.Message.Visitor.Name

                };

                _context.MensagemZAP.Add(mensagem);


                var cliente = _context.Cliente.Where(x => x.Celular == mensagem.From).FirstOrDefault();

                if(cliente == null)
                {
                    cliente = new Cliente();
                    cliente.Celular = mensagem.From;
                    cliente.Nome = mensagem.VisitorFullName;

                    _context.Cliente.Add(cliente);
                }

                _context.SaveChanges();

                if(mensagem.Conteudo == "oi" && mensagem.Direction == "IN")
                {
                    Autenticacao autenticacao = new Autenticacao();                    
                    autenticacao.ClienteId = cliente.ClienteId;
                    autenticacao.Data = DateTime.Now;
                    autenticacao.GerarCodigo();

                    _context.Autenticacao.Add(autenticacao);
                    _context.SaveChanges();

                    var mensagemAutenticacao = $"Digite:{autenticacao.Codigo} para completar a autenticação:";
                    

                    var sender = new SenderMessageRequest();
                    sender.From = "furry-time";
                    sender.To = cliente.Celular;
                    sender.Contents = new List<Models.SendMessageZenvia.Content>();
                    sender.Contents.Add(new Models.SendMessageZenvia.Content { Type = "text", Text = mensagemAutenticacao, Payload = string.Empty });


                    var json = JsonConvert.SerializeObject(sender);                                    
                    var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://api.zenvia.com/v1/channels/whatsapp/messages");
                    httpRequestMessage.Headers.Add("X-API-TOKEN", "sxyGdagDRB3AFLl51p_y5gGzXnIyx2w4qmzR");
                    httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var httpClient = new HttpClient();
                    var response = httpClient.SendAsync(httpRequestMessage).Result;
                }



            }

            return Ok();

        }



        [HttpGet("autentica")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Autenticar(string codigo)
        {
            var autenticacao = _context.Autenticacao
                                       .Include(x=>x.Cliente) 
                                       .Where(x => x.Codigo == codigo)
                                       .FirstOrDefault();

            if(autenticacao != null)
            {
                return Ok(new { idCliente = autenticacao.ClienteId, nomeCliente = autenticacao.Cliente.Nome  });
            }
            else
            {
                return Ok(new { idCliente = 0, nomeCliente = "nao autenticado" });
            }
           
        }

       }
    }
