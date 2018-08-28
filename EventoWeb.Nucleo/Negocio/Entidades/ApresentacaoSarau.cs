using System;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class ApresentacaoSarau : Entidade
    {
        private Evento mEvento;
        private string mNome;
        private string mTipo;
        private int mDuracaoMin;
        private IList<Inscricao> mInscritos;
        private IList<Pessoa> mPessoasAguardandoInscricao;

        public ApresentacaoSarau(Evento evento, string nome, int duracaoMin, string tipo, IEnumerable<Inscricao> inscritos)
        {
            Evento = evento;
            Nome = nome;
            DuracaoMin = duracaoMin;
            Tipo = tipo;

            mPessoasAguardandoInscricao = new List<Pessoa>();
            mInscritos = new List<Inscricao>();
            AtualizarInscricoes(inscritos);
        }

        protected ApresentacaoSarau() { }

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

                if (!value.TemSarau)
                    throw new InvalidOperationException("Este evento não está configurado para ter Sarau.");

                mEvento = value;
            }
        }

        public virtual String Tipo
        {
            get { return mTipo; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Tipo", "O nome não pode ser nulo.");

                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Tipo não pode ser vazio.");

                mTipo = value;
            }
        }

        public virtual IEnumerable<Inscricao> Inscritos { get { return mInscritos; } }

        public virtual IEnumerable<Pessoa> PessoasAguardandoInscricao { get { return mPessoasAguardandoInscricao; } }

        public virtual int DuracaoMin
        {
            get { return mDuracaoMin; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("A Duração deve ser dada em minutos e o seu valor deve ser maior que zero.");

                mDuracaoMin = value;
            }
        }

        public virtual void AdicionarInscricao(Inscricao inscricao)
        {
            if (inscricao == null)
                throw new ArgumentNullException("inscricao", "Inscrição não pode ser nula.");

            if (mInscritos.FirstOrDefault(x => x == inscricao) != null)
                throw new ArgumentException("Esta inscrição já consta nesta apresentação.");

            mInscritos.Add(inscricao);
        }

        public virtual void AtualizarInscricoes(IEnumerable<Inscricao> inscricoes)
        {
            if (inscricoes == null || inscricoes.Count() == 0)
                throw new ArgumentException("É preciso de pelo menos uma inscrição.", "inscritos");

            if (inscricoes.Count(x=> x == null) > 0)
                throw new ArgumentException("Há na lista inscrições nulas.", "inscritos");

            if (inscricoes != null && inscricoes.GroupBy(x => x.Id).Count(y => y.Count() > 1) > 0)
                throw new ArgumentException("Há inscrições mais de uma vez na lista.");

            mInscritos.Clear();
            foreach (var inscricao in inscricoes)
                mInscritos.Add(inscricao);
        }

        public virtual void AdicionarPessoaSerInscrita(Pessoa pessoa)
        {
            if (pessoa == null)
                throw new ArgumentNullException("pessoa", "Pessoa não pode ser nula.");

            if (mPessoasAguardandoInscricao.FirstOrDefault(x => x == pessoa) != null)
                throw new ArgumentException("Esta pessoa já consta na apresentação.");

            mPessoasAguardandoInscricao.Add(pessoa);
        }

        public virtual void AtualizarPessoas(IEnumerable<Pessoa> pessoas)
        {
            if (pessoas != null && pessoas.GroupBy(x => x.Id).Count(y=> y.Count() > 1) > 0)
                throw new ArgumentException("Há pessoas mais de uma vez na lista.");

            mPessoasAguardandoInscricao.Clear();
            if (pessoas != null && pessoas.Count() > 0)
            {
                foreach (var pessoa in pessoas)
                    mPessoasAguardandoInscricao.Add(pessoa);
            }
        }
    }
}
