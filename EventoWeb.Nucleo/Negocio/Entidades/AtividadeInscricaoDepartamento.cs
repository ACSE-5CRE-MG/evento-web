using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class AtividadeInscricaoDepartamento: AAtividadeInscricao
    {
        private Departamento mDepartamentoEscolhido;

        public AtividadeInscricaoDepartamento(InscricaoParticipante inscrito, Departamento departamentoEscolhido)
            : base(inscrito)
        {
            DepartamentoEscolhido = departamentoEscolhido;
            EhCoordenacao = false;
        }

        protected AtividadeInscricaoDepartamento() { }

        public virtual Departamento DepartamentoEscolhido
        {
            get { return mDepartamentoEscolhido; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("DepartamentoEscolhido");

                mDepartamentoEscolhido = value;
            }
        }

        public virtual Boolean EhCoordenacao { get; set; }
    }
}
