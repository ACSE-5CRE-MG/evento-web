using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.WS.Secretaria.Controllers.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosAdmController : ControllerBase
    {
        private readonly IContexto m_Contexto;

        public UsuariosAdmController(IContexto contexto)
        {
            m_Contexto = contexto;
        }

        [Authorize("Bearer", Roles = "ADM")]
        [HttpGet("listar-todos")]
        public IList<DTOUsuario> Listar()
        {
            var app = new AppUsuarioListagem(m_Contexto);
            return app.ListarTodos();
        }

        [Authorize("Bearer", Roles = "ADM")]
        [HttpGet("obter/{nomeUsuario}")]
        public DTOUsuario Obter(string nomeUsuario)
        {
            var app = new AppUsuarioObter(m_Contexto)
            {
                Login = nomeUsuario
            };
            return app.Obter();
        }

        [Authorize("Bearer", Roles = "ADM")]
        [HttpPost("incluir")]
        public void Incluir(DTOUsuarioInclusao dto)
        {
            var app = new AppUsuarioInclusao(m_Contexto)
            {
                DadosUsuario = dto
            };

            app.Incluir();
        }

        [Authorize("Bearer", Roles = "ADM")]
        [HttpPut("atualizar")]
        public void Atualizar(DTOUsuario dto)
        {
            var app = new AppUsuarioAlteracaoDados(m_Contexto)
            {
                DadosUsuario = dto
            };

            app.Alterar();
        }

        [Authorize("Bearer", Roles = "ADM")]
        [HttpDelete("excluir/{nomeUsuario}")]
        public void Excluir(string nomeUsuario)
        {
            var app = new AppUsuarioExclusao(m_Contexto)
            {
                Login = nomeUsuario
            };
            app.Excluir();
        }

        [Authorize("Bearer", Roles = "ADM")]
        [HttpPut("atualizar-senha/{nomeUsuario}")]
        public void AlteraSenhaAdm(string nomeUsuario, DTOAlteracaoSenhaWS dto)
        {
            var app = new AppUsuarioAlteracaoSenhaPeloAdm(m_Contexto)
            {
                Login = nomeUsuario,
                NovaSenha = dto.NovaSenha,
                NovaSenhaRepeticao = dto.NovaSenhaRepetida
            };

            app.Alterar();
        }
    }
}
