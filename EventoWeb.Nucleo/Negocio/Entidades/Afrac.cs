using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Afrac: Entidade
    {
        private string mNome;
        private Evento mEvento;
        private int? mNumeroTotalParticipantes;
        private bool mDeveSerParNumeroTotalParticipantes;
        private IList<InscricaoParticipante> mParticipantes;

        public Afrac(Evento evento, string nome)
        {
            Evento = evento;
            Nome = nome;

            mParticipantes = new List<InscricaoParticipante>();
        }

        protected Afrac() { }

        public virtual String Nome
        {
            get { return mNome; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Nome", "O nome não pode ser nulo.");

                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Nome não pode ser vazio.");

                mNome = value;
            }
        }

        public virtual Evento Evento
        {
            get { return mEvento; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Evento", "Evento não pode ser nulo.");

                if (!value.EstaAbertoNestaData(DateTime.Now))
                    throw new ArgumentException("O evento já foi encerrado.", "evento");

                if (!value.TemOficinas)
                    throw new InvalidOperationException("Este evento não está configurado para ter Oficinas.");

                mEvento = value;
            }
        }

        public virtual int? NumeroTotalParticipantes 
        {
            get { return mNumeroTotalParticipantes; }
            set
            {
                if (value != null && DeveSerParNumeroTotalParticipantes && value % 2 != 0)
                    throw new ArgumentException("O número de participantes deve ser par.", "NumeroTotalParticipantes");

                mNumeroTotalParticipantes = value;
            }
        }

        public virtual bool DeveSerParNumeroTotalParticipantes 
        {
            get { return mDeveSerParNumeroTotalParticipantes; }
            set
            {
                if (value && mNumeroTotalParticipantes != null && mNumeroTotalParticipantes % 2 != 0)
                    throw new ArgumentException("O número de participantes deve ser par.", "NumeroTotalParticipantes");

                mDeveSerParNumeroTotalParticipantes = value;
            }
        }

        public virtual IEnumerable<InscricaoParticipante> Participantes { get { return mParticipantes; } }

        public virtual void AdicionarParticipante(InscricaoParticipante participante)
        {
            ValidarSeParticipanteEhNulo(participante);
            ValidarSeParticipanteEhMesmoEvento(participante);

            if (mNumeroTotalParticipantes != null && mParticipantes.Count >= mNumeroTotalParticipantes.Value)
                throw new ArgumentException("Não é possível incluir mais participantes. Número Total atingido.", "participante");

            if (!EstaNaListaDeParticipantes(participante))
                mParticipantes.Add(participante);
        }

        public virtual void RemoverParticipante(InscricaoParticipante participante)
        {
            ValidarSeParticipanteEhNulo(participante);

            if (!EstaNaListaDeParticipantes(participante))
                throw new ArgumentException("Participante não existe na lista de participantes desta afrac.", "participante");

            mParticipantes.Remove(participante);
        }

        public virtual void RemoverTodosParticipantes()
        {
            mParticipantes.Clear();
        }

        public virtual bool EstaNaListaDeParticipantes(InscricaoParticipante participante)
        {
            return mParticipantes.Where(x => x == participante).Count() > 0;
        }

        private void ValidarSeParticipanteEhNulo(InscricaoParticipante participante)
        {
            if (participante == null)
                throw new ArgumentNullException("participante", "Participante não pode ser nulo.");
        }

        private void ValidarSeParticipanteEhMesmoEvento(InscricaoParticipante participante)
        {
            if (participante.Evento != mEvento)
                throw new ArgumentException("participante", "Participante deve ser do mesmo evento da afrac .");
        }
    }
}
