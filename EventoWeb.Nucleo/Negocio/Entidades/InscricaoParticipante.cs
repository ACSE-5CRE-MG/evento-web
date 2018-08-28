using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum EnumTipoParticipante { Trabalhador, Participante, ParticipanteTrabalhador }

    public class InscricaoParticipante: Inscricao
    {
        private IList<AAtividadeInscricao> mAtividades;
        private EnumTipoParticipante mTipo;

        public InscricaoParticipante(Pessoa pessoa, Evento evento, DateTime dataRecebimento, EnumTipoParticipante tipo = EnumTipoParticipante.Participante) :
            base(pessoa, evento, dataRecebimento)
        {
            mAtividades = new List<AAtividadeInscricao>();
            mTipo = tipo;
        }

        protected InscricaoParticipante() { }

        public virtual IEnumerable<AAtividadeInscricao> Atividades { get { return mAtividades; } }

        public virtual EnumTipoParticipante Tipo
        {
            get { return mTipo; }
            set { mTipo = value; }
        }

        public virtual void AdicionarAtividade(AAtividadeInscricao atividade)
        {
            if (atividade.Inscrito != this)
                throw new ArgumentException("Atividade é de outra inscrição", "atividade");

            if (mAtividades.Count(x=> x.GetType() == atividade.GetType()) > 0)
                throw new ArgumentException("Não é possível ter a mesma atividade mais de uma vez", "atividade");

            if (!Evento.TemDepartamentalizacao && atividade.GetType() == typeof(AtividadeInscricaoDepartamento) ||
                !Evento.TemOficinas && atividade.GetType() == typeof(AtividadeInscricaoOficinas) ||
                !Evento.TemSalasEstudo && atividade.GetType() == typeof(AtividadeInscricaoSalaEstudo))
                throw new ArgumentException("Tipo de atividade não é aceita pelo evento", "atividade");

            mAtividades.Add(atividade);
        }

        public virtual void RemoverTodasAtividade()
        {
            mAtividades.Clear();
        }

        public override bool EhValidaIdade(int idade)
        {
            return idade >= 13;
        }
    }
}
