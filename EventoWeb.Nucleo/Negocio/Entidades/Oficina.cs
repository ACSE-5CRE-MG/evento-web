using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Oficina: Entidade
    {
        private string m_Nome;
        private Evento m_Evento;
        private int? m_NumeroTotalParticipantes;
        private bool m_DeveSerParNumeroTotalParticipantes;
        private IList<InscricaoParticipante> m_Participantes;

        public Oficina(Evento evento, string nome)
        {
            Evento = evento;
            Nome = nome;

            m_Participantes = new List<InscricaoParticipante>();
        }

        protected Oficina() { }

        public virtual String Nome
        {
            get { return m_Nome; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Nome", "O nome não pode ser nulo.");

                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Nome não pode ser vazio.");

                m_Nome = value;
            }
        }

        public virtual Evento Evento
        {
            get { return m_Evento; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Evento", "Evento não pode ser nulo.");

                if (value.ConfiguracaoOficinas == null)
                    throw new InvalidOperationException("Este evento não está configurado para ter Oficinas.");

                m_Evento = value;
            }
        }

        public virtual int? NumeroTotalParticipantes 
        {
            get { return m_NumeroTotalParticipantes; }
            set
            {
                if (value != null && DeveSerParNumeroTotalParticipantes && value % 2 != 0)
                    throw new ArgumentException("O número de participantes deve ser par.", "NumeroTotalParticipantes");

                m_NumeroTotalParticipantes = value;
            }
        }

        public virtual bool DeveSerParNumeroTotalParticipantes 
        {
            get { return m_DeveSerParNumeroTotalParticipantes; }
            set
            {
                if (value && m_NumeroTotalParticipantes != null && m_NumeroTotalParticipantes % 2 != 0)
                    throw new ArgumentException("O número de participantes deve ser par.", "NumeroTotalParticipantes");

                m_DeveSerParNumeroTotalParticipantes = value;
            }
        }

        public virtual IEnumerable<InscricaoParticipante> Participantes { get { return m_Participantes; } }

        public virtual void AdicionarParticipante(InscricaoParticipante participante)
        {
            ValidarSeParticipanteEhNulo(participante);
            ValidarSeParticipanteEhMesmoEvento(participante);

            if (m_NumeroTotalParticipantes != null && m_Participantes.Count >= m_NumeroTotalParticipantes.Value)
                throw new ArgumentException("Não é possível incluir mais participantes. Número Total atingido.", "participante");

            if (!EstaNaListaDeParticipantes(participante))
                m_Participantes.Add(participante);
        }

        public virtual void RemoverParticipante(InscricaoParticipante participante)
        {
            ValidarSeParticipanteEhNulo(participante);

            if (!EstaNaListaDeParticipantes(participante))
                throw new ArgumentException("Participante não existe na lista de participantes desta oficina.", "participante");

            m_Participantes.Remove(participante);
        }

        public virtual void RemoverTodosParticipantes()
        {
            m_Participantes.Clear();
        }

        public virtual bool EstaNaListaDeParticipantes(InscricaoParticipante participante)
        {
            return m_Participantes.Where(x => x == participante).Count() > 0;
        }

        private void ValidarSeParticipanteEhNulo(InscricaoParticipante participante)
        {
            if (participante == null)
                throw new ArgumentNullException("participante", "Participante não pode ser nulo.");
        }

        private void ValidarSeParticipanteEhMesmoEvento(InscricaoParticipante participante)
        {
            if (participante.Evento != m_Evento)
                throw new ArgumentException("participante", "Participante deve ser do mesmo evento da oficina.");
        }
    }
}
