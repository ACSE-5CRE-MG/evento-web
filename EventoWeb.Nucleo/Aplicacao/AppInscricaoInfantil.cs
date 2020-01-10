using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppInscricaoInfantil : AppBase
    {
        public AppInscricaoInfantil(IContexto contexto)
            : base(contexto) { }

        internal void IncluirOuAtualizarPorParticipanteSemExecucaoSegura(InscricaoParticipante inscricao, IEnumerable<DTOCrianca> dtoCriancas)
        {
            var repInscricoes = Contexto.RepositorioInscricoes;

            var inscricoesInfantis = repInscricoes.ListarInscricoesInfantisDoResponsavel(inscricao);
            var inscInfantilsRemovidas = inscricoesInfantis.Where(x => dtoCriancas.Count(y => y.Id == x.Id) == 0).ToList();
            foreach (var crianca in inscInfantilsRemovidas)
            {
                if (crianca.InscricaoResponsavel1 == inscricao)
                {
                    if (crianca.InscricaoResponsavel2 == null)
                        repInscricoes.Excluir(crianca);
                    else
                    {
                        crianca.AtribuirResponsaveis(crianca.InscricaoResponsavel2, null);
                        repInscricoes.Atualizar(crianca);
                    }
                }
                else
                {
                    crianca.AtribuirResponsaveis(crianca.InscricaoResponsavel1, null);
                    repInscricoes.Atualizar(crianca);
                }
            }

            foreach (var dtoCrianca in dtoCriancas)
            {
                InscricaoInfantil inscricaoInfantil;
                if (dtoCrianca.Id == null)
                {
                    inscricaoInfantil = new InscricaoInfantil(
                        inscricao.Evento,
                        new Pessoa(dtoCrianca.Nome, new Endereco(dtoCrianca.Cidade, dtoCrianca.Uf), dtoCrianca.DataNascimento, dtoCrianca.Sexo, dtoCrianca.Email),
                        inscricao,
                        DateTime.Today);
                    inscricaoInfantil.AtribuirDados(dtoCrianca);

                    repInscricoes.Incluir(inscricaoInfantil);

                    dtoCrianca.Id = inscricaoInfantil.Id;
                }
                else
                {
                    inscricaoInfantil = (InscricaoInfantil)repInscricoes.ObterInscricaoPeloIdEventoEInscricao(inscricao.Evento.Id, dtoCrianca.Id.Value);
                    inscricaoInfantil.AtribuirDados(dtoCrianca);

                    if (inscricaoInfantil.InscricaoResponsavel2 == null)
                        inscricaoInfantil.AtribuirResponsaveis(inscricaoInfantil.InscricaoResponsavel1, inscricao);

                    repInscricoes.Atualizar(inscricaoInfantil);
                }
            }
        }
    }
}
