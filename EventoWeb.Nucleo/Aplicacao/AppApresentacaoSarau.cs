using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppApresentacaoSarau : AppBase
    {
        public AppApresentacaoSarau(IContexto contexto)
            : base(contexto) { }

        internal void IncluirOuAtualizarPorParticipanteSemExecucaoSegura(InscricaoParticipante inscricao, IEnumerable<DTOSarau> dtoApresentacoes)
        {
            var repApresentacoes = Contexto.RepositorioApresentacoesSarau;
            var apresentacoes = repApresentacoes.ListarPorInscricao(inscricao.Id);
            var apresentacoesRemovidas = apresentacoes.Where(x => dtoApresentacoes.Count(y => y.Id == x.Id) == 0).ToList();
            foreach (var apresentacao in apresentacoesRemovidas)
            {
                if (apresentacao.Inscritos.Count() == 1)
                    repApresentacoes.Excluir(apresentacao);
                else
                {
                    var inscritos = new List<Inscricao>(apresentacao.Inscritos);
                    inscritos.Remove(inscricao);

                    apresentacao.AtualizarInscricoes(inscritos);

                    repApresentacoes.Atualizar(apresentacao);
                }
            }

            foreach (var dtoSarau in dtoApresentacoes)
            {
                if (dtoSarau.Id != null)
                {
                    var apresentacao = repApresentacoes.ObterPorId(inscricao.Evento.Id, dtoSarau.Id.Value);
                    apresentacao.DuracaoMin = dtoSarau.DuracaoMin;
                    apresentacao.Tipo = dtoSarau.Tipo;
                    if (apresentacao.Inscritos.Count(x => x == inscricao) == 0)
                    {
                        var inscritos = new List<Inscricao>(apresentacao.Inscritos)
                        {
                            inscricao
                        };

                        apresentacao.AtualizarInscricoes(inscritos);
                    }

                    repApresentacoes.Atualizar(apresentacao);
                }
                else
                {
                    var apresentacao = new ApresentacaoSarau(inscricao.Evento, dtoSarau.DuracaoMin, dtoSarau.Tipo,
                        new List<Inscricao>() { inscricao });
                    repApresentacoes.Incluir(apresentacao);
                }
            }
        }
    }
}
