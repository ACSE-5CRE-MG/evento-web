using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        private const string TIPO_CONTEUDO_PDF = "application/pdf";
        private readonly IContexto m_Contexto;

        public RelatoriosController(IContexto contexto)
        {
            m_Contexto = contexto;
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/divisao-salas")]
        public IActionResult GerarRelatorioDivisaoSalas(int idEvento)
        {
            var appRelDivisaoSalas = new AppRelatorioDivisaoSalas(m_Contexto, 
                m_Contexto.RepositorioEventos, m_Contexto.RepositorioSalasEstudo, m_Contexto.RepositorioInscricoes,
                m_Contexto.RelatorioDivisaoSalasEstudo);

            return File(appRelDivisaoSalas.GerarImpressoPDF(idEvento), TIPO_CONTEUDO_PDF);
        }
    }
}