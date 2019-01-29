using System;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum SexoQuarto { Masculino, Feminino, Misto }

    public class QuartoInscrito: Entidade
    {
        private Inscricao mInscricao;
        private Quarto mQuarto;

        public QuartoInscrito(Quarto quarto, Inscricao inscrito, Boolean ehCoordenador)
        {
            if (quarto == null)
                throw new ArgumentNullException("quarto");
            
            if (inscrito == null)
                throw new ArgumentNullException("inscrito");

            mQuarto = quarto;
            mInscricao = inscrito;
            EhCoordenador = ehCoordenador;
        }

        protected QuartoInscrito() { }

        public virtual Quarto Quarto { get { return mQuarto; } }
        public virtual Inscricao Inscricao { get { return mInscricao; } }
        public virtual Boolean EhCoordenador { get; set; }
    }

    public class Quarto: Entidade
    {
        private String mNome;
        private Evento mEvento;
        private int? mCapacidade;
        private IList<QuartoInscrito> mInscritos;

        public Quarto(Evento evento, string nome, Boolean ehFamilia, SexoQuarto sexo)
        {
            Nome = nome;
            Evento = evento;
            Sexo = sexo;
            AtribuirSexoEEhFamilia(ehFamilia, sexo);
            mInscritos = new List<QuartoInscrito>();
        }

        protected Quarto() { }

        public virtual Evento Evento
        {
            get { return mEvento; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Evento", "Evento não pode ser nulo");

                if (!value.TemDormitorios)
                    throw new InvalidOperationException("Este evento não está configurado para ter Quartos.");

                mEvento = value;
            }
        }

        public virtual string Nome 
        {
            get { return mNome; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Nome", "Nome não pode ser nulo.");

                if (value == "")
                    throw new ArgumentException("Nome", "Nome não pode ser vazio.");

                mNome = value;
            }
        }

        public virtual int? Capacidade
        {
            get { return mCapacidade; }
            set
            {
                if (value != null && value <= 0)
                    throw new ArgumentException("Capacidade", "A capacidade informada deve ser maior que zero.");

                mCapacidade = value;
            }
        }

        public virtual Boolean EhFamilia { get; protected set; }

        public virtual SexoQuarto Sexo { get; protected set; }


        public virtual IEnumerable<QuartoInscrito> Inscritos { get { return mInscritos; } }

        public virtual void AtribuirSexoEEhFamilia(bool ehFamilia, SexoQuarto sexo)
        {
            if (!ehFamilia && sexo == SexoQuarto.Misto)
                throw new InvalidOperationException("Somente quarto família pode ter o sexo misto.");

            EhFamilia = ehFamilia;
            Sexo = sexo;
        }

        public virtual void AdicionarInscrito(Inscricao inscrito, Boolean ehCoordenador = false)
        {
            if (inscrito.Evento != Evento)
                throw new ArgumentException("A inscrição não é do mesmo evento do quarto.");

            if (inscrito is InscricaoParticipante && this.Sexo != SexoQuarto.Misto && (int)inscrito.Pessoa.Sexo != (int)this.Sexo)
                throw new ArgumentException("O inscrito não é do mesmo sexo definido para o quarto.");

            if (Capacidade != null && mInscritos.Count == Capacidade.Value)
                throw new ArgumentException("Nâo é possível incluir mais participantes neste quarto.");


            mInscritos.Add(new QuartoInscrito(this, inscrito, ehCoordenador));
        }

        public virtual void RemoverInscrito(QuartoInscrito inscrito)
        {
            if (!mInscritos.Remove(inscrito))
                throw new ArgumentException("Nâo existe este inscrito no quarto.");
        }

        public virtual void RemoverTodosInscritos()
        {
            mInscritos.Clear();
        }
    }
}
