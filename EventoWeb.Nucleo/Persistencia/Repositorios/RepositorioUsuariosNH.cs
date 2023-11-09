using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;
using System.Collections.Generic;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioUsuariosNH: AUsuarios
    {
        private ISession mSessao;

        public RepositorioUsuariosNH(ISession sessao) : base(new PersistenciaNH<Usuario>(sessao))
        {
            mSessao = sessao;
        }

        public override IList<Usuario> ListarTodos()
        {
            return mSessao
                .QueryOver<Usuario>()
                .List();
        }

        public override Usuario ObterPeloLogin(string login)
        {
            return mSessao.Get<Usuario>(login);
        }
    }
}
