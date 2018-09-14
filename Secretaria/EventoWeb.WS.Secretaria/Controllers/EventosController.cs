using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.WS.Secretaria.Controllers.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private AppEvento mAppEvento;

        public EventosController(IContexto contexto)
        {
            mAppEvento = new AppEvento(contexto);
        }

        // GET api/eventos/obter-todos
        [HttpGet("obter-todos")]
        [Authorize("Bearer")]
        public ActionResult<IEnumerable<DTOEventoMinimo>> Get()
        {
            return mAppEvento.ObterTodos().Select(x => new DTOEventoMinimo()
            {
                Id = x.Id,
                DataFimInscricao = x.DataFimInscricao,
                DataInicioInscricao = x.DataInicioInscricao,
                Situacao = x.Situacao,
                Nome = x.Nome,
                Logotipo = x.Logotipo
            })
            .ToList();
        }

        // GET api/eventos/obter-id/5
        [HttpGet("obter-id/{id}")]
        [Authorize("Bearer")]
        public ActionResult<DTOEventoCompleto> Get(int id)
        {
            var evento = mAppEvento.ObterPorId(id);

            if (evento == null)
                return null;
            else 
                return new DTOEventoCompleto
                {
                    Id = evento.Id,
                    DataFimInscricao = evento.DataFimInscricao.ToString("dd/MM/yyyy HH:mm"),
                    DataInicioInscricao = evento.DataInicioInscricao.ToString("dd/MM/yyyy HH:mm"),
                    DataFimEvento = evento.DataFimEvento.ToString("dd/MM/yyyy HH:mm"),
                    DataInicioEvento = evento.DataInicioEvento.ToString("dd/MM/yyyy HH:mm"),
                    DataRegistro = evento.DataRegistro.ToString("dd/MM/yyyy HH:mm"),
                    Logotipo = evento.Logotipo,
                    Nome = evento.Nome,
                    ValorInscricao = evento.ValorInscricao,
                    Situacao = evento.Situacao,
                    PodeAlterar = evento.Situacao != SituacaoEvento.Concluido,
                    EnderecoEmail = evento.ConfiguracaoEmail.EnderecoEmail,
                    UsuarioEmail = evento.ConfiguracaoEmail.UsuarioEmail,
                    SenhaEmail = evento.ConfiguracaoEmail.SenhaEmail,
                    ServidorEmail = evento.ConfiguracaoEmail.ServidorEmail,
                    PortaServidor = evento.ConfiguracaoEmail.PortaServidor,
                    TipoSeguranca = evento.ConfiguracaoEmail.TipoSeguranca == TipoSegurancaEmail.Nenhuma ? "Nenhuma" : "SSL",
                    TituloEmailConfirmacaoInscricao = evento.ConfiguracaoEmail.TituloEmailConfirmacaoInscricao,
                    MensagemEmailConfirmacaoInscricao = evento.ConfiguracaoEmail.MensagemEmailConfirmacaoInscricao,
                    TemDepartamentalizacao = evento.TemDepartamentalizacao,
                    TemDormitorios = evento.TemDormitorios,
                    TemOficinas = evento.TemOficinas,
                    TemSalasEstudo = evento.TemSalasEstudo,
                    TemEvangelizacao = evento.TemEvangelizacao,
                    PublicoEvangelizacao = evento.PublicoEvangelizacao,
                    TemSarau = evento.TemSarau,
                    TempoDuracaoSarauMin = evento.TempoDuracaoSarauMin,
                    ModeloDivisaoSalaEstudo = evento.ModeloDivisaoSalasEstudo
                };
        }

        // POST api/eventos/incluir
        [HttpPost("incluir")]
        [Authorize("Bearer")]
        public void Post([FromBody] DTOEvento eventoDTO)
        {
            Evento evento = new Evento(eventoDTO.Nome,
                DateTime.Parse(eventoDTO.DataInicioInscricao), DateTime.Parse(eventoDTO.DataFimInscricao),
                DateTime.Parse(eventoDTO.DataInicioEvento), DateTime.Parse(eventoDTO.DataFimEvento),
                eventoDTO.ValorInscricao);
            evento.Logotipo = eventoDTO.Logotipo;
            evento.TemDepartamentalizacao = eventoDTO.TemDepartamentalizacao;
            evento.TemDormitorios = eventoDTO.TemDormitorios;
            evento.TemOficinas = eventoDTO.TemOficinas;

            if (eventoDTO.TemSalasEstudo)
                evento.UtilizaSalasEstudo(eventoDTO.ModeloDivisaoSalaEstudo.Value);
            else
                evento.NaoUtilizaSalasEstudo();

            evento.ConfigurarEvangelizacao(eventoDTO.TemEvangelizacao, eventoDTO.PublicoEvangelizacao);
            evento.ConfigurarSarau(eventoDTO.TemSarau, eventoDTO.TempoDuracaoSarauMin);

            evento.ConfiguracaoEmail.EnderecoEmail = eventoDTO.EnderecoEmail;
            evento.ConfiguracaoEmail.UsuarioEmail = eventoDTO.UsuarioEmail;
            evento.ConfiguracaoEmail.SenhaEmail = eventoDTO.SenhaEmail;
            evento.ConfiguracaoEmail.ServidorEmail = eventoDTO.ServidorEmail;
            evento.ConfiguracaoEmail.PortaServidor = eventoDTO.PortaServidor;
            evento.ConfiguracaoEmail.TituloEmailConfirmacaoInscricao = eventoDTO.TituloEmailConfirmacaoInscricao;
            evento.ConfiguracaoEmail.MensagemEmailConfirmacaoInscricao = eventoDTO.MensagemEmailConfirmacaoInscricao;

            if (String.IsNullOrEmpty(eventoDTO.TipoSeguranca))
                evento.ConfiguracaoEmail.TipoSeguranca = null;
            else
                evento.ConfiguracaoEmail.TipoSeguranca = eventoDTO.TipoSeguranca == "Nenhuma" ? TipoSegurancaEmail.Nenhuma : TipoSegurancaEmail.SSL;

            mAppEvento.Incluir(evento);
        }

        // PUT api/eventos/atualizar/5
        [HttpPut("atualizar/{id}")]
        [Authorize("Bearer")]
        public void Put(int id, [FromBody] DTOEvento eventoDTO)
        {
            Evento evento = ObterEvento(id);

            evento.Nome = eventoDTO.Nome;
            evento.DefinirDataInscricao(DateTime.Parse(eventoDTO.DataInicioInscricao), DateTime.Parse(eventoDTO.DataFimInscricao));
            evento.DefinirDataEvento(DateTime.Parse(eventoDTO.DataInicioEvento), DateTime.Parse(eventoDTO.DataFimEvento));
            evento.Logotipo = eventoDTO.Logotipo;
            evento.ValorInscricao = eventoDTO.ValorInscricao;
            evento.TemDepartamentalizacao = eventoDTO.TemDepartamentalizacao;
            evento.TemDormitorios = eventoDTO.TemDormitorios;
            evento.TemOficinas = eventoDTO.TemOficinas;

            if (eventoDTO.TemSalasEstudo)
                evento.UtilizaSalasEstudo(eventoDTO.ModeloDivisaoSalaEstudo.Value);
            else
                evento.NaoUtilizaSalasEstudo();

            evento.ConfigurarEvangelizacao(eventoDTO.TemEvangelizacao, eventoDTO.PublicoEvangelizacao);
            evento.ConfigurarSarau(eventoDTO.TemSarau, eventoDTO.TempoDuracaoSarauMin);

            evento.ConfiguracaoEmail.EnderecoEmail = eventoDTO.EnderecoEmail;
            evento.ConfiguracaoEmail.UsuarioEmail = eventoDTO.UsuarioEmail;
            evento.ConfiguracaoEmail.SenhaEmail = eventoDTO.SenhaEmail;
            evento.ConfiguracaoEmail.ServidorEmail = eventoDTO.ServidorEmail;
            evento.ConfiguracaoEmail.PortaServidor = eventoDTO.PortaServidor;
            evento.ConfiguracaoEmail.TituloEmailConfirmacaoInscricao = eventoDTO.TituloEmailConfirmacaoInscricao;
            evento.ConfiguracaoEmail.MensagemEmailConfirmacaoInscricao = eventoDTO.MensagemEmailConfirmacaoInscricao;

            if (String.IsNullOrEmpty(eventoDTO.TipoSeguranca))
                evento.ConfiguracaoEmail.TipoSeguranca = null;
            else
                evento.ConfiguracaoEmail.TipoSeguranca = eventoDTO.TipoSeguranca == "Nenhuma" ? TipoSegurancaEmail.Nenhuma : TipoSegurancaEmail.SSL;

            mAppEvento.Atualizar(evento);
        }

        // PUT api/eventos/concluir/5
        [HttpPut("concluir/{id}")]
        [Authorize("Bearer")]
        public void Put(int id)
        {
            Evento evento = ObterEvento(id);
            evento.concluir();
            mAppEvento.Atualizar(evento);
        }

        // DELETE api/eventos/excluir/5
        [HttpDelete("excluir/{id}")]
        [Authorize("Bearer")]
        public void Delete(int id)
        {
            mAppEvento.Excluir(ObterEvento(id));
        }

        private Evento ObterEvento(int id)
        {
            Evento evento = mAppEvento.ObterPorId(id);

            if (evento != null)
                return evento;
            else
                throw new ExcecaoAPI("Eventos", "Não foi encontrado nenhum evento com o id informado.");
        }
    }
}
