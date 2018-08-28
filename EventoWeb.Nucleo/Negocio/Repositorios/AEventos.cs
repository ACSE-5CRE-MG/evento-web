using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AEventos : ARepositorio<Evento>
    {
        public AEventos(IPersistencia<Evento> persistencia) : base(persistencia) { }

        public abstract IList<Evento> ObterTodosEventos();
        public abstract Evento ObterEventoPeloId(int id);

        public override void Excluir(Evento objeto)
        {
            if (objeto.Situacao != SituacaoEvento.Aberto)
                throw new ExcecaoPersistencia("Não se pode excluir um evento que esteja em situação diferente de Aberto");

            base.Excluir(objeto);
        }
    }
}
