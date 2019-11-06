using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppInscOnlineEventoManutencaoInscricoes : AppBase
    {
        public AppInscOnlineEventoManutencaoInscricoes(IContexto contexto)
            : base(contexto) { }

        public DTOInscricaoCompleta ObterInscricao(int id)
        {
            DTOInscricaoCompleta dto = null;
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(id);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoInfantil)
                        throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "A inscrição não pode ser infantil");

                    var inscParticipante = (InscricaoParticipante)inscricao;
                    dto = inscParticipante.Converter();

                    dto.Sarais = Contexto.RepositorioApresentacoesSarau.ListarPorInscricao(inscParticipante.Id)
                        .Select(x => x.Converter()).ToList();

                    var inscInfantis = Contexto.RepositorioInscricoes.ListarInscricoesInfantisDoResponsavel(inscParticipante)
                        .Select(x => x.Converter())
                        .ToList();
                }
            });
            return dto;
        }

        public void AtualizarInscricao(int id, DTOInscricaoAtualizacao dtoInscricao)
        {
            ExecutarSeguramente(() =>
            {
                var repInscricoes = Contexto.RepositorioInscricoes;
                var inscricao = repInscricoes.ObterInscricaoPeloId(id) ??
                    throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "Inscrição não encontrada");

                if (inscricao is InscricaoInfantil)
                    throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "Não é possível atualizar inscrição infantil");

                var inscParticipante = (InscricaoParticipante)inscricao;
                inscParticipante.AtribuirDados(dtoInscricao);                

                inscParticipante.RemoverTodasAtividade();
                inscParticipante.AtribuirAtividadeOficina(dtoInscricao.Oficina, Contexto.RepositorioOficinas);
                inscParticipante.AtribuirAtividadeSalaEstudo(dtoInscricao.SalasEstudo, Contexto.RepositorioSalasEstudo);
                inscParticipante.AtribuirAtividadeDepartamento(dtoInscricao.Departamento, Contexto.RepositorioDepartamentos);

                inscParticipante.TornarPendente();
                repInscricoes.Atualizar(inscParticipante);

                new AppInscricaoInfantil(Contexto)
                    .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(inscParticipante, dtoInscricao.Criancas);

                var appApresentacaoSarau = new AppApresentacaoSarau(Contexto);
                appApresentacaoSarau
                    .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(inscParticipante, dtoInscricao.Sarais);

                foreach(var dtoCrianca in dtoInscricao.Criancas.Where(x=>x.Sarais?.Count > 0))
                {
                    var crianca = repInscricoes.ObterInscricaoPeloId(dtoCrianca.Id.Value);
                    appApresentacaoSarau
                        .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(crianca, dtoCrianca.Sarais);
                }

                Contexto.ServicoEmail.EnviarEmailInscricaoRegistrada();
            });
        }

        public DTOSarau ObterSarau(int idEvento, string codigo)
        {
            DTOSarau dto = null;
            ExecutarSeguramente(() =>
            {
                var idSarau = new AppInscOnLineCodigoSarau().ExtrarId(codigo);
                var sarau = Contexto.RepositorioApresentacoesSarau.ObterPorId(idEvento, idSarau);
                dto = sarau?.Converter();
            });
            return dto;
        }

        public DTOCrianca ObterInscricaoInfantil(int idEvento, string codigo)
        {
            DTOCrianca dto = null;
            ExecutarSeguramente(() =>
            {
                var idInscricao = new AppInscOnLineCodigoInfantil().ExtrarId(codigo);
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(idInscricao);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoParticipante)
                        throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "Inscrição informada não é de uma criança.");
                    else if (inscricao.Evento.Id != idEvento)
                        throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "Essa inscrição não é do evento escolhido.");
                    else
                        dto = ((InscricaoInfantil)inscricao).Converter();
                }
            });
            return dto;
        }
    }
}
