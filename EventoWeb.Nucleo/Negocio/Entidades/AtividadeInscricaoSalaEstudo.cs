using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class AtividadeInscricaoSalaEstudo : AAtividadeInscricao
    {
        public AtividadeInscricaoSalaEstudo(InscricaoParticipante inscrito)
            : base(inscrito)
        {
        }

        protected AtividadeInscricaoSalaEstudo() { }
    }

    public class AtividadeInscricaoSalaEstudoCoordenacao : AAtividadeInscricao
    {
        private SalaEstudo mSalaEscolhida;

        public AtividadeInscricaoSalaEstudoCoordenacao(InscricaoParticipante inscrito, SalaEstudo sala)
            : base(inscrito)
        {
            SalaEscolhida = sala;
        }

        protected AtividadeInscricaoSalaEstudoCoordenacao() { }

        public virtual SalaEstudo SalaEscolhida
        {
            get { return mSalaEscolhida; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("SalaEscolhida");

                mSalaEscolhida = value;
            }
        }
    }
}
