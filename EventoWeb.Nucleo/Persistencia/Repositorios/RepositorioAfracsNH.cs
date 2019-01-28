using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Persistencia.Repositorios;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Persistencia
{
    public class RepositorioAfracsNH: PersistenciaNH<Afrac>, AAfracs
    {
        private ISession mSessao;

        public RepositorioAfracsNH(ISession sessao)
            : base(sessao)
        {
            mSessao = sessao;
        }

        public IList<Afrac> ListarTodasPorEvento(int idEvento)
        {
            return mSessao.QueryOver<Afrac>()
                .Where(afrac => afrac.Evento.Id == idEvento)
                .List();
        }

        public Afrac ObterPorId(int id)
        {
            return mSessao.QueryOver<Afrac>()
                .Where(afrac => afrac.Id == id)
                .SingleOrDefault();
        }

        public bool InscritoEhResponsavelPorAfrac(Evento evento, InscricaoParticipante inscParticipante)
        {
            return mSessao.QueryOver<AtividadeInscricaoOficinasCoordenacao>()
                .Where(x => x.Inscrito.Id == inscParticipante.Id)
                .JoinQueryOver(x => x.Inscrito)
                    .JoinQueryOver(y => y.Evento)
                        .Where(y => y.Id == evento.Id)
                .RowCount() > 0;
        }

        public IList<Afrac> ListarTodasAfracsComParticipantesPorEvento(Evento evento)
        {
            var consulta = mSessao.QueryOver<Afrac>()
                .Where(afrac => afrac.Evento == evento)
                .Future();

            mSessao.QueryOver<Afrac>()
                .Where(afrac => afrac.Evento == evento)
                .Left.JoinQueryOver(x => x.Participantes)
                .Left.JoinQueryOver(y=>y.Pessoa)
                .Future();

            return consulta.ToList();
        }

        public IList<InscricaoParticipante> ListarParticipantesSemAfracNoEvento(Evento evento)
        {
            InscricaoParticipante aliasParticipante = null;
            AtividadeInscricaoOficinas aliasAtividade = null;

            var subQueryParticipantes = QueryOver.Of<Afrac>()
                .JoinQueryOver<InscricaoParticipante>(x => x.Participantes, () => aliasParticipante)
                .Where(x => x.Id == aliasAtividade.Inscrito.Id)
                .SelectList(x => x.Select(() => aliasParticipante.Id));

            var subQueryCoordenadores = QueryOver.Of<AtividadeInscricaoOficinasCoordenacao>()
                .Where(x => x.Inscrito.Id == aliasAtividade.Id)
                .Select(x => x.Inscrito.Id);

            return mSessao.QueryOver<AtividadeInscricaoOficinas>(()=> aliasAtividade)
                .JoinQueryOver(x=>x.Inscrito)
                    .JoinQueryOver(y=>y.Evento)
                        .Where(y=>y.Id == evento.Id)
                .WithSubquery.WhereNotExists(subQueryParticipantes)
                .Select(x => x.Inscrito)
                .List<InscricaoParticipante>();
        }

        public bool HaAfracsSemResponsavelDefinidoDoEvento(Evento evento)
        {
            Afrac aliasAfrac = null;

            var consultaResponsaveis =
                QueryOver.Of<AtividadeInscricaoOficinasCoordenacao>()
                    .Where(x => x.AfracEscolhida.Id == aliasAfrac.Id)
                    .SelectList(x => x.Select(y => y.Inscrito));

            return mSessao.QueryOver<Afrac>(() => aliasAfrac)
                .Where(x => x.Evento.Id == evento.Id)
                .WithSubquery.WhereNotExists(consultaResponsaveis)
                .RowCount() > 0;
        }

        public bool EhParticipanteDeAfracNoEvento(Evento evento, InscricaoParticipante participante)
        {
            return mSessao.QueryOver<Afrac>()
                .Where(x => x.Evento == evento)
                    .JoinQueryOver(x => x.Participantes)
                        .Where(c => c.Id == participante.Id)
                .RowCount() > 0;
        }

        public bool HaParticipatesOuResponsaveisEmOutraAfrac(Afrac afrac)
        {
            InscricaoParticipante aliasParticipante = null;

            var queryParticipantes = mSessao.QueryOver<Afrac>()
                .Where(x => x.Id != afrac.Id)
                .JoinQueryOver<InscricaoParticipante>(x => x.Participantes, () => aliasParticipante)
                .SelectList(x => x.Select(() => aliasParticipante.Id))
                .Future<int>();
                
            return queryParticipantes.Where(x => afrac.Participantes.Select(i => i.Id).Contains(x)).Count() > 0;
        }

        public Afrac BuscarAfracDoInscrito(int idEvento, int idInscricao)
        {
            Afrac aliasAfrac = null;

            var consultaParticipantes = QueryOver.Of<Afrac>()
                .Where(x => x.Id == aliasAfrac.Id)
                .JoinQueryOver(x => x.Participantes)
                    .Where(y => y.Id == idInscricao)
                .SelectList(lista => lista
                    .Select(x => x.Id));

            var consultaCoordenadores = QueryOver.Of<AtividadeInscricaoOficinasCoordenacao>()
                .Where(x => x.AfracEscolhida.Id == aliasAfrac.Id && x.Inscrito.Id == idInscricao)
                .SelectList(lista => lista
                    .Select(x => x.Inscrito.Id));

            return mSessao.QueryOver<Afrac>(() => aliasAfrac)
                .Where(Restrictions.Conjunction()
                    .Add<Afrac>(x => x.Evento.Id == idEvento)
                    .Add(Restrictions.Disjunction()
                        .Add(Subqueries.WhereExists(consultaCoordenadores))
                        .Add(Subqueries.WhereExists(consultaParticipantes)
                        )
                    )
                )
                .SingleOrDefault();
        }

        public int ContarTotalAfracs(Evento mEvento)
        {
            return mSessao.QueryOver<Afrac>().RowCount();
        }
    }
}
