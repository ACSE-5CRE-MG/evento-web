﻿using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.WS.Secretaria.Controllers.DTOS;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private IContexto m_Contexto;

        public AutenticacaoController(IContexto contexto) 
        {
            m_Contexto = contexto;
        }

        [AllowAnonymous]
        [HttpPut("autenticar")]
        public DTWAutenticacao Autenticar([FromBody] DTWDadosAutenticacao dadosAutenticacao, 
            [FromServices]ConfiguracaoAutorizacao configuracaoAutorizacao)
        {
            var app = new AppUsuarioAutenticacao(m_Contexto)
            {
                Login = dadosAutenticacao.Login,
                Senha = dadosAutenticacao.Senha
            };

            var usuario = app.Autenticar();
            if (usuario != null)
                return new DTWAutenticacao
                {
                    Usuario = usuario,
                    Validade = DateTime.Now + TimeSpan.FromSeconds(configuracaoAutorizacao.TempoSegExpirar),
                    TokenAutenticacao = GerarTokenApi(configuracaoAutorizacao, usuario)
                };
            else
                return null;
        }       

        [Authorize("Bearer")]
        [HttpDelete("desautenticar")]
        public void Desautenticar()
        {
            HttpContext.SignOutAsync();
        }

        private static string GerarTokenApi(ConfiguracaoAutorizacao configuracaoAutorizacao, DTOUsuario usuario)
        {
            ClaimsIdentity identidade = new ClaimsIdentity(
                    new GenericIdentity(usuario.Login, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Login)
                    }
                );

            if (usuario.EhAdministrador)
                identidade.AddClaim(new Claim(ClaimTypes.Role, "ADM"));

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(configuracaoAutorizacao.TempoSegExpirar);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = configuracaoAutorizacao.Emissor,
                Audience = configuracaoAutorizacao.Publico,
                SigningCredentials = configuracaoAutorizacao.CredenciasAssinatura,
                Subject = identidade,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });

            return handler.WriteToken(securityToken);
        }        
    }
}
