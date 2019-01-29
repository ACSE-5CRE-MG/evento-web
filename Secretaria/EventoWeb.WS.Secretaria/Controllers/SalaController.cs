using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaController : ControllerBase
    {
        private AppSalasEstudo mAppSalasEstudo;

        public SalaController(IContexto contexto)
        {
            mAppSalasEstudo = new AppSalasEstudo(contexto);
        }

        [Authorize("Bearer")]
        [HttpGet("obter")]
        public Object GetObter(int id)
        {
            var sala = mAppSalasEstudo.ObterPorId(id);
            return sala;
        }

        [Authorize("Bearer")]
        [HttpGet("listarTodas")]
        public IEnumerable<DTOSalaEstudo> ListarTudo(int idEvento)
        {
            var lista = mAppSalasEstudo.ObterTodos(idEvento);
            return lista;
        }

        // POST api/afrac/?idEvento=5
        [Authorize("Bearer")]
        [HttpPost("criar")]
        public DTOId IncluirSala(int idEvento, [FromBody] DTOSalaEstudo dtoSala)
        {
            var id = mAppSalasEstudo.Incluir(idEvento, dtoSala);
            return id;
        }

        [Authorize("Bearer")]
        [HttpPut("atualizar")]
        public void AlterarSala(int id, [FromBody] DTOSalaEstudo dtoSala)
        {
            mAppSalasEstudo.Atualizar(id, dtoSala);
        }

        [Authorize("Bearer")]
        [HttpDelete("excluir")]
        public void ExcluirSala(int id)
        {
            mAppSalasEstudo.Excluir(id);
        }

        /*
        [HttpPost]
        [ValidacaoToken]
        [SessaoPorRequisicao]
        [ActionName("dividirAutInscricoes")]
        public Object DividirAutomaticamenteInscricoesPorSala(int idEvento)
        {
            Evento evento = BuscarEventoPeloId(idEvento);
            IList<SalaEstudo> salas = null;

            if (evento.ModeloDivisaoSalasEstudo.Value == EnumModeloDivisaoSalasEstudo.PorIdadeCidade)
            {
                DivisaoAutomaticaInscricoesParticipantePorSalaEstudo divisor =
                    new DivisaoAutomaticaInscricoesParticipantePorSalaEstudo(evento, mInscricoes, mSalasEstudo);
                salas = divisor.Dividir();
            }
            else
                salas = new DivisaoAutomaticaInscricoesParticipantesPorSalaEstudoEscolha(evento, mInscricoes, mSalasEstudo).Dividir();            

            foreach (var sala in salas)
                mSalasEstudo.Atualizar(sala);

            return ObterDivisaoDeParticipantesPorSala(evento);
        }

        [HttpPost]
        [ValidacaoToken]
        [SessaoPorRequisicao]
        [ActionName("incluirParticipantesSala")]
        public Object IncluirParticipantesSala(int idEvento, int id, [FromBody] int[] idsParticipantesIncluir)
        {
            Evento evento = BuscarEventoPeloId(idEvento);
            SalaEstudo sala = BuscarSalaEstudoPeloId(idEvento, id);

            DivisaoManualParticipantesPorSalaEstudo divisor =
                new DivisaoManualParticipantesPorSalaEstudo(evento, mSalasEstudo);

            foreach(var idParticipante in idsParticipantesIncluir) {
                InscricaoParticipante participante = (InscricaoParticipante) 
                    mInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idParticipante);

                divisor.Sala(sala).IncluirParticipante(participante);
            }

            new AlteracaoSalaEstudo(mSalasEstudo).Persistir(sala);

            return ObterDivisaoDeParticipantesPorSala(evento); 
        }

        [HttpPut]
        [ValidacaoToken]
        [SessaoPorRequisicao]
        [ActionName("moverParticipantesSala")]
        public Object MoverParticipantesSala(int idEvento, int idSalaOrigem, int idSalaDestino, 
            [FromBody] int[] idsParticipantesMover)
        {
            Evento evento = BuscarEventoPeloId(idEvento);
            SalaEstudo salaOrigem = BuscarSalaEstudoPeloId(idEvento, idSalaOrigem);
            SalaEstudo salaDestino = BuscarSalaEstudoPeloId(idEvento, idSalaDestino);

            DivisaoManualParticipantesPorSalaEstudo divisor =
                new DivisaoManualParticipantesPorSalaEstudo(evento, mSalasEstudo);

            foreach (var idParticipante in idsParticipantesMover)
            {
                InscricaoParticipante participante = (InscricaoParticipante)
                    mInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idParticipante);

                divisor.Sala(salaOrigem).MoverParticipante(participante).Para(salaDestino);
            }

            var alteracao = new AlteracaoSalaEstudo(mSalasEstudo);
            alteracao.Persistir(salaOrigem);
            alteracao.Persistir(salaDestino);

            return ObterDivisaoDeParticipantesPorSala(evento);
        }

        [HttpDelete]
        [ValidacaoToken]
        [SessaoPorRequisicao]
        [ActionName("excluirDivisaoSalas")]
        public Object ExcluirDivisaoSalas(int idEvento)
        {
            Evento evento = BuscarEventoPeloId(idEvento);
            IList<SalaEstudo> salas = mSalasEstudo.ListarTodasSalasEstudoDoEvento(evento);

            var alteracao = new AlteracaoSalaEstudo(mSalasEstudo);

            foreach (var sala in salas)
            {
                sala.RemoverTodosParticipantes();
                alteracao.Persistir(sala);
            }

            return ObterDivisaoDeParticipantesPorSala(evento);
        }

        [HttpDelete]
        [ValidacaoToken]
        [SessaoPorRequisicao]
        [ActionName("excluirParticipantesSala")]
        public Object ExcluirParticipanteDeSala(int idEvento, int id, int[] inscritosExcluir)
        {
            Evento evento = BuscarEventoPeloId(idEvento);
            SalaEstudo sala = BuscarSalaEstudoPeloId(idEvento, id);
            DivisaoManualParticipantesPorSalaEstudo divisor = new DivisaoManualParticipantesPorSalaEstudo(
                evento, mSalasEstudo);
            foreach (var idInscrito in inscritosExcluir)
            {
                var participante = (InscricaoParticipante)mInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscrito);
                divisor.Sala(sala).RemoverParticipante(participante);
            }

            new AlteracaoSalaEstudo(mSalasEstudo).Persistir(sala);

            return ObterDivisaoDeParticipantesPorSala(evento);
        }

        [HttpGet]
        [ValidacaoToken]
        [SessaoPorRequisicao]
        [ActionName("obterDivisaoSalas")]
        public Object ObterDivisaoSalas(int idEvento)
        {
            Evento evento = BuscarEventoPeloId(idEvento);

            return ObterDivisaoDeParticipantesPorSala(evento);
        }

        [HttpGet]
        [ValidacaoToken]
        [SessaoPorRequisicao()]
        [ActionName("relatorioDivisao")]
        public String GerarRelatorioDivisao(int idEvento)
        {
            Evento evento = BuscarEventoPeloId(idEvento);

            var relatorio = new GeradorRelatorioDivisaoSalasEstudo()
                .Gerar(mSalasEstudo.ListarTodasSalasEstudoComParticipantesDoEvento(evento),
                       mInscricoes.ListarTodasInscricoesPorAtividade<AtividadeInscricaoSalaEstudoCoordenacao>(evento));

            using (var memoryStream = new MemoryStream())
            {
                relatorio.Position = 0;
                relatorio.CopyTo(memoryStream);
                var relatorioEmBytes = memoryStream.ToArray();

                return Convert.ToBase64String(relatorioEmBytes); 
            }
        }

        private Object Converter(SalaEstudo sala)
        {
            return new
            {
                Id = sala.Id,
                Nome = sala.Nome,
                sala.FaixaEtaria
            };
        }

        private Evento BuscarEventoPeloId(int idEvento)
        {
            Evento evento = mEventos.ObterEventoPeloId(idEvento);

            if (evento == null)
                throw new Exception("Evento não encontrado com este id");

            return evento;
        }

        private SalaEstudo BuscarSalaEstudoPeloId(int idEvento, int idSala)
        {
            SalaEstudo sala = mSalasEstudo.ObterSalaEstudoDoEventoPorId(BuscarEventoPeloId(idEvento), idSala);

            if (sala == null)
                throw new Exception("Sala não encontrada com este id no evento informado.");

            return sala;
        }

        private Object ObterDivisaoDeParticipantesPorSala(Evento evento)
        {
            IList<SalaEstudo> salas = mSalasEstudo.ListarTodasSalasEstudoComParticipantesDoEvento(evento);
            IList<InscricaoParticipante> participantesSemSala = mSalasEstudo.ListarParticipantesSemSalaEstudoNoEvento(evento);
            IList<AtividadeInscricaoSalaEstudoCoordenacao> coordenadores = 
                mInscricoes.ListarTodasInscricoesPorAtividade<AtividadeInscricaoSalaEstudoCoordenacao>(evento);
            var conversorInscricaoBasico = new ConversorInscricaoDTOInscricaoBasico();

            List<Object> salasDTO = new List<Object>();
            salasDTO.AddRange(salas.Select(x => new
            {
                x.Id,
                x.Nome,
                Coordenadores = coordenadores
                                    .Where(c => c.SalaEscolhida == x)
                                    .Select(c => conversorInscricaoBasico.Converter(c.Inscrito)),
                Participantes = x.Participantes
                                    .Select(i => conversorInscricaoBasico.Converter(i))
            }));

            if (participantesSemSala.Count > 0)
            {
                salasDTO.Add(new 
                    {
                        Id = 0,
                        Nome = "Participantes sem sala definida",
                        Coordenadores = new List<DTOInscricaoBasico>(),
                        Participantes = new List<DTOInscricaoBasico>(
                            participantesSemSala.Select(x=> conversorInscricaoBasico.Converter(x)))
                    });
            }

            return new
                {
                    lista = salasDTO
                };
        }*/
    }
}
