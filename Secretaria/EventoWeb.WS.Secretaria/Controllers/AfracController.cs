using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AfracController : ControllerBase
    {
        private AppAfracs mAppAfracs;

        public AfracController(IContexto contexto)
        {
            mAppAfracs = new AppAfracs(contexto);
        }

        // GET api/afrac/obter/1
        [Authorize("Bearer")]
        [HttpGet("obter")]
        public DTOAfrac GetObterAfrac(int id)
        {
            var afrac = mAppAfracs.ObterPorId(id);
            return afrac;
        }

        // GET api/afrac/?idEvento=5
        [Authorize("Bearer")]
        [HttpGet("listarTodos")]
        public IEnumerable<DTOAfrac> ListarAfracs(int idEvento)
        {
            var lista = mAppAfracs.ObterTodos(idEvento);

            return lista;
        }

        // POST api/afrac/?idEvento=5
        [Authorize("Bearer")]
        [HttpPost("criar")]
        public DTOId IncluirAfrac(int idEvento, [FromBody] DTOAfrac dtoAfrac)
        {
            var id = mAppAfracs.Incluir(idEvento, dtoAfrac);

            return id;
        }

        // PUT api/afrac/1
        [Authorize("Bearer")]
        [HttpPut("atualizar")]
        public void AlterarAfrac(int id, [FromBody] DTOAfrac dtoAfrac)
        {
            mAppAfracs.Atualizar(id, dtoAfrac);
        }

        // DELETE api/afrac/2
        [Authorize("Bearer")]
        [HttpDelete("excluir")]
        public void ExcluirAfrac(int id)
        {
            mAppAfracs.Excluir(id);
        }

        /*[Authorize("Bearer")]
        [HttpPost("dividirAutInscricoes")]
        public Object DividirAutomaticamenteInscricoes(int idEvento)
        {
            Evento evento = BuscarEventoPeloId(idEvento);

            var divisor = new DivisaoAutomaticaInscricoesParticipantesPorAfrac(evento, mRepositorioInscricoes, mRepositorioAfracs);
            IList<Afrac> afracs = divisor.Dividir();

            var alteracao = new AlteracaoAfrac(mRepositorioAfracs);

            foreach (var afrac in afracs)
                alteracao.Persistir(afrac);

            return GerarDTODivisaoAfrac(evento);
        }

        [Authorize("Bearer")]
        [HttpPost("incluirParticipantesAfrac")]
        public Object IncluirParticipantesAfrac(int idEvento, int id, [FromBody] int[] idsParticipantesIncluir)
        {
            Evento evento = BuscarEventoPeloId(idEvento);
            Afrac afrac = BuscarAfracEstudoPeloId(idEvento, id);

            var divisor = new DivisaoManualParticipantesPorAfrac(evento, mRepositorioAfracs);

            foreach (var idParticipante in idsParticipantesIncluir)
            {
                InscricaoParticipante participante = (InscricaoParticipante)
                    mRepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idParticipante);

                divisor.Afrac(afrac).IncluirParticipante(participante);
            }

            new AlteracaoAfrac(mRepositorioAfracs).Persistir(afrac);

            return GerarDTODivisaoAfrac(evento);
        }

        [Authorize("Bearer")]
        [HttpPut("moverParticipantesAfrac")]
        public Object MoverParticipantesAfrac(int idEvento, int idAfracOrigem, int idAfracDestino,
            [FromBody] int[] idsParticipantesMover)
        {
            Evento evento = BuscarEventoPeloId(idEvento);
            Afrac afracOrigem = BuscarAfracEstudoPeloId(idEvento, idAfracOrigem);
            Afrac afracDestino = BuscarAfracEstudoPeloId(idEvento, idAfracDestino);

            var divisor = new DivisaoManualParticipantesPorAfrac(evento, mRepositorioAfracs);

            foreach (var idParticipante in idsParticipantesMover)
            {
                InscricaoParticipante participante = (InscricaoParticipante)
                    mRepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idParticipante);

                divisor.Afrac(afracOrigem).MoverParticipante(participante).Para(afracDestino);
            }


            new AlteracaoAfrac(mRepositorioAfracs).Persistir(afracOrigem);
            new AlteracaoAfrac(mRepositorioAfracs).Persistir(afracDestino);

            return GerarDTODivisaoAfrac(evento);
        }

        [Authorize("Bearer")]
        [HttpDelete("excluirDivisaoAfracs")]
        public Object ExcluirDivisaoAfracs(int idEvento)
        {
            Evento evento = BuscarEventoPeloId(idEvento);
            IList<Afrac> salas = mRepositorioAfracs.ListarTodasAfracsPorEvento(idEvento);

            var alteracao = new AlteracaoAfrac(mRepositorioAfracs);

            foreach (var sala in salas)
            {
                sala.RemoverTodosParticipantes();
                alteracao.Persistir(sala);
            }

            return GerarDTODivisaoAfrac(evento);
        }

        [Authorize("Bearer")]
        [HttpDelete("excluirParticipantesAfrac")]
        public Object ExcluirParticipanteDeAfrac(int idEvento, int id, int[] inscritosExcluir)
        {
            Evento evento = BuscarEventoPeloId(idEvento);
            Afrac afrac = BuscarAfracEstudoPeloId(idEvento, id);
            var divisor = new DivisaoManualParticipantesPorAfrac(evento, mRepositorioAfracs);
            foreach (var idInscrito in inscritosExcluir)
            {
                var participante = (InscricaoParticipante)mRepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscrito);
                divisor.Afrac(afrac).RemoverParticipante(participante);
            }

            new AlteracaoAfrac(mRepositorioAfracs).Persistir(afrac);

            return GerarDTODivisaoAfrac(evento);
        }

        [Authorize("Bearer")]
        [HttpGet("obterDivisaoAfracs")]
        public Object ObterDivisaoAfracs(int idEvento)
        {
            Evento evento = BuscarEventoPeloId(idEvento);
            var lista = GerarDTODivisaoAfrac(evento);

            return lista;
        }

        [Authorize("Bearer")]
        [HttpGet("relatorioDivisao")]
        public String GerarRelatorioDivisao(int idEvento)
        {
            Evento evento = BuscarEventoPeloId(idEvento);

            var relatorio = new GeradorRelatorioDivisaoAfrac()
                .Gerar(mRepositorioAfracs.ListarTodasAfracsComParticipantesPorEvento(evento),
                       mRepositorioInscricoes.ListarTodasInscricoesPorAtividade<AtividadeInscricaoOficinasCoordenacao>(evento));

            using (var memoryStream = new MemoryStream())
            {
                relatorio.Position = 0;
                relatorio.CopyTo(memoryStream);
                var relatorioEmBytes = memoryStream.ToArray();

                return Convert.ToBase64String(relatorioEmBytes);
            }
        }

        private Object GerarDTODivisaoAfrac(Evento evento)
        {
            IList<Afrac> afracs = mRepositorioAfracs
                .ListarTodasAfracsComParticipantesPorEvento(evento);
            IList<InscricaoParticipante> participantesSemAfrac = mRepositorioAfracs
                .ListarParticipantesSemAfracNoEvento(evento);
            IList<AtividadeInscricaoOficinasCoordenacao> coordenadores =
                mRepositorioInscricoes.ListarTodasInscricoesPorAtividade<AtividadeInscricaoOficinasCoordenacao>(evento);

            List<Object> afracsDTO = new List<Object>();
            afracsDTO.AddRange(afracs.Select(x => new
            {
                x.Id,
                x.Nome,
                Responsaveis = coordenadores
                                    .Where(c => c.AfracEscolhida == x)
                                    .Select(c => mConversorInscricaoBasico.Converter(c.Inscrito)),
                Participantes = x.Participantes
                                    .Select(i => mConversorInscricaoBasico.Converter(i))
            }));

            if (participantesSemAfrac.Count > 0)
            {
                afracsDTO.Add(new 
                {
                    Id = 0,
                    Nome = "Participantes sem afrac definida",
                    Responsaveis = new List<DTOInscricaoBasico>(),
                    Participantes = new List<DTOInscricaoBasico>(
                        participantesSemAfrac.Select(x => mConversorInscricaoBasico.Converter(x)))
                });
            }

            return new
            {
                lista = afracsDTO
            };
        }

        private Evento BuscarEventoPeloId(int idEvento)
        {
            Evento evento = mRepositorioEvento.ObterEventoPeloId(idEvento);

            if (evento == null)
                throw new Exception("Evento não encontrado com este id");

            return evento;
        }

        private Afrac BuscarAfracEstudoPeloId(int idEvento, int idSala)
        {
            Afrac afrac = mRepositorioAfracs.ObterAfracPorIdEventoEAfrac(idEvento, idSala);

            if (afrac == null)
                throw new Exception("Afrac não encontrada com este id no evento informado.");

            return afrac;
        }

        private Object Converter(Afrac afrac)
        {
            return new
            {
                Id = afrac.Id,
                Nome = afrac.Nome,
                DeveSerParNumeroTotalParticipantes = afrac.DeveSerParNumeroTotalParticipantes,
                NumeroTotalParticipantes = afrac.NumeroTotalParticipantes,
            };
        }*/
    }
}