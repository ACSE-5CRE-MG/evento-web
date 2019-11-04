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
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(id) ??
                    throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "Inscrição não encontrada");

                if (inscricao is InscricaoInfantil)
                    throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "Não é possível atualizar inscrição infantil");

                var inscParticipante = (InscricaoParticipante)inscricao;

                inscParticipante.InstituicoesEspiritasFrequenta = dtoInscricao.CentroEspirita;
                inscParticipante.NomeCracha = dtoInscricao.NomeCracha;
                inscParticipante.NomeResponsavelCentro = dtoInscricao.NomeResponsavelCentro;
                inscParticipante.NomeResponsavelLegal = dtoInscricao.NomeResponsavelLegal;
                inscParticipante.Observacoes = dtoInscricao.Observacoes;
                inscParticipante.Pessoa.AlergiaAlimentos = dtoInscricao.DadosPessoais.AlimentosAlergia;
                inscParticipante.Pessoa.Celular = dtoInscricao.DadosPessoais.Celular;
                inscParticipante.Pessoa.DataNascimento = dtoInscricao.DadosPessoais.DataNascimento;
                inscParticipante.Pessoa.EhDiabetico = dtoInscricao.DadosPessoais.EhDiabetico;
                inscParticipante.Pessoa.EhVegetariano = dtoInscricao.DadosPessoais.EhVegetariano;
                inscParticipante.Pessoa.Email = dtoInscricao.DadosPessoais.Email;
                inscParticipante.Pessoa.Endereco.Cidade = dtoInscricao.DadosPessoais.Cidade;
                inscParticipante.Pessoa.Endereco.UF = dtoInscricao.DadosPessoais.Uf;
                inscParticipante.Pessoa.MedicamentosUsados = dtoInscricao.DadosPessoais.MedicamentosUsa;
                inscParticipante.Pessoa.Nome = dtoInscricao.DadosPessoais.Nome;
                inscParticipante.Pessoa.Sexo = dtoInscricao.DadosPessoais.Sexo;
                inscParticipante.Pessoa.TelefoneFixo = dtoInscricao.DadosPessoais.TelefoneFixo;
                inscParticipante.Pessoa.TiposCarneNaoCome = dtoInscricao.DadosPessoais.CarnesNaoCome;
                inscParticipante.Pessoa.UsaAdocanteDiariamente = dtoInscricao.DadosPessoais.UsaAdocanteDiariamente;
                inscParticipante.PrimeiroEncontro = dtoInscricao.PrimeiroEncontro;
                inscParticipante.TelefoneResponsavelCentro = dtoInscricao.TelefoneResponsavelCentro;
                inscParticipante.TelefoneResponsavelLegal = dtoInscricao.TelefoneResponsavelLegal;
                inscParticipante.TempoEspirita = dtoInscricao.TempoEspirita;
                inscParticipante.Tipo = dtoInscricao.TipoInscricao;

                inscParticipante.Pagamento = new Pagamento(
                    dtoInscricao.Pagamento.Forma,
                    dtoInscricao.Pagamento.ComprovantesBase64
                        .Select(x => new ArquivoBinario(Convert.FromBase64String(x), EnumTipoArquivoBinario.ImagemJPEG))
                        .ToList(),
                    dtoInscricao.Pagamento.Observacao);

                inscParticipante.RemoverTodasAtividade();
                if (dtoInscricao.Oficina != null)
                {
                    if (dtoInscricao.Oficina.Coordenador != null)
                        inscParticipante.AdicionarAtividade(
                            new AtividadeInscricaoOficinasCoordenacao(
                                inscParticipante,
                                Contexto.RepositorioOficinas.ObterPorId(dtoInscricao.Oficina.Coordenador.Id)));
                    else if (dtoInscricao.Oficina.EscolhidasParticipante != null)
                    {
                        var oficinas = Contexto.RepositorioOficinas.ListarTodasPorEvento(inscParticipante.Evento.Id);
                        var escolhas = new OficinasEscolhidas(inscParticipante.Evento);
                        foreach (var dtoOficina in dtoInscricao.Oficina.EscolhidasParticipante)
                        {
                            if (escolhas.Oficinas.Count() == 0)
                                escolhas.DefinirPrimeiraPosicao(oficinas.FirstOrDefault(x => x.Id == dtoOficina.Id));
                            else
                                escolhas.DefinirProximaPosicao(oficinas.FirstOrDefault(x => x.Id == dtoOficina.Id));
                        }

                        var gestaoOficinas = new GestaoOficinasEscolhidas(
                            Contexto.RepositorioOficinas,
                            escolhas);

                        inscParticipante.AdicionarAtividade(
                            new AtividadeInscricaoOficinas(
                                inscParticipante,
                                gestaoOficinas));
                    }
                    else
                        throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "As informações para as oficinas estão incompletas");
                }

                if (dtoInscricao.SalasEstudo != null)
                {
                    if (dtoInscricao.SalasEstudo.Coordenador != null)
                        inscParticipante.AdicionarAtividade(
                            new AtividadeInscricaoSalaEstudoCoordenacao(
                                inscParticipante,
                                Contexto.RepositorioSalasEstudo.ObterPorId(dtoInscricao.SalasEstudo.Coordenador.Id)));
                    else if (dtoInscricao.Oficina.EscolhidasParticipante != null)
                    {
                        var salas = Contexto.RepositorioSalasEstudo.ListarTodasPorEvento(inscParticipante.Evento.Id);
                        var escolhas = new SalasEstudoEscolhidas(inscParticipante.Evento);
                        foreach (var dtoOficina in dtoInscricao.SalasEstudo.EscolhidasParticipante)
                        {
                            if (escolhas.Salas.Count() == 0)
                                escolhas.DefinirPrimeiraPosicao(salas.FirstOrDefault(x => x.Id == dtoOficina.Id));
                            else
                                escolhas.DefinirProximaPosicao(salas.FirstOrDefault(x => x.Id == dtoOficina.Id));
                        }

                        var gestaoOficinas = new GestaoSalasEstudoEscolhidas(
                            Contexto.RepositorioSalasEstudo,
                            escolhas);

                        inscParticipante.AdicionarAtividade(
                            new AtividadeInscricaoSalaEstudoOrdemEscolha(
                                inscParticipante,
                                gestaoOficinas));
                    }
                    else
                        inscParticipante.AdicionarAtividade(
                            new AtividadeInscricaoSalaEstudo(inscParticipante));
                }

                if (dtoInscricao.Departamento != null)
                {
                    Departamento departamento;
                    bool ehCoordenador = false;
                    if (dtoInscricao.Departamento.Coordenador != null)
                    {
                        departamento = Contexto.RepositorioDepartamentos.ObterPorId(dtoInscricao.Departamento.Coordenador.Id);
                        ehCoordenador = true;
                    }
                    else
                        departamento = Contexto.RepositorioDepartamentos.ObterPorId(dtoInscricao.Departamento.Participante.Id);

                    if (departamento == null)
                        throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "As informações para departamento estão incompletas");

                    inscParticipante.AdicionarAtividade(
                        new AtividadeInscricaoDepartamento(inscParticipante, departamento) { EhCoordenacao = ehCoordenador });
                }

                inscParticipante.TornarPendente();
                Contexto.RepositorioInscricoes.Atualizar(inscParticipante);

                // Atualizar/incluir inscrições infantis

                // Atualizar/Incluir Apresentação Sarau
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
