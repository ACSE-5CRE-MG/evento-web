using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class ExcecaoOficinaInvalida : Exception
    {
        public ExcecaoOficinaInvalida(String mensagem) : base(mensagem) { }
    }

    public class GestaoOficinasEscolhidas
    {
        private OficinasEscolhidas mEscolhas;
        private AOficinas mRepositorioOficinas;

        public GestaoOficinasEscolhidas(AOficinas repositorio, OficinasEscolhidas escolhas)
        {
            mRepositorioOficinas = repositorio;
            mEscolhas = escolhas;
        }

        public virtual IEnumerable<Oficina> GerarLista()
        {
            var totalAfracs = mRepositorioOficinas.ContarTotalOficinas(mEscolhas.Evento);
            var afracs = mEscolhas.Oficinas;
            if (totalAfracs == 0 && afracs.Count() > 0)
                throw new ArgumentException("Não há nenhuma oficina neste evento.", "evento");

            if (totalAfracs > 0 && afracs.Count() == totalAfracs)
                throw new ArgumentException("Todas as oficinas do evento devem ser escolhidas e ordenadas.", "evento");

            return afracs;
        }
    }

    public class OficinasEscolhidas
    {
        private IList<Oficina> mOficinas;
        private Evento mEvento;

        public OficinasEscolhidas(Evento evento)
        {
            if (evento == null)
                throw new ArgumentNullException("evento", "Evento não pode ser nulo.");
            mEvento = evento;
            mOficinas = new List<Oficina>();
        }

        protected OficinasEscolhidas() { }
        
        public virtual void DefinirPrimeiraPosicao(Oficina oficina)
        {
            ValidarAfracNula(oficina);
            ValidarAfracExisteEvento(oficina);
            ValidarAfracEstaLista(oficina);

            mOficinas.Clear();
            mOficinas.Add(oficina);
        }

        public virtual void DefinirProximaPosicao(Oficina oficina)
        {
            ValidarAfracNula(oficina);
            ValidarAfracExisteEvento(oficina);
            ValidarAfracEstaLista(oficina);

            if (mOficinas.Count == 0)
                throw new IndexOutOfRangeException("Deve-se definir a primeira posição.");

            mOficinas.Add(oficina);
        }

        private void ValidarAfracNula(Oficina oficina)
        {
            if (oficina == null)
                throw new ArgumentNullException("item", "Oficina não pode ser nula");
        }

        private void ValidarAfracExisteEvento(Oficina oficina)
        {
            if (mEvento != oficina.Evento)
                throw new ExcecaoOficinaInvalida("A oficina informada não existe no evento.");
        }

        private void ValidarAfracEstaLista(Oficina oficina)
        {
            if (mOficinas.Count(x=> x == oficina) > 0)
                throw new ExcecaoOficinaInvalida("A oficina informada já esta na lista.");

            if (oficina.Id == 0)
                throw new ExcecaoOficinaInvalida("A oficina informada não foi efetivada no banco de dados.");
        }

        public virtual Evento Evento { get { return mEvento; } }

        public virtual IEnumerable<Oficina> Oficinas { get { return mOficinas; } }
    }
}
