using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EventoWeb.WS.Inscricao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscricoesController : ControllerBase
    {
        private readonly AppInscOnlineEventoAcessoInscricoes mAppInscricao;

        public InscricoesController(IContexto contexto)
        {
            mAppInscricao = new AppInscOnlineEventoAcessoInscricoes(contexto);
        }

        [HttpGet("basicoPorId/{id}")]
        public DTOBasicoInscricao ObterPorId(int id)
        {
            return mAppInscricao.ObterPorId(id);
        }

        [HttpGet("validarCodigo/{id}")]
        public DTOAcessoInscricao ValidarCodigo(int id, [FromBody]string codigo)
        {
            if (!mAppInscricao.ValidarCodigo(id, codigo))
                return null;
            else
            { 
                ClaimsIdentity identidade = new ClaimsIdentity(
                    new GenericIdentity(id.ToString() + codigo, "IdInscricao"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, id.ToString() + codigo)
                    }
                );

                using (var provider = new RSACryptoServiceProvider(2048))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                    {
                        Issuer = "EventoWeb",
                        Audience = "EventoWeb_Emissor",
                        SigningCredentials = new SigningCredentials(new RsaSecurityKey(provider.ExportParameters(true)), SecurityAlgorithms.RsaSha256Signature),
                        Subject = identidade,
                        NotBefore = DateTime.Now,
                        Expires = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59)
                    });

                    return new DTOAcessoInscricao
                    {
                        Autorizacao = handler.WriteToken(securityToken),
                        IdInscricao = id
                    };
                }
            }
        }

        [HttpPut("enviarCodigo/{identificacao}")]
        public DTOEnvioCodigoAcessoInscricao EnviarCodigo(string identificacao)
        {
            return mAppInscricao.EnviarCodigo(identificacao);
        }

        [HttpPost("criar/{id}")]
        public DTODadosConfirmacao CriarInscricao(int id, [FromBody] DTODadosCriarInscricao dadosInscricao)
        {

        }
    }
}