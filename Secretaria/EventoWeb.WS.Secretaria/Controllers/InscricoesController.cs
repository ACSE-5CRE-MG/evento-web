using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Persistencia.Comunicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscricoesController : ControllerBase
    {
        private readonly AppInscricoes m_App;

        public InscricoesController(IContexto contexto)
        {
            m_App = new AppInscricoes(contexto,
                new AppEmailMsgPadrao(contexto, new ServicoEmail(), new GeracaoMensagemEmailRazor()));
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/listarTodas")]
        public IEnumerable<DTOBasicoInscricao> ListarTodas(int idEvento, EnumSituacaoInscricao situacao)
        {
            return m_App.ListarTodas(idEvento, situacao);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/excluir/{idInscricao}")]
        public void Excluir(int idEvento, int idInscricao)
        {
            m_App.Excluir(idEvento, idInscricao);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter/{idInscricao}")]
        public DTOInscricaoCompleta Obter(int idEvento, int idInscricao)
        {
            return m_App.Obter(idEvento, idInscricao);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/aceitar/{idInscricao}")]
        public void Aceitar(int idEvento, int idInscricao, [FromBody] DTOInscricaoAtualizacao atualizacao)
        {
            m_App.Aceitar(idEvento, idInscricao, atualizacao);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/rejeitar/{idInscricao}")]
        public void Rejeitar(int idEvento, int idInscricao)
        {
            m_App.Rejeitar(idEvento, idInscricao);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/completar/{idInscricao}")]
        public void CompletarEAceitar(int idEvento, int idInscricao, [FromBody] DTOInscricaoAtualizacao atualizacao)
        {
            m_App.CompletarEAceitar(idEvento, idInscricao, atualizacao);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/atualizar/{idInscricao}")]
        public void Atualizar(int idEvento, int idInscricao, [FromBody] DTOInscricaoAtualizacao atualizacao)
        {
            m_App.Atualizar(idEvento, idInscricao, atualizacao);
        }
    }
}