namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class AtividadeInscricaoOficinaSemEscolha : AAtividadeInscricao
    {
        public AtividadeInscricaoOficinaSemEscolha(InscricaoParticipante inscrito)
            : base(inscrito)
        {
        }

        protected AtividadeInscricaoOficinaSemEscolha() { }
    }
}
