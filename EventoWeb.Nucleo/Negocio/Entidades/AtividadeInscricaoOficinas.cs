using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class AtividadeInscricaoOficinas : AAtividadeInscricao
    {
        private IList<Afrac> mAfracs;

        public AtividadeInscricaoOficinas(InscricaoParticipante inscrito, GestaoAfracsEscolhidas gestaoEscolhaAfracs)
            : base(inscrito)
        {
            AtualizarAfracs(gestaoEscolhaAfracs);
        }

        protected AtividadeInscricaoOficinas() { }

        public virtual void AtualizarAfracs(GestaoAfracsEscolhidas gestaoEscolhaAfracs)
        {
            if (gestaoEscolhaAfracs == null)
                throw new ArgumentException("É preciso informar as escolhas feitas das oficinas", "afracs");

            mAfracs = new List<Afrac>(gestaoEscolhaAfracs.GerarLista());
        }

        public virtual IEnumerable<Afrac> Afracs { get { return mAfracs; } }
    }

    public class AtividadeInscricaoOficinasCoordenacao : AAtividadeInscricao
    {
        private Afrac mAfracEscolhida;

        public AtividadeInscricaoOficinasCoordenacao(InscricaoParticipante inscrito, Afrac afrac)
            : base(inscrito)
        {
            AfracEscolhida = afrac;
        }

        protected AtividadeInscricaoOficinasCoordenacao() { }

        public virtual Afrac AfracEscolhida
        {
            get { return mAfracEscolhida; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("AfracEscolhida");

                mAfracEscolhida = value;
            }
        }
    }
}
