using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Persistencia.Comunicacao;
using Microsoft.AspNetCore.Authorization;
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
            mAppInscricao = new AppInscOnlineEventoManutencaoInscricoes(contexto,
                new AppEmailMsgPadrao(contexto, new ServicoEmail(), new GeracaoMensagemEmailRazor()));
        }

        [Authorize(Policy = "Bearer")]
        [HttpGet("obterInscricao/{id}")]
        public DTOInscricaoCompleta ObterInscricao(int id)
        {
            return mAppInscricao.ObterInscricao(id);
        }

        [Authorize(Policy = "Bearer")]
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