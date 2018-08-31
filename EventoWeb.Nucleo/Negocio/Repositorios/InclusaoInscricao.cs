using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public class InclusaoInscricao
    {
        private AInscricoes mInscricoes;
        //private NotificacaoEmailInclusaoInscricao mNotificacaoEmailInclusaoInscricao; remodelar o envio de email
        private IList<IValidacaoInscricao> mValidacoes;

        public InclusaoInscricao(AInscricoes aInscricoes, 
            IList<IValidacaoInscricao> validacoes/*,
            NotificacaoEmailInclusaoInscricao notificacaoEmailInclusaoInscricao*/)
        {
            this.mInscricoes = aInscricoes;
            //this.mNotificacaoEmailInclusaoInscricao = notificacaoEmailInclusaoInscricao;
            mValidacoes = validacoes;
        }
        public void Persistir(Inscricao inscricao)
        {
            foreach (var validacao in mValidacoes)
            {
                if (validacao.PossoValidar(inscricao))
                    validacao.Validar(inscricao);
            }

            mInscricoes.Incluir(inscricao);
            //mNotificacaoEmailInclusaoInscricao.Notificar(inscricao);
        }
    }
}
