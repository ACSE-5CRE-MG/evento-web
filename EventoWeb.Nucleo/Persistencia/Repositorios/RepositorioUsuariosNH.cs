using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using NHibernate;

namespace EventoWeb.Nucleo.Persistencia.Repositorios
{
    public class RepositorioUsuariosNH: AUsuarios
    {
        private ISession mSessao;

        public RepositorioUsuariosNH(ISession sessao)
        {
            mSessao = sessao;
        }

        public override Usuario ObterUsuarioPeloLogin(string login)
        {
            return mSessao.Get<Usuario>(login);
        }
    }
}
