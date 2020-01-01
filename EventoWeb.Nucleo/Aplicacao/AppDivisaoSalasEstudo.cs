using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using EventoWeb.Nucleo.Negocio.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppDivisaoSalasEstudo : AppBase
    {
        private readonly AEventos m_RepEventos;
        private readonly ASalasEstudo m_RepSalas;
        private readonly AInscricoes m_RepInscricoes;

        public AppDivisaoSalasEstudo(IContexto contexto, AEventos repEventos, ASalasEstudo repSalasEstudo, 
            AInscricoes repInscricoes) : base(contexto)
        {
            m_RepEventos = repEventos ?? throw new ExcecaoAplicacao("AppDivisaoSalasEstudo", "repEventos não pode ser nulo");
            m_RepInscricoes = repInscricoes ?? throw new ExcecaoAplicacao("AppDivisaoSalasEstudo", "repInscricoes não pode ser nulo"); 
            m_RepSalas = repSalasEstudo ?? throw new ExcecaoAplicacao("AppDivisaoSalasEstudo", "repSalasEstudo não pode ser nulo");
        }

        public IEnumerable<DTODivisaoSalaEstudo> ObterDivisao(int idEvento)
        {
            IList<DTODivisaoSalaEstudo> salasDTO = new List<DTODivisaoSalaEstudo>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);
                salasDTO = ObterDivisaoSalas(evento);
            });

            return salasDTO;
        }

        public IEnumerable<DTODivisaoSalaEstudo> RealizarDivisaoAutomatica(int idEvento)
        {
            IList<DTODivisaoSalaEstudo> salasDTO = new List<DTODivisaoSalaEstudo>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);                
                IList<SalaEstudo> salas;
                if (evento.ConfiguracaoSalaEstudo == EnumModeloDivisaoSalasEstudo.PorIdadeCidade)
                {
                    DivisaoAutomaticaInscricoesParticipantePorSalaEstudo divisor =
                        new DivisaoAutomaticaInscricoesParticipantePorSalaEstudo(evento, m_RepInscricoes, m_RepSalas);
                    salas = divisor.Dividir();
                }
                else
                {
                    DivisaoAutomaticaInscricoesParticipantesPorSalaEstudoEscolha divisor =
                        new DivisaoAutomaticaInscricoesParticipantesPorSalaEstudoEscolha(evento, m_RepInscricoes, m_RepSalas);
                    salas = divisor.Dividir();
                }

                foreach (var sala in salas)
                    m_RepSalas.Atualizar(sala);

                salasDTO = ObterDivisaoSalas(evento);
            });

            return salasDTO;
        }

        public IEnumerable<DTODivisaoSalaEstudo> MoverParticipante(int idEvento, int idInscricao, int daIdSala, int paraIdSala)
        {
            IList<DTODivisaoSalaEstudo> salasDTO = new List<DTODivisaoSalaEstudo>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);
                SalaEstudo salaOrigem = m_RepSalas.ObterPorId(idEvento, daIdSala);
                SalaEstudo salaDestino = m_RepSalas.ObterPorId(idEvento, paraIdSala);

                InscricaoParticipante participante = (InscricaoParticipante)
                        m_RepInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);

                DivisaoManualParticipantesPorSalaEstudo divisor =
                    new DivisaoManualParticipantesPorSalaEstudo(evento, m_RepSalas);

                divisor.Sala(salaOrigem).MoverParticipante(participante).Para(salaDestino);

                m_RepSalas.Atualizar(salaOrigem);
                m_RepSalas.Atualizar(salaDestino);

                salasDTO = ObterDivisaoSalas(evento);
            });

            return salasDTO;
        }

        public IEnumerable<DTODivisaoSalaEstudo> RemoverParticipante(int idEvento, int idInscricao, int idSala)
        {
            IList<DTODivisaoSalaEstudo> salasDTO = new List<DTODivisaoSalaEstudo>();
            ExecutarSeguramente(() =>
            {
                var evento = m_RepEventos.ObterEventoPeloId(idEvento);
                var sala = m_RepSalas.ObterPorId(idEvento, idSala);
                var participante = (InscricaoParticipante)m_RepInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);

                var divisor = new DivisaoManualParticipantesPorSalaEstudo(
                    evento, m_RepSalas);

                divisor.Sala(sala).RemoverParticipante(participante);

                m_RepSalas.Atualizar(sala);

                salasDTO = ObterDivisaoSalas(evento);
            });

            return salasDTO;
        }

        public IEnumerable<DTODivisaoSalaEstudo> IncluirParticipante(int idEvento, int idSala, int idInscricao)
        {
            IList<DTODivisaoSalaEstudo> salasDTO = new List<DTODivisaoSalaEstudo>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);
                InscricaoParticipante inscricao = (InscricaoParticipante)m_RepInscricoes.ObterInscricaoPeloId(idInscricao);

                SalaEstudo sala = m_RepSalas.ObterPorId(idEvento, idSala);

                DivisaoManualParticipantesPorSalaEstudo divisor =
                    new DivisaoManualParticipantesPorSalaEstudo(evento, m_RepSalas);

                divisor.Sala(sala).IncluirParticipante(inscricao);

                m_RepSalas.Atualizar(sala);

                salasDTO = ObterDivisaoSalas(evento);
            });

            return salasDTO;
        }

        public IEnumerable<DTODivisaoSalaEstudo> RemoverTodasDivisoes(int idEvento)
        {
            IList<DTODivisaoSalaEstudo> salasDTO = new List<DTODivisaoSalaEstudo>();
            ExecutarSeguramente(() =>
            {
                Evento evento = m_RepEventos.ObterEventoPeloId(idEvento);
                
                IList<SalaEstudo> salas = m_RepSalas.ListarTodasPorEvento(evento.Id);
                
                foreach (var sala in salas)
                {
                    sala.RemoverTodosParticipantes();
                    m_RepSalas.Atualizar(sala);
                }

                salasDTO = ObterDivisaoSalas(evento);
            });

            return salasDTO;
        }

        private IList<DTODivisaoSalaEstudo> ObterDivisaoSalas(Evento evento)
        {
            Dictionary<EnumModeloDivisaoSalasEstudo, Func<Evento, IList<InscricaoParticipante>>> pesquisasParticipantesSemSala =
                new Dictionary<EnumModeloDivisaoSalasEstudo, Func<Evento, IList<InscricaoParticipante>>>()
                {
                    {EnumModeloDivisaoSalasEstudo.PorIdadeCidade,  m_RepSalas.ListarParticipantesSemSalaEstudoNormal},
                    {EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao,  m_RepSalas.ListarParticipantesSemSalaEstudoPorOrdem},
                };

            List<DTODivisaoSalaEstudo> salasDTO = new List<DTODivisaoSalaEstudo>();

            IList<SalaEstudo> salas = m_RepSalas.ListarTodasSalasEstudoComParticipantesDoEvento(evento);
            IList<InscricaoParticipante> participantesSemSala = pesquisasParticipantesSemSala[evento.ConfiguracaoSalaEstudo.Value](evento);
            IList<AtividadeInscricaoSalaEstudoCoordenacao> coordenadores =
                 m_RepInscricoes.ListarTodasInscricoesPorAtividade<AtividadeInscricaoSalaEstudoCoordenacao>(evento);


            salasDTO.AddRange(salas.Select(x => new DTODivisaoSalaEstudo
            {
                Id = x.Id,
                Nome = x.Nome,
                Coordenadores = coordenadores
                                    .Where(c => c.SalaEscolhida == x)
                                    .Select(c => c.Inscrito.ConverterBasico()),
                Participantes = x.Participantes
                                    .Select(i => i.ConverterBasico())
            }));

            if (participantesSemSala.Count > 0)
            {
                salasDTO.Add(new DTODivisaoSalaEstudo
                {
                    Id = 0,
                    Nome = "Participantes sem sala definida",
                    Coordenadores = new List<DTOBasicoInscricao>(),
                    Participantes = new List<DTOBasicoInscricao>(
                            participantesSemSala.Select(x => x.ConverterBasico()))
                });
            }

            return salasDTO;
        }
    }
}
