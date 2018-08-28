using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class ExcecaoAfracInvalida : Exception
    {
        public ExcecaoAfracInvalida(String mensagem) : base(mensagem) { }
    }

    public class GestaoAfracsEscolhidas
    {
        private AfracsEscolhidas mEscolhas;
        private AAfracs mRepositorioAfrac;

        public GestaoAfracsEscolhidas(AAfracs repositorio, AfracsEscolhidas escolhas)
        {
            mRepositorioAfrac = repositorio;
            mEscolhas = escolhas;
        }

        public virtual IEnumerable<Afrac> GerarLista()
        {
            var totalAfracs = mRepositorioAfrac.ContarTotalAfracs(mEscolhas.Evento);
            var afracs = mEscolhas.Afracs;
            if (totalAfracs == 0 && afracs.Count() > 0)
                throw new ArgumentException("Não há nenhuma afrac neste evento.", "evento");

            if (totalAfracs > 0 && afracs.Count() == totalAfracs)
                throw new ArgumentException("Todas as afracs do evento devem ser escolhidas e ordenadas.", "evento");

            return afracs;
        }
    }

    public class AfracsEscolhidas
    {
        private IList<Afrac> mAfracs;
        private Evento mEvento;

        public AfracsEscolhidas(Evento evento)
        {
            if (evento == null)
                throw new ArgumentNullException("evento", "Evento não pode ser nulo.");
            mEvento = evento;
            mAfracs = new List<Afrac>();
        }

        protected AfracsEscolhidas() { }
        
        public virtual void DefinirPrimeiraPosicao(Afrac afrac)
        {
            ValidarAfracNula(afrac);
            ValidarAfracExisteEvento(afrac);
            ValidarAfracEstaLista(afrac);

            mAfracs.Clear();
            mAfracs.Add(afrac);
        }

        public virtual void DefinirProximaPosicao(Afrac afrac)
        {
            ValidarAfracNula(afrac);
            ValidarAfracExisteEvento(afrac);
            ValidarAfracEstaLista(afrac);

            if (mAfracs.Count == 0)
                throw new IndexOutOfRangeException("Deve-se definir a primeira posição.");

            mAfracs.Add(afrac);
        }

        private void ValidarAfracNula(Afrac afrac)
        {
            if (afrac == null)
                throw new ArgumentNullException("item", "Afrac não pode ser nula");
        }

        private void ValidarAfracExisteEvento(Afrac afrac)
        {
            if (mEvento != afrac.Evento)
                throw new ExcecaoAfracInvalida("A afrac informada não existe no evento.");
        }

        private void ValidarAfracEstaLista(Afrac afrac)
        {
            if (mAfracs.Count(x=> x == afrac) > 0)
                throw new ExcecaoAfracInvalida("A afrac informada já esta na lista.");

            if (afrac.Id == 0)
                throw new ExcecaoAfracInvalida("A afrac informada não foi efetivada no banco de dados.");
        }

        public virtual Evento Evento { get { return mEvento; } }

        public virtual IEnumerable<Afrac> Afracs { get { return mAfracs; } }
    }
}
