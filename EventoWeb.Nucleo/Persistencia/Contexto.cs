using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Persistencia.Infra;
using EventoWeb.Nucleo.Persistencia.Repositorios;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Mapping.ByCode;
using System.Reflection;

namespace EventoWeb.Nucleo.Persistencia
{
    public class ConfiguracaoNHibernate {

        public ISessionFactory GerarFabricaSessao()
        {
            NHibernate.Cfg.Environment.UseReflectionOptimizer = false;
            var configuration = new Configuration();
            configuration.Configure();
            var mapper = new ModelMapper();

            configuration.AddAssembly("EventoWeb.Nucleo");
            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());

            configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            return configuration.BuildSessionFactory();
        }
    }

    public class Contexto: IContexto
    {
        private ISession m_Sessao;

        public Contexto(ISession sessao)
        {
            m_Sessao = sessao;
        }

        public void IniciarTransacao()
        {
            m_Sessao.BeginTransaction();
        }

        public void SalvarTransacao()
        {
            var session = m_Sessao;
            if (session.Transaction.IsActive)
                session.Transaction.Commit();
        }

        public void CancelarTransacao()
        {
            var session = m_Sessao;
            if (session.Transaction.IsActive)
            {
                session.Transaction.Rollback();
                session.Clear();
            }
        }

        public void Dispose()
        {
            m_Sessao.Close();
            m_Sessao.Dispose();
        }

        public AEventos RepositorioEventos => new RepositorioEventosNH(m_Sessao);
        public AUsuarios RepositorioUsuarios => new RepositorioUsuariosNH(m_Sessao);
        public ADepartamentos RepositorioDepartamentos => new RepositorioDepartamentosNH(m_Sessao);
        public ASalasEstudo RepositorioSalasEstudo => new RepositorioSalasEstudoNH(m_Sessao);
        public AAfracs RepositorioAfracs => new RepositorioAfracsNH(m_Sessao);
        public AConfiguracoesEmail RepositorioConfiguracoesEmail => new RepositorioConfiguracoesEmailNH(m_Sessao);
        public AInscricoes RepositorioInscricoes => new InscricoesNH(m_Sessao);
        public ACodigosAcessoInscricao RepositorioCodigosAcessoInscricao => new RepositorioCodigosAcessoInscricaoNH(m_Sessao);
        public IServicoGeradorCodigoSeguro ServicoGeradorCodigoSeguro => new ServicoGeradorCodigoSeguro(RepositorioCodigosAcessoInscricao);
        public AMensagensEmailPadrao RepositorioMensagensEmailPadrao => new RepositorioMensagensEmailPadrao(m_Sessao);
        public AVariaveisEmailInscricao RepositorioVariaveisEmailIncricao => new RepositorioVariaveisEmailIncricao();
        public IServicoEmail ServicoEmail => new ServicoEmail(this);
    }
}
