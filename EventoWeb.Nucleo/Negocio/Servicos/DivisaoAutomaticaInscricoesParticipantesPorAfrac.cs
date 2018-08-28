using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class DivisaoAutomaticaInscricoesParticipantesPorAfrac
    {
        private class OrdenaDivisao
        {
            public InscricaoParticipante Inscrito { get; set; }
            public Afrac Afrac { get; set; }
            public int PosicaoAfrac { get; set; }
        }

        private Evento mEvento;
        private AInscricoes mRepositorioInscricoes;
        private AAfracs mRepositorioAfracs;

        public DivisaoAutomaticaInscricoesParticipantesPorAfrac(Evento evento, AInscricoes inscricoes, AAfracs afracs)
        {
            if (inscricoes == null)
                throw new ArgumentNullException("inscricoes", "Repositorio de inscrições não informado.");

            if (afracs == null)
                throw new ArgumentNullException("afracs", "Repositorio de afracs não informado.");

            if (evento == null)
                throw new ArgumentNullException("Evento", "Evento não pode ser nulo.");

            mRepositorioInscricoes = inscricoes;
            mEvento = evento;
            mRepositorioAfracs = afracs;
        }

        public virtual IList<Afrac> Dividir()
        {
            var afracs = mRepositorioAfracs.ListarTodasAfracsComParticipantesPorEvento(mEvento);

            if (afracs.Count() == 0)
                throw new InvalidOperationException("Não há afracs para realizar a divisão.");

            var listaOrdenada = new List<OrdenaDivisao>();

            var participantesDividir = mRepositorioInscricoes
                .ListarTodasInscricoesPorAtividade<AtividadeInscricaoOficinas>(mEvento)
                .ToList();

            foreach (var afrac in afracs)
                afrac.RemoverTodosParticipantes();

            foreach (var inscricao in participantesDividir)
            {
                int posicao = 1;
                foreach (var afrac in inscricao.Afracs)
                {
                    listaOrdenada.Add(new OrdenaDivisao()
                    {
                        Afrac = afrac,
                        Inscrito = inscricao.Inscrito,
                        PosicaoAfrac = posicao
                    });

                    posicao++;
                }
            }

            for (var posicao = 1; posicao <= afracs.Count; posicao++)
            {
                var inscricoesSelecionadas = listaOrdenada
                                            .Where(i => i.PosicaoAfrac == posicao)
                                            .OrderBy(i => i.Afrac.Id)
                                            .ThenBy(i => i.Inscrito.DataRecebimento)
                                            .GroupBy(x=>x.Afrac);

                foreach (var grupo in inscricoesSelecionadas)
                {
                    var afrac = afracs.First(a => a == grupo.Key);

                    if (afrac.NumeroTotalParticipantes == null ||
                        (afrac.NumeroTotalParticipantes != null && afrac.Participantes.Count() < afrac.NumeroTotalParticipantes))
                    {
                        var quantosInscritosIncluir = grupo.Count();

                        if (afrac.NumeroTotalParticipantes != null && 
                            afrac.Participantes.Count() + quantosInscritosIncluir >= afrac.NumeroTotalParticipantes)
                            quantosInscritosIncluir = afrac.NumeroTotalParticipantes.Value - afrac.Participantes.Count();

                        if (afrac.DeveSerParNumeroTotalParticipantes && quantosInscritosIncluir % 2 != 0)
                            quantosInscritosIncluir = quantosInscritosIncluir - 1;

                        for (var indice = 0; indice < quantosInscritosIncluir; indice++)
                        {
                            afrac.AdicionarParticipante(grupo.ElementAt(indice).Inscrito);
                            listaOrdenada.RemoveAll(l => l.Inscrito.Id == grupo.ElementAt(indice).Inscrito.Id);
                        }
                    }
                }

                foreach (var item in inscricoesSelecionadas.SelectMany(grupo => grupo))
                    listaOrdenada.Remove(item);
            }

            return afracs;
        }        
    }
}
