using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppEvento: AppBase
    {
        public AppEvento(IContexto contexto)
            : base(contexto) { }

        public Evento ObterPorId(int id)
        {
            Evento evento = null;
            ExecutarSeguramente(() =>
            {
                evento = Contexto.RepositorioEventos.ObterEventoPeloId(id);
            });

            return evento;
        }

        public IList<Evento> ObterTodos()
        {
            IList<Evento> eventos = null;
            ExecutarSeguramente(() =>
            {
                eventos = Contexto.RepositorioEventos.ObterTodosEventos();
            });

            return eventos;
        }

        public void Incluir(Evento evento)
        {
            ExecutarSeguramente(() =>
            {
                Contexto.RepositorioEventos.Incluir(evento);
            });
        }

        public void Atualizar(Evento evento)
        {
            ExecutarSeguramente(() =>
            {
                Contexto.RepositorioEventos.Atualizar(evento);
            });
        }

        public void Excluir(Evento evento)
        {
            ExecutarSeguramente(() =>
            {
                Contexto.RepositorioEventos.Excluir(evento);
            });
        }
    }
}
