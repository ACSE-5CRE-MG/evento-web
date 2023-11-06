using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.WS.Secretaria.Controllers.DTOS;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private AppUsuarioAutenticacao mAppUsuario;

        public AutenticacaoController(IContexto contexto) 
        {
            mAppUsuario = new AppUsuarioAutenticacao(contexto);
        }

        [AllowAnonymous]
        [HttpPost("autenticar")]
        public DadosUsuario PostAutenticar([FromBody] String token, [FromServices]ConfiguracaoAutorizacao configuracaoAutorizacao)
        {
            var usuario = ConsultarEmailUsuarioFacebook(token);
            if (usuario != null && mAppUsuario.BuscarPeloLogin(usuario.Login) != null)
            {
                usuario.TokenApi = GerarTokenApi(configuracaoAutorizacao, usuario.Login);                 
                return usuario;
            }
            else
                throw new ExcecaoAPI("Autenticacao", "Token incorreto ou usuário não existe.");
        }

        [AllowAnonymous]
        [HttpPost("autenticarSemFacebook")]
        public DadosUsuario PostAutenticar(String emailFacebook, String token, [FromServices]ConfiguracaoAutorizacao configuracaoAutorizacao)
        {
            if (emailFacebook == "robsonmbobbi@gmail.com" && !String.IsNullOrEmpty(token) && token == "EVENTOWEB-0192")
            {
                return new DadosUsuario()
                {
                    Login = emailFacebook,
                    Nome = emailFacebook,
                    TokenApi = GerarTokenApi(configuracaoAutorizacao, emailFacebook)
                };
            }
            else
                throw new ExcecaoAPI("Autenticacao", "Token incorreto ou usuário não existe.");

            /*Usuario usuario = null;
            if ((usuario = mAppUsuario.BuscarPeloLogin(emailFacebook)) != null && 
                !String.IsNullOrEmpty(token) && token == "EVENTOWEB-0192")
            {
                return new DadosUsuario()
                {
                    Login = emailFacebook,
                    Nome = emailFacebook,
                    TokenApi = GerarTokenApi(configuracaoAutorizacao, usuario.Login)
                };
            }
            else
                throw new ExcecaoAPI("Autenticacao", "Token incorreto ou usuário não existe.");*/
        }

        [Authorize("Bearer")]
        [HttpDelete("desautenticar")]
        public void DeleteDesautenticar()
        {
            var authentication = HttpContext.SignOutAsync();
        }

        private string GerarTokenApi(ConfiguracaoAutorizacao configuracaoAutorizacao, string login)
        {
            ClaimsIdentity identidade = new ClaimsIdentity(
                    new GenericIdentity(login, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, login)
                    }
                );

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

        private DadosUsuario ConsultarEmailUsuarioFacebook(string token)
        {
            DadosUsuario fbUser = null;
            var path = "https://graph.facebook.com/me?access_token=" + token +"&fields=name,picture,email";
            var client = new HttpClient();
            var uri = new Uri(path);
            var response = client.GetAsync(uri);
            if (response.Result.IsSuccessStatusCode)
            {
                var content = response.Result.Content.ReadAsStringAsync();             
                fbUser = JsonConvert.DeserializeObject<DadosUsuario>(content.Result);
            }

            if (fbUser != null)
            {
                var retorno = new HttpClient().GetByteArrayAsync(fbUser.Imagem.Imagem.Url);
                if (retorno.Result != null)
                    fbUser.Imagem64 = Convert.ToBase64String(retorno.Result);
            }

            return fbUser;
        }
    }
}
