using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class SalaEstudo: Entidade
    {
        private Evento mEvento;
        private string mNome;
        private IList<InscricaoParticipante> mParticipantes;
        private FaixaEtaria mFaixaEtaria;

        public SalaEstudo(Evento evento, string nome)
        {
            mParticipantes = new List<InscricaoParticipante>();

            if (evento == null)
                throw new ArgumentNullException("evento", "Evento não pode ser nulo.");

            if (!evento.EstaAbertoNestaData(DateTime.Now))
                throw new ArgumentException("O evento já foi encerrado.", "evento");

            if (!evento.TemSalasEstudo)
                throw new InvalidOperationException("Este evento não está configurado para ter Salas de Estudo.");


            mEvento = evento;
            Nome = nome;
        }

        protected SalaEstudo() { }

        public virtual Evento Evento { get { return mEvento; } }

        public virtual string Nome 
        {
            get { return mNome; }
            set 
            { 
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException("Nome","O nome não pode ser vazio.");

                mNome = value; 
            }
        }

        public virtual IEnumerable<InscricaoParticipante> Participantes { get { return mParticipantes; } }

        public virtual FaixaEtaria FaixaEtaria
        {
            get { return mFaixaEtaria; }
            set
            {
                if (mEvento.ModeloDivisaoSalasEstudo == EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao)
                    throw new ArgumentException("O modelo de divisão da salas de estudo não permite o uso da faixa etária.", "FaixaEtaria");

                mFaixaEtaria = value;
            }
        }
        

        public virtual void AdicionarParticipante(InscricaoParticipante participante)
        {
            ValidarSeParticipanteEhNulo(participante);
            ValidarSeParticipanteEhMesmoEvento(participante);

            if (mEvento.ModeloDivisaoSalasEstudo == EnumModeloDivisaoSalasEstudo.PorIdadeCidade &&
                mFaixaEtaria != null &&
                (participante.Pessoa.CalcularIdadeEmAnos(mEvento.DataInicioEvento) < mFaixaEtaria.IdadeMin ||
                participante.Pessoa.CalcularIdadeEmAnos(mEvento.DataInicioEvento) > mFaixaEtaria.IdadeMax))
                throw new ArgumentException("Participante fora da faixa etária definida para esta sala.");

            if (!EstaNaListaDeParticipantes(participante))
                mParticipantes.Add(participante);
        }

        public virtual void RemoverParticipante(InscricaoParticipante participante)
        {
            ValidarSeParticipanteEhNulo(participante);

            if (!EstaNaListaDeParticipantes(participante))
                throw new ArgumentException("Participante não existe na lista de participantes desta sala.", "participante");

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
                throw new ArgumentException("participante", "Participante deve ser do mesmo evento da sala.");
        }
    }
}
