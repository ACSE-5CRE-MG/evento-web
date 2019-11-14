using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using RazorLight;

namespace EventoWeb.Nucleo.Persistencia.Comunicacao
{
    public class GeracaoMensagemEmailRazor : AGeracaoMensagemEmail
    {
        private RazorLightEngine m_MotorRazor;

        public GeracaoMensagemEmailRazor()
        {
            m_MotorRazor = new RazorLightEngineBuilder()
              .Build();
        }

        public override string GerarMensagemModelo<T>(string modeloMensagem, T objetoDados)
        {
            return m_MotorRazor.CompileRenderAsync("MODELO", modeloMensagem, objetoDados).Result;
        }
    }
}
