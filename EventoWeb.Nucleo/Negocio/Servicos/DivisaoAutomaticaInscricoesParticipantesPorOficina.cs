using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class DivisaoAutomaticaInscricoesParticipantesPorOficina
    {
        private class OrdenaDivisao
        {
            public InscricaoParticipante Inscrito { get; set; }
            public Oficina Oficina { get; set; }
            public int PosicaoOficina { get; set; }
        }

        private Evento mEvento;
        private AInscricoes mRepositorioInscricoes;
        private AOficinas mRepositorioOficinas;

        public DivisaoAutomaticaInscricoesParticipantesPorOficina(Evento evento, AInscricoes inscricoes, AOficinas oficinas)
        {
            if (inscricoes == null)
                throw new ArgumentNullException("inscricoes", "Repositorio de inscrições não informado.");

            if (oficinas == null)
                throw new ArgumentNullException("oficinas", "Repositorio de oficinas não informado.");

            if (evento == null)
                throw new ArgumentNullException("Evento", "Evento não pode ser nulo.");

            mRepositorioInscricoes = inscricoes;
            mEvento = evento;
            mRepositorioOficinas = oficinas;
        }

        public virtual IList<Oficina> Dividir()
        {
            var oficinas = mRepositorioOficinas.ListarTodasComParticipantesPorEvento(mEvento);

            if (oficinas.Count() == 0)
                throw new InvalidOperationException("Não há oficinas para realizar a divisão.");

            var listaOrdenada = new List<OrdenaDivisao>();

            var participantesDividir = mRepositorioInscricoes
                .ListarTodasInscricoesAceitasPorAtividade<AtividadeInscricaoOficinas>(mEvento)
                .ToList();

            foreach (var oficina in oficinas)
                oficina.RemoverTodosParticipantes();

            foreach (var inscricao in participantesDividir)
            {
                int posicao = 1;
                foreach (var oficina in inscricao.Oficinas)
                {
                    listaOrdenada.Add(new OrdenaDivisao()
                    {
                        Oficina = oficina,
                        Inscrito = inscricao.Inscrito,
                        PosicaoOficina = posicao
                    });

                    posicao++;
                }
            }

            for (var posicao = 1; posicao <= oficinas.Count; posicao++)
            {
                var inscricoesSelecionadas = listaOrdenada
                                            .Where(i => i.PosicaoOficina == posicao)
                                            .OrderBy(i => i.Oficina.Id)
                                            .ThenBy(i => i.Inscrito.DataRecebimento)
                                            .GroupBy(x=>x.Oficina);

                foreach (var grupo in inscricoesSelecionadas)
                {
                    var oficina = oficinas.First(a => a == grupo.Key);

                    if (oficina.NumeroTotalParticipantes == null ||
                        (oficina.NumeroTotalParticipantes != null && oficina.Participantes.Count() < oficina.NumeroTotalParticipantes))
                    {
                        var quantosInscritosIncluir = grupo.Count();

                        if (oficina.NumeroTotalParticipantes != null && 
                            oficina.Participantes.Count() + quantosInscritosIncluir >= oficina.NumeroTotalParticipantes)
                            quantosInscritosIncluir = oficina.NumeroTotalParticipantes.Value - oficina.Participantes.Count();

                        if (oficina.DeveSerParNumeroTotalParticipantes && quantosInscritosIncluir % 2 != 0)
                            quantosInscritosIncluir = quantosInscritosIncluir - 1;

                        for (var indice = 0; indice < quantosInscritosIncluir; indice++)
                        {
                            oficina.AdicionarParticipante(grupo.ElementAt(indice).Inscrito);
                            listaOrdenada.RemoveAll(l => l.Inscrito.Id == grupo.ElementAt(indice).Inscrito.Id);
                        }
                    }
                }

                foreach (var item in inscricoesSelecionadas.SelectMany(grupo => grupo))
                    listaOrdenada.Remove(item);
            }

            return oficinas;
        }        
    }
}
