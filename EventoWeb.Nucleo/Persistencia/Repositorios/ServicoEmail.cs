using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Negocio.Servicos;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class ServicoEmail : IServicoEmail
    {
        private IContexto m_Contexto;

        public ServicoEmail(IContexto contexto)
        {
            m_Contexto = contexto;
        }

        public void Enviar(Evento evento, ModeloMensagem mensagem)
        {
            throw new NotImplementedException();
        }

        public void EnviarEmailCodigoInscricao(Inscricao inscricao, string codigo)
        {
            var mensagemEnvio = m_Contexto.RepositorioMensagensEmailPadrao.Obter(inscricao.Evento.Id);
            if (mensagemEnvio != null)
            {
                var tradutor = new TraducaoVariaveisEmail(m_Contexto.RepositorioVariaveisEmailIncricao.ListarTodas(), inscricao);
                var mensagem = mensagemEnvio.MensagemInscricaoCodigoAcessoAcompanhamento
                    .GerarMensagemTraduzidaVariaveis(tradutor);
                Enviar(inscricao.Evento, mensagem);
            }
            else
                throw new Exception("Não há mensagens de email definida para o evento");
        }
    }
}
