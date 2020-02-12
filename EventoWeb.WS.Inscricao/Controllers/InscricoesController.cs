using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Persistencia.Comunicacao;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.WS.Inscricao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscricoesController : ControllerBase
    {
        private readonly AppInscOnlineEventoAcessoInscricoes mAppInscricao;
        private readonly ConfiguracaoJwtBearer mConfiguracaoJwt;

        public InscricoesController(IContexto contexto, ConfiguracaoJwtBearer configuracaoJwt)
        {
            mAppInscricao = new AppInscOnlineEventoAcessoInscricoes(contexto, 
                new AppEmailMsgPadrao(contexto, new ServicoEmail(), new GeracaoMensagemEmailRazor()));
            mConfiguracaoJwt = configuracaoJwt;
        }

        [HttpGet("basicoPorId/{id}")]
        public DTOBasicoInscricao ObterPorId(int id)
        {
            return mAppInscricao.ObterPorId(id);
        }

        [HttpPut("validarCodigo/{id}")]
        public DTOAcessoInscricao ValidarCodigo(int id, [FromBody]string codigo)
        {
            if (!mAppInscricao.ValidarCodigo(id, codigo))
                return null;
            else
                return new DTOAcessoInscricao
                {
                    Autorizacao = mConfiguracaoJwt.GerarTokenJWTBearer(id.ToString() + codigo),
                    IdInscricao = id
                };            
        }

        [HttpPut("enviarCodigo/{identificacao}")]
        public DTOEnvioCodigoAcessoInscricao EnviarCodigo(string identificacao)
        {
            return mAppInscricao.EnviarCodigo(identificacao);
        }

        [HttpPost("criar/{idEvento}")]
        public DTODadosConfirmacao CriarInscricao(int idEvento, [FromBody] DTODadosCriarInscricao dadosInscricao)
        {
            return mAppInscricao.CriarInscricao(idEvento, dadosInscricao);
        }

        [HttpPut("validarCodigoEmail")]
        public bool ValidarCodigoEmail(DTOValidacaoCodigoEmail validar)
        {
            return mAppInscricao.ValidarCodigoEmail(validar.Identificacao, validar.Codigo);
        }

        [HttpPut("enviarCodigoEmail")]
        public void EnviarCodigoEmail(DTOEnvioCodigoEmail envio)
        {
            mAppInscricao.EnviarCodigoEmail(envio.Identificacao, envio.Email);
        }
    }

    public class DTOEnvioCodigoEmail
    {
        public string Identificacao { get; set; }
        public string Email { get; set; }
    }

    public class DTOValidacaoCodigoEmail
    {
        public string Identificacao { get; set; }
        public string Codigo { get; set; }
    }
}