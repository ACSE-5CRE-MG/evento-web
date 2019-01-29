using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private AppDepartamentos mAppDepartamentos;

        public DepartamentoController(IContexto contexto)
        {
            mAppDepartamentos = new AppDepartamentos(contexto);
        }

        [Authorize("Bearer")]
        [HttpGet("obter")]
        public DTODepartamento GetObter(int id)
        {
            var departamento = mAppDepartamentos.ObterPorId(id);
            return departamento;
        }

        [Authorize("Bearer")]
        [HttpGet("listarTodos")]
        public IEnumerable<DTODepartamento> ListarTudo(int idEvento)
        {
            var lista = mAppDepartamentos.ObterTodos(idEvento);

            return lista;
        }

        // POST api/afrac/?idEvento=5
        [Authorize("Bearer")]
        [HttpPost("criar")]
        public DTOId Incluir(int idEvento, [FromBody] string nome)
        {
            var id = mAppDepartamentos.Incluir(idEvento, nome);

            return id;
        }

        [Authorize("Bearer")]
        [HttpPut("atualizar")]
        public void Alterar(int id, [FromBody] string nome)
        {
            mAppDepartamentos.Atualizar(id, nome);
        }

        [Authorize("Bearer")]
        [HttpDelete("excluir")]
        public void Excluir(int id)
        {
            mAppDepartamentos.Excluir(id);
        }

        /*[HttpGet]
        [ValidacaoToken]
        [SessaoPorRequisicao]
        [ActionName("relatorioDivisao")]
        public String GerarRelatorioDivisao(int idEvento)
        {
            Stream relatorio = null;

            Evento evento = mEventos.ObterEventoPeloId(idEvento);            

            relatorio = new GeradorRelatorioDivisaoDepartamento().Gerar(
                mInscricoes.ListarTodasInscricoesPorAtividade<AtividadeInscricaoDepartamento>(evento));

            using (var memoryStream = new MemoryStream())
            {
                relatorio.Position = 0;
                relatorio.CopyTo(memoryStream);
                var relatorioEmBytes = memoryStream.ToArray();

                return Convert.ToBase64String(relatorioEmBytes);
            }
        }*/
    }
}
