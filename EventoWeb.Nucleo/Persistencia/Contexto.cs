using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Persistencia.Repositorios;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Mapping.ByCode;
using System.Reflection;

namespace EventoWeb.Nucleo.Persistencia
{
    public class Contexto: IContexto
    {
        private static ISessionFactory mFabricaSession;

        private static ISession SessionEmAberto
        {
            get
            {
                if (mFabricaSession == null)
                {
                    NHibernate.Cfg.Environment.UseReflectionOptimizer = false;
                    var configuration = new Configuration();
                    configuration.Configure();
                    var mapper = new ModelMapper();

                    configuration.AddAssembly("EventoWeb.Nucleo");
                    mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());

                    configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

                    mFabricaSession = configuration.BuildSessionFactory();
                }

                if (CurrentSessionContext.HasBind(mFabricaSession) && !mFabricaSession.GetCurrentSession().IsOpen)
                    CurrentSessionContext.Unbind(mFabricaSession);

                if (!CurrentSessionContext.HasBind(mFabricaSession))
                    CurrentSessionContext.Bind(mFabricaSession.OpenSession());

                return mFabricaSession.GetCurrentSession();
            }
        }

        public void IniciarTransacao()
        {
            SessionEmAberto.BeginTransaction();
        }

        public void SalvarTransacao()
        {
            var session = SessionEmAberto;
            if (session.Transaction.IsActive)
                session.Transaction.Commit();
        }

        public void CancelarTransacao()
        {
            var session = SessionEmAberto;
            if (session.Transaction.IsActive)
                session.Transaction.Rollback();
        }

        public void FinalizarConexao()
        {
            var session = SessionEmAberto;
            session.Close();
            session.Dispose();
        }

        public AEventos RepositorioEventos
        {
            get
            {
                return new RepositorioEventosNH(SessionEmAberto);
            }
        }

    }
}
