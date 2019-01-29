using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class DivisaoAutomaticaInscricoesParticipantesPorSalaEstudoEscolha
    {
        private class OrdenaDivisao
        {
            public InscricaoParticipante Inscrito { get; set; }
            public SalaEstudo Sala { get; set; }
            public int PosicaoSala { get; set; }
        }

        private Evento mEvento;
        private AInscricoes mRepositorioInscricoes;
        private ASalasEstudo mRepositorioSalas;

        public DivisaoAutomaticaInscricoesParticipantesPorSalaEstudoEscolha(Evento evento, AInscricoes inscricoes, ASalasEstudo salas)
        {
            if (inscricoes == null)
                throw new ArgumentNullException("inscricoes", "Repositorio de inscrições não informado.");

            if (salas == null)
                throw new ArgumentNullException("salas", "Repositorio de salas não informado.");

            if (evento == null)
                throw new ArgumentNullException("Evento", "Evento não pode ser nulo.");

            if (evento.ConfiguracaoSalaEstudo.ModeloDivisao != EnumModeloDivisaoSalasEstudo.PorOrdemEscolhaInscricao)
                throw new ArgumentException("O evento não aceita este tipo de divisão automática.");

            mRepositorioInscricoes = inscricoes;
            mEvento = evento;
            mRepositorioSalas = salas;
        }

        public virtual IList<SalaEstudo> Dividir()
        {
            var salas = mRepositorioSalas.ListarTodasSalasEstudoComParticipantesDoEvento(mEvento);

            if (salas.Count == 0)
                throw new InvalidOperationException("Não há salas para realizar a divisão.");

            var listaOrdenada = new List<OrdenaDivisao>();

            var participantesDividir = mRepositorioInscricoes
                .ListarTodasInscricoesPorAtividade<AtividadeInscricaoSalaEstudoOrdemEscolha>(mEvento)
                .ToList();

            int capacidadeParticipantesPorSala = participantesDividir.Count / salas.Count;

            if (participantesDividir.Count % salas.Count != 0)
                capacidadeParticipantesPorSala = capacidadeParticipantesPorSala + 1;

            foreach (var afrac in salas)
                afrac.RemoverTodosParticipantes();

            foreach (var inscricao in participantesDividir)
            {
                int posicao = 1;
                foreach (var sala in inscricao.Salas)
                {
                    listaOrdenada.Add(new OrdenaDivisao()
                    {
                        Sala = sala,
                        Inscrito = inscricao.Inscrito,
                        PosicaoSala = posicao
                    });

                    posicao++;
                }
            }

            for (var posicao = 1; posicao <= salas.Count; posicao++)
            {
                var inscricoesSelecionadas = listaOrdenada
                                            .Where(i => i.PosicaoSala == posicao)
                                            .OrderBy(i => i.Sala.Id)
                                            .ThenBy(i => i.Inscrito.DataRecebimento)
                                            .GroupBy(x=>x.Sala);

                foreach (var grupo in inscricoesSelecionadas)
                {
                    var sala = salas.First(a => a == grupo.Key);

                    if (sala.Participantes.Count() < capacidadeParticipantesPorSala)
                    {
                        var quantosInscritosIncluir = grupo.Count();

                        if (sala.Participantes.Count() + quantosInscritosIncluir >= capacidadeParticipantesPorSala)
                            quantosInscritosIncluir = capacidadeParticipantesPorSala - sala.Participantes.Count();

                        for (var indice = 0; indice < quantosInscritosIncluir; indice++)
                        {
                            sala.AdicionarParticipante(grupo.ElementAt(indice).Inscrito);
                            listaOrdenada.RemoveAll(l => l.Inscrito.Id == grupo.ElementAt(indice).Inscrito.Id);
                        }
                    }
                }

                foreach (var item in inscricoesSelecionadas.SelectMany(grupo => grupo))
                    listaOrdenada.Remove(item);
            }

            return salas;
        }        
    }
}
