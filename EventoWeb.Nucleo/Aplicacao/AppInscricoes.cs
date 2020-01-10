using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppInscricoes : AppBase
    {
        private readonly AppEmailMsgPadrao m_AppEmail;

        public AppInscricoes(IContexto contexto, AppEmailMsgPadrao appEmail) : 
            base(contexto)
        {
            m_AppEmail = appEmail;
        }

        public IEnumerable<DTOBasicoInscricao> ListarTodas(int idEvento, EnumSituacaoInscricao situacao)
        {
            var lista = new List<DTOBasicoInscricao>();
            ExecutarSeguramente(() =>
            {
                var inscricoes = Contexto.RepositorioInscricoes.ListarTodasPorEventoESituacao(idEvento, situacao);
                if (inscricoes.Count > 0)
                    lista.AddRange(inscricoes.Select(x => x.ConverterBasico()));
            });

            return lista;
        }

        public void Excluir(int idEvento, int idInscricao)
        {
            ExecutarSeguramente(() =>
            {
                var repositorio = Contexto.RepositorioInscricoes;
                var inscricao = repositorio.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);

                if (inscricao != null)
                {
                    if (inscricao is InscricaoParticipante)
                    {
                        var inscricoesInfantis = repositorio.ListarInscricoesInfantisDoResponsavel(inscricao);
                        foreach(var crianca in inscricoesInfantis)
                        {
                            if (crianca.InscricaoResponsavel1 == inscricao)
                            {
                                if (crianca.InscricaoResponsavel2 == null)
                                    repositorio.Excluir(crianca);
                                else
                                {
                                    crianca.AtribuirResponsaveis(crianca.InscricaoResponsavel2, null);
                                    repositorio.Atualizar(crianca);
                                }
                            }
                            else
                            {
                                crianca.AtribuirResponsaveis(crianca.InscricaoResponsavel1, null);
                                repositorio.Atualizar(crianca);
                            }
                        }
                    }

                    repositorio.Excluir(inscricao);
                }
                else
                    throw new ExcecaoAplicacao("AppInscricoes", "Inscrição não encontrada para este evento");
            });
        }

        public void Rejeitar(int idEvento, int idInscricao)
        {
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoInfantil)
                        throw new ExcecaoAplicacao("AppInscricoes", "A inscrição não pode ser infantil");
                    var participante = (InscricaoParticipante)inscricao;
                    participante.Rejeitar();

                    m_AppEmail.EnviarInscricaoRejeitada((InscricaoParticipante)inscricao);
                }
            });
        }

        public void Aceitar(int idEvento, int idInscricao, DTOInscricaoAtualizacao atualizacao)
        {
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoInfantil)
                        throw new ExcecaoAplicacao("AppInscricoes", "A inscrição não pode ser infantil");
                    var participante = (InscricaoParticipante)inscricao;
                    AtualizarInscricao(participante, atualizacao);
                    participante.Aceitar();

                    m_AppEmail.EnviarInscricaoAceita((InscricaoParticipante)inscricao);
                }
            });
        }

        public void CompletarEAceitar(int idEvento, int idInscricao, DTOInscricaoAtualizacao atualizacao)
        {
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoInfantil)
                        throw new ExcecaoAplicacao("AppInscricoes", "A inscrição não pode ser infantil");
                    var participante = (InscricaoParticipante)inscricao;
                    AtualizarInscricao(participante, atualizacao);
                    participante.TornarPendente();
                    participante.Aceitar();

                    m_AppEmail.EnviarInscricaoAceita((InscricaoParticipante)inscricao);
                }
            });
        }

        public void Atualizar(int idEvento, int idInscricao, DTOInscricaoAtualizacao atualizacao)
        {
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoInfantil)
                        throw new ExcecaoAplicacao("AppInscricoes", "A inscrição não pode ser infantil");
                    var participante = (InscricaoParticipante)inscricao;
                    AtualizarInscricao(participante, atualizacao);
                }
            });
        }

        private void AtualizarInscricao(InscricaoParticipante inscParticipante, DTOInscricaoAtualizacao dtoInscricao)
        {
            var repInscricoes = Contexto.RepositorioInscricoes;

            inscParticipante.AtribuirDados(dtoInscricao);

            inscParticipante.RemoverTodasAtividade();
            inscParticipante.AtribuirAtividadeOficina(dtoInscricao.Oficina, Contexto.RepositorioOficinas);
            inscParticipante.AtribuirAtividadeSalaEstudo(dtoInscricao.SalasEstudo, Contexto.RepositorioSalasEstudo);
            inscParticipante.AtribuirAtividadeDepartamento(dtoInscricao.Departamento, Contexto.RepositorioDepartamentos);

            repInscricoes.Atualizar(inscParticipante);

            new AppInscricaoInfantil(Contexto)
                .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(inscParticipante, dtoInscricao.Criancas);

            var appApresentacaoSarau = new AppApresentacaoSarau(Contexto);
            appApresentacaoSarau
                .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(inscParticipante, dtoInscricao.Sarais);

            foreach (var dtoCrianca in dtoInscricao.Criancas.Where(x => x.Sarais?.Count > 0))
            {
                var crianca = repInscricoes.ObterInscricaoPeloIdEventoEInscricao(inscParticipante.Evento.Id, dtoCrianca.Id.Value);
                appApresentacaoSarau
                    .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(crianca, dtoCrianca.Sarais);
            }
        }

        public DTOInscricaoCompleta Obter(int idEvento, int idInscricao)
        {
            DTOInscricaoCompleta dto = null;
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloIdEventoEInscricao(idEvento, idInscricao);
                if (inscricao != null)
                {
                    if (inscricao is InscricaoInfantil)
                        throw new ExcecaoAplicacao("AppInscricoes", "A inscrição não pode ser infantil");

                    var inscParticipante = (InscricaoParticipante)inscricao;
                    dto = inscParticipante.Converter();

                    dto.Evento.Departamentos = Contexto.RepositorioDepartamentos.ListarTodosPorEvento(inscParticipante.Evento.Id)
                        .Select(x => x.Converter())
                        .ToList();
                    dto.Evento.Oficinas = Contexto.RepositorioOficinas.ListarTodasPorEvento(inscParticipante.Evento.Id)
                        .Select(x => x.Converter())
                        .ToList();
                    dto.Evento.SalasEstudo = Contexto.RepositorioSalasEstudo.ListarTodasPorEvento(inscParticipante.Evento.Id)
                        .Select(x => x.Converter())
                        .ToList();

                    dto.Sarais = Contexto.RepositorioApresentacoesSarau.ListarPorInscricao(inscParticipante.Id)
                        .Select(x => x.Converter()).ToList();

                    dto.Criancas = Contexto.RepositorioInscricoes.ListarInscricoesInfantisDoResponsavel(inscParticipante)
                        .Select(x => x.Converter())
                        .ToList();
                }
            });
            return dto;
        }
    }
}
