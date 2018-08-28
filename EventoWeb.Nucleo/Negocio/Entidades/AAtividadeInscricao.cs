using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public abstract class AAtividadeInscricao: Entidade
    {
        private InscricaoParticipante mInscrito;

        public AAtividadeInscricao(InscricaoParticipante inscrito)
        {
            if (inscrito == null)
                throw new ArgumentNullException("inscrito");
            mInscrito = inscrito;
        }

        protected AAtividadeInscricao() { }

        public virtual InscricaoParticipante Inscrito { get { return mInscrito; } }
    }
}
