using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class InscricaoInfantil : Inscricao
    {
        private const String MSG_ERRO_RESPONSAVEL = "A inscrição do responsável não pode ser do tipo infantil.";
        private const String MSG_ERRO_INSCRICAO_OUTRO_EVENTO = "A inscrição do responsável deve ser do mesmo evento da inscrição infantil.";

        private Inscricao m_InscricaoResponsavel1;
        private Inscricao m_InscricaoResponsavel2;

        protected InscricaoInfantil() { }

        public InscricaoInfantil(Evento evento, Pessoa pessoa, Inscricao inscricaoResponsavel, DateTime dataRecebimento)
            : base(evento, pessoa, dataRecebimento)
        {
            AtribuirResponsaveis(inscricaoResponsavel, null);
            TornarPendente();
        }

        public InscricaoInfantil(Pessoa pessoa, Evento evento, Inscricao inscricaoResponsavel,
            Inscricao inscricaoResponsavel2, DateTime dataRecebimento)
            : base(evento, pessoa, dataRecebimento)
        {
            if (inscricaoResponsavel2 == null)
                throw new ArgumentNullException("InscricaoResponsavel2", "Responsável deve ser informado.");

            AtribuirResponsaveis(inscricaoResponsavel, inscricaoResponsavel2);
        }

        public virtual Inscricao InscricaoResponsavel1 
        { 
            get { return m_InscricaoResponsavel1; }
        }
        public virtual Inscricao InscricaoResponsavel2 
        { 
            get { return m_InscricaoResponsavel2; }
        }

        public virtual void AtribuirResponsaveis(Inscricao responsavel1, Inscricao responsavel2)
        {
            if (responsavel1 == null)
                throw new ArgumentNullException("InscricaoResponsavel1", "Responsável deve ser informado.");

            if (responsavel1.GetType() == typeof(InscricaoInfantil))
                throw new ArgumentException(MSG_ERRO_RESPONSAVEL, "InscricaoResponsavel1");

            if (Evento != responsavel1.Evento)
                throw new ArgumentException(MSG_ERRO_INSCRICAO_OUTRO_EVENTO, "InscricaoResponsavel1");

            if (responsavel2 != null)
            {
                if (responsavel2.GetType() == typeof(InscricaoInfantil))
                    throw new ArgumentException(MSG_ERRO_RESPONSAVEL, "InscricaoResponsavel2");

                if (Evento != responsavel2.Evento)
                    throw new ArgumentException(MSG_ERRO_INSCRICAO_OUTRO_EVENTO, "InscricaoResponsavel2");

                if (responsavel1 == responsavel2)
                    throw new ArgumentException("Os responsáveis devem ser diferentes.", "InscricaoResponsavel1/InscricaoResponsavel2");
            }

            if (DormeEvento)
                ValidarInscricaoParaQuartoFamilia(responsavel1, responsavel2);

            m_InscricaoResponsavel1 = responsavel1;
            m_InscricaoResponsavel2 = responsavel2;
        }

        private void ValidarInscricaoParaQuartoFamilia(Inscricao responsavel1, Inscricao responsavel2)
        {
            var podeDormirQuartoFamilia = PoderaDormirQuartoFamilia(responsavel1) || PoderaDormirQuartoFamilia(responsavel2);
            if (!podeDormirQuartoFamilia)
                throw new ArgumentException("Para uma criança que dormirá no evento, pelo um dos responsáveis deve dormir no evento.", "InscricaoResponsavel1/InscricaoResponsavel2");
        }

        private bool PoderaDormirQuartoFamilia(Inscricao responsavel)
        {
            return responsavel != null && responsavel.DormeEvento; 
        }

        public override bool EhValidaIdade(int idade)
        {
            return idade < Evento.IdadeMinimaInscricaoAdulto;
        }

        protected override void ValidarInscricaoParaSeTornarPendente()
        {
        }
    }
}
