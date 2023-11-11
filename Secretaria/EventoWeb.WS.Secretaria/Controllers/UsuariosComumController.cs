using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.WS.Secretaria.Controllers.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosComumController : ControllerBase
    {
        private readonly IContexto m_Contexto;

        public UsuariosComumController(IContexto contexto)
        {
            m_Contexto = contexto;
        }

        [Authorize("Bearer")]
        [HttpGet("obter")]
        public DTOUsuario Obter()
        {            
            var app = new AppUsuarioObter(m_Contexto)
            {
                Login = User.Identity.Name
            };
            return app.Obter();
        }
        
        [Authorize("Bearer")]
        [HttpPut("atualizar")]
        public void Atualizar(DTOUsuario dto)
        {
            if (User.Identity.Name.ToUpper() != dto.Login.ToUpper())
                throw new Exception("Login dos dados de alteração diferente do login autenticado");

            var app = new AppUsuarioAlteracaoDados(m_Contexto)
            {
                DadosUsuario = dto
            };

            app.Alterar();
        }

        [Authorize("Bearer")]
        [HttpPut("atualizar-senha")]
        public void AlteraSenhaAdm(DTOAlteracaoSenhaComumWS dto)
        {
            var app = new AppUsuarioAlteracaoSenhaPeloUsuario(m_Contexto)
            {
                Login = User.Identity.Name,
                SenhaAtual = dto.SenhaAtual,
                NovaSenha = dto.NovaSenha,
                NovaSenhaRepeticao = dto.NovaSenhaRepetida
            };

            app.Alterar();
        }
    }
}
