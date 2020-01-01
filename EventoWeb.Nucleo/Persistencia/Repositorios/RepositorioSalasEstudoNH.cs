using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Persistencia.Repositorios;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Persistencia
{
    public class RepositorioSalasEstudoNH : ASalasEstudo
    {
        private readonly ISession mSessao;

        public RepositorioSalasEstudoNH(ISession sessao)
            : base(new PersistenciaNH<SalaEstudo>(sessao))
        {
            mSessao = sessao;
        }

        public override IList<SalaEstudo> ListarTodasPorEvento(int idEvento)
        {
            return mSessao.QueryOver<SalaEstudo>()
                            .Where(x => x.Evento.Id == idEvento)
                            .List();
        }

        public override bool HaSalasSemCoordenadorDefinidoDoEvento(Evento evento)
        {
            SalaEstudo aliasSala = null;

            var consultaCoordenadores =
                QueryOver.Of<AtividadeInscricaoSalaEstudoCoordenacao>()
                    .Where(x=>x.SalaEscolhida.Id == aliasSala.Id)
                    .SelectList(x => x.Select(y => y.Id));

            return mSessao.QueryOver<SalaEstudo>(() => aliasSala)
                .Where(x => x.Evento.Id == evento.Id)
                .WithSubquery.WhereNotExists(consultaCoordenadores)
                .RowCount() > 0;
        }

        public override bool EhCoordenadorDeSalaNoEvento(Evento evento, InscricaoParticipante participante)
        {
            return mSessao.QueryOver<AtividadeInscricaoSalaEstudoCoordenacao>()
                .Where(x => x.Inscrito.Id == participante.Id)
                .JoinQueryOver(x => x.Inscrito)
                    .JoinQueryOver(y => y.Evento)
                        .Where(y => y.Id == evento.Id)
                .RowCount() > 0;
        }

        public override bool EhParticipanteDeSalaNoEvento(Evento evento, InscricaoParticipante participante)
        {
            return mSessao.QueryOver<SalaEstudo>()
                .Where(x => x.Evento == evento)
                    .JoinQueryOver(x => x.Participantes)
                        .Where(c => c.Id == participante.Id)
                .SingleOrDefault() != null;
        }

        public override SalaEstudo ObterPorId(int idEvento, int id)
        {
            return mSessao.QueryOver<SalaEstudo>()
                .Where(x => x.Id == id && x.Evento.Id == idEvento)
                .SingleOrDefault();
        }

        public override IList<InscricaoParticipante> ListarParticipantesSemSalaEstudoNormal(Evento evento)
        {
            InscricaoParticipante aliasParticipante = null;
            AtividadeInscricaoSalaEstudo aliasAtividade = null;

            var subQueryParticipantes = QueryOver.Of<SalaEstudo>()
                .JoinQueryOver<InscricaoParticipante>(x => x.Participantes, () => aliasParticipante)
                .Where(x => x.Id == aliasAtividade.Inscrito.Id && x.Situacao == EnumSituacaoInscricao.Aceita)
                .SelectList(x => x.Select(() => aliasParticipante.Id));

            var subQueryCoordenadores = QueryOver.Of<AtividadeInscricaoSalaEstudoCoordenacao>()
                .Where(x => x.Inscrito.Id == aliasAtividade.Id)
                .Select(x => x.Inscrito.Id);

            return mSessao.QueryOver<AtividadeInscricaoSalaEstudo>(() => aliasAtividade)
                .JoinQueryOver(x => x.Inscrito)
                    .Where(x=> x.Situacao == EnumSituacaoInscricao.Aceita)
                    .JoinQueryOver(y => y.Evento)
                        .Where(y => y.Id == evento.Id)
                .WithSubquery.WhereNotExists(subQueryParticipantes)
                .Select(x => x.Inscrito)
                .List<InscricaoParticipante>();
        }

        public override IList<SalaEstudo> ListarTodasSalasEstudoComParticipantesDoEvento(Evento evento)
        {
            var ConsSalas = mSessao.QueryOver<SalaEstudo>()
                            .Where(x => x.Evento == evento)
                            .Future();
            mSessao.QueryOver<SalaEstudo>()
                .Where(x => x.Evento == evento)
                .Left.JoinQueryOver(x => x.Participantes)
                .Left.JoinQueryOver(y => y.Pessoa)
                .Future();

            return ConsSalas.ToList();
        }

        protected override bool HaParticipatesEmOutraSala(SalaEstudo sala)
        {
            InscricaoParticipante aliasParticipante = null;

            var queryParticipantes = mSessao.QueryOver<SalaEstudo>()
                .Where(x => x.Id != sala.Id)
                .JoinQueryOver<InscricaoParticipante>(x => x.Participantes, () => aliasParticipante)
                .SelectList(x => x.Select(() => aliasParticipante.Id))
                .Future<int>();

            return queryParticipantes.Where(x => sala.Participantes.Select(i => i.Id).Contains(x)).Count() > 0;
        }

        public override SalaEstudo BuscarSalaDoInscrito(int idEvento, int idInscricao)
        {
            SalaEstudo aliasSala = null;

            var consultaParticipantes = QueryOver.Of<SalaEstudo>()
                .Where(x => x.Id == aliasSala.Id)
                .JoinQueryOver(x => x.Participantes)
                    .Where(y => y.Id == idInscricao)
                .SelectList(lista => lista
                    .Select(x => x.Id));

            var consultaCoordenadores = QueryOver.Of<AtividadeInscricaoSalaEstudoCoordenacao>()
                .Where(x => x.SalaEscolhida.Id == aliasSala.Id && x.Inscrito.Id == idInscricao)
                .SelectList(lista => lista
                    .Select(x => x.Inscrito.Id));

            return mSessao.QueryOver<SalaEstudo>(() => aliasSala)
                .Where(Restrictions.Conjunction()
                    .Add<SalaEstudo>(x => x.Evento.Id == idEvento)
                    .Add(Restrictions.Disjunction()
                        .Add(Subqueries.WhereExists(consultaCoordenadores))
                        .Add(Subqueries.WhereExists(consultaParticipantes)
                        )
                    )
                )
                .SingleOrDefault();
        }

        public override int ContarTotalSalas(Evento evento)
        {
            return mSessao.QueryOver<SalaEstudo>().RowCount();
        }

        protected override bool HaSalaComFaixaEtariaDefinida(SalaEstudo sala)
        {
            return mSessao
                    .QueryOver<SalaEstudo>()
                    .Where(x => x.FaixaEtaria != null && x.Id != sala.Id && x.Evento.Id == sala.Evento.Id)
                    .RowCount() > 0;
        }

        protected override bool FoiEscolhidaInscricao(SalaEstudo sala)
        {
            return mSessao
                .QueryOver<AtividadeInscricaoSalaEstudoOrdemEscolha>()
                .JoinQueryOver(x => x.Salas)
                .Where(x => x.Id == sala.Id)
                .Take(1)
                .RowCount() > 0;
        }

        public override IList<InscricaoParticipante> ListarParticipantesSemSalaEstudoPorOrdem(Evento evento)
        {
            InscricaoParticipante aliasParticipante = null;
            AtividadeInscricaoSalaEstudoOrdemEscolha aliasAtividade = null;

            var subQueryParticipantes = QueryOver.Of<SalaEstudo>()
                .JoinQueryOver<InscricaoParticipante>(x => x.Participantes, () => aliasParticipante)
                .Where(x => x.Id == aliasAtividade.Inscrito.Id && x.Situacao == EnumSituacaoInscricao.Aceita)
                .SelectList(x => x.Select(() => aliasParticipante.Id));

            var subQueryCoordenadores = QueryOver.Of<AtividadeInscricaoSalaEstudoCoordenacao>()
                .Where(x => x.Inscrito.Id == aliasAtividade.Id)
                .Select(x => x.Inscrito.Id);

            return mSessao.QueryOver<AtividadeInscricaoSalaEstudoOrdemEscolha>(() => aliasAtividade)
                .JoinQueryOver(x => x.Inscrito)
                    .Where(x => x.Situacao == EnumSituacaoInscricao.Aceita)
                    .JoinQueryOver(y => y.Evento)
                        .Where(y => y.Id == evento.Id)
                .WithSubquery.WhereNotExists(subQueryParticipantes)
                .Select(x => x.Inscrito)
                .List<InscricaoParticipante>();
        }
    }
}
