using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Negocio.Servicos;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppDivisaoOficinas : AppBase
    {
        private readonly AEventos m_RepEventos;
        private readonly AOficinas m_RepOficinas;
        private readonly AInscricoes m_RepInscricoes;

        public AppDivisaoOficinas(IContexto contexto, AEventos repEventos, AOficinas repOficinas, 
            AInscricoes repInscricoes) : base(contexto)
        {
            m_RepEventos = repEventos ?? throw new ExcecaoAplicacao("AppDivisaoOficinas", "repEventos não pode ser nulo");
            m_RepInscricoes = repInscricoes ?? throw new ExcecaoAplicacao("AppDivisaoOficinas", "repInscricoes não pode ser nulo"); 
            m_RepOficinas = repOficinas ?? throw new ExcecaoAplicacao("AppDivisaoOficinas", "repOficinas não pode ser nulo");
        }

        public IEnumerable<DTODivisaoOficina> ObterDivisao(int idEvento)
        {
            IList<DTODivisaoOficina> oficinasDTO = new List<DTODivisaoOficina>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);
                oficinasDTO = ObterDivisaoOficinas(evento);
            });

            return oficinasDTO;
        }

        public IEnumerable<DTODivisaoOficina> RealizarDivisaoAutomatica(int idEvento)
        {
            IList<DTODivisaoOficina> oficinasDTO = new List<DTODivisaoOficina>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);

                IList<Oficina> oficinas;
                if (evento.ConfiguracaoOficinas == EnumModeloDivisaoOficinas.PorOrdemEscolhaInscricao)
                {
                    var divisor = new DivisaoAutomaticaInscricoesParticipantesPorOficinasEscolhidas(evento, m_RepInscricoes, m_RepOficinas);
                    oficinas = divisor.Dividir();
                }
                else
                {
                    var divisor = new DivisaoAutomaticaInscricoesParticipantePorOficinaOrdem(evento, m_RepInscricoes, m_RepOficinas);
                    oficinas = divisor.Dividir();
                }

                foreach (var oficina in oficinas)
                    m_RepOficinas.Atualizar(oficina);

                oficinasDTO = ObterDivisaoOficinas(evento);
            });

            return oficinasDTO;
        }

        public IEnumerable<DTODivisaoOficina> MoverParticipante(int idEvento, int idInscricao, int daIdOficina, int paraIdOficina)
        {
            IList<DTODivisaoOficina> oficinasDTO = new List<DTODivisaoOficina>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);
                Oficina oficinaOrigem = m_RepOficinas.ObterPorId(idEvento, daIdOficina);
                Oficina oficinaDestino = m_RepOficinas.ObterPorId(idEvento, paraIdOficina);

                InscricaoParticipante participante = (InscricaoParticipante)
                        m_RepInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);

                DivisaoManualParticipantesPorOficina divisor =
                    new DivisaoManualParticipantesPorOficina(evento, m_RepOficinas);

                divisor.Oficina(oficinaOrigem).MoverParticipante(participante).Para(oficinaDestino);

                m_RepOficinas.Atualizar(oficinaOrigem);
                m_RepOficinas.Atualizar(oficinaDestino);

                oficinasDTO = ObterDivisaoOficinas(evento);
            });

            return oficinasDTO;
        }

        public IEnumerable<DTODivisaoOficina> RemoverParticipante(int idEvento, int idInscricao, int idOficina)
        {
            IList<DTODivisaoOficina> oficinasDTO = new List<DTODivisaoOficina>();
            ExecutarSeguramente(() =>
            {
                var evento = m_RepEventos.ObterEventoPeloId(idEvento);
                var oficina = m_RepOficinas.ObterPorId(idEvento, idOficina);
                var participante = (InscricaoParticipante)m_RepInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);

                var divisor = new DivisaoManualParticipantesPorOficina(
                    evento, m_RepOficinas);

                divisor.Oficina(oficina).RemoverParticipante(participante);

                m_RepOficinas.Atualizar(oficina);

                oficinasDTO = ObterDivisaoOficinas(evento);
            });

            return oficinasDTO;
        }

        public IEnumerable<DTODivisaoOficina> IncluirParticipante(int idEvento, int idSala, int idInscricao)
        {
            IList<DTODivisaoOficina> oficinasDTO = new List<DTODivisaoOficina>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);
                InscricaoParticipante inscricao = (InscricaoParticipante)m_RepInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);

                Oficina oficina = m_RepOficinas.ObterPorId(idEvento, idSala);

                DivisaoManualParticipantesPorOficina divisor =
                    new DivisaoManualParticipantesPorOficina(evento, m_RepOficinas);

                divisor.Oficina(oficina).IncluirParticipante(inscricao);

                m_RepOficinas.Atualizar(oficina);

                oficinasDTO = ObterDivisaoOficinas(evento);
            });

            return oficinasDTO;
        }

        public IEnumerable<DTODivisaoOficina> RemoverTodasDivisoes(int idEvento)
        {
            IList<DTODivisaoOficina> oficinasDTO = new List<DTODivisaoOficina>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);
                
                IList<Oficina> oficinas = m_RepOficinas.ListarTodasPorEvento(evento.Id);
                
                foreach (var oficina in oficinas)
                {
                    oficina.RemoverTodosParticipantes();
                    m_RepOficinas.Atualizar(oficina);
                }

                oficinasDTO = ObterDivisaoOficinas(evento);
            });

            return oficinasDTO;
        }

        private IList<DTODivisaoOficina> ObterDivisaoOficinas(Evento evento)
        {
            List<DTODivisaoOficina> oficinasDTO = new List<DTODivisaoOficina>();

            IList<Oficina> oficina = m_RepOficinas.ListarTodasComParticipantesPorEvento(evento);
            IList<InscricaoParticipante> participantesSemSala = m_RepOficinas.ListarParticipantesSemOficinaNoEvento(evento);
            IList<AtividadeInscricaoOficinasCoordenacao> coordenadores =
                 m_RepInscricoes.ListarTodasInscricoesAceitasPorAtividade<AtividadeInscricaoOficinasCoordenacao>(evento);


            oficinasDTO.AddRange(oficina.Select(x => new DTODivisaoOficina
            {
                Id = x.Id,
                Nome = x.Nome,
                Coordenadores = coordenadores
                                    .Where(c => c.OficinaEscolhida == x)
                                    .Select(c => c.Inscrito.ConverterBasico()),
                Participantes = x.Participantes
                                    .Select(i => i.ConverterBasico())
            }));

            if (participantesSemSala.Count > 0)
            {
                oficinasDTO.Add(new DTODivisaoOficina
                {
                    Id = 0,
                    Nome = "Participantes sem oficina definida",
                    Coordenadores = new List<DTOBasicoInscricao>(),
                    Participantes = new List<DTOBasicoInscricao>(
                            participantesSemSala.Select(x => x.ConverterBasico()))
                });
            }

            return oficinasDTO;
        }
    }
}
