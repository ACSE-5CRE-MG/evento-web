using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioEventosNH: AEventos
    {
        private ISession mSessao;

        public RepositorioEventosNH(ISession sessao): base(new PersistenciaNH<Evento>(sessao))
        {
            mSessao = sessao;
        }

        public override IList<Evento> ObterTodosEventos()
        {
            return mSessao.QueryOver<Evento>().List();
        }

        public override Evento ObterEventoPeloId(int id)
        {
            return mSessao.Get<Evento>(id);
        }
    }
}
