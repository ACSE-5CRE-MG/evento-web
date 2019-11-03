using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.WS.Inscricao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManutencaoInscricoesController : ControllerBase
    {
        private readonly AppInscOnlineEventoManutencaoInscricoes mAppInscricao;

        public ManutencaoInscricoesController(IContexto contexto)
        {
            mAppInscricao = new AppInscOnlineEventoManutencaoInscricoes(contexto);
        }

        [HttpGet("obterInscricao/{id}")]
        public DTOInscricaoCompleta ObterInscricao(int id)
        {
            return mAppInscricao.ObterInscricao(id);
        }

        [HttpPut("atualizarInscricao/{id}")]
        public void AtualizarInscricao(int id, [FromBody]DTOInscricaoAtualizacao dtoInscricao)
        {
            mAppInscricao.AtualizarInscricao(id, dtoInscricao);
        }

        [HttpGet("evento/{idEvento}/obterSarau/{codigo}")]
        public DTOSarau ObterSarau(int idEvento, string codigo) 
        {
            return mAppInscricao.ObterSarau(idEvento, codigo);
        }

        [HttpGet("evento/{idEvento}/obterCrianca/{codigo}")]
        public DTOCrianca ObterCrianca(int idEvento, string codigo)
        {
            return mAppInscricao.ObterInscricaoInfantil(idEvento, codigo);
        }
    }
}