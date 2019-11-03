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
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(id) ?? 
                    throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "Inscrição não encontrada");

                if (inscricao is InscricaoInfantil)
                    throw new ExcecaoAplicacao("AppInscOnlineEventoManutencaoInscricoes", "A inscrição não pode ser infantil");

                var inscParticipante = (InscricaoParticipante)inscricao;
                
                dto = new DTOInscricaoCompleta
                {
                    CentroEspirita = inscParticipante.InstituicoesEspiritasFrequenta,
                    DadosPessoais = new DTOInscricaoDadosPessoais
                    {
                        AlimentosAlergia = inscParticipante.Pessoa.AlergiaAlimentos,
                        CarnesNaoCome = inscParticipante.Pessoa.TiposCarneNaoCome,
                        Cidade = inscParticipante.Pessoa.Endereco.Cidade,
                        DataNascimento = inscParticipante.Pessoa.DataNascimento,
                        EhDiabetico = inscParticipante.Pessoa.EhDiabetico,
                        EhVegetariano = inscParticipante.Pessoa.EhVegetariano,
                        Email = inscParticipante.Pessoa.Email,
                        MedicamentosUsa = inscParticipante.Pessoa.MedicamentosUsados,
                        Nome = inscParticipante.Pessoa.Nome,
                        Sexo = inscParticipante.Pessoa.Sexo,
                        Uf = inscParticipante.Pessoa.Endereco.UF,
                        UsaAdocanteDiariamente = inscParticipante.Pessoa.UsaAdocanteDiariamente
                    },
                    Evento = new DTOEventoCompleto
                    {
                        Id = inscParticipante.Evento.Id,
                        PeriodoInscricao = inscParticipante.Evento.PeriodoInscricaoOnLine,
                        PeriodoRealizacao = inscParticipante.Evento.PeriodoRealizacaoEvento,
                        DataRegistro = inscParticipante.Evento.DataRegistro,
                        Logotipo = Convert.ToBase64String(inscParticipante.Evento.Logotipo.Arquivo),
                        Nome = inscParticipante.Evento.Nome,
                        TemDepartamentalizacao = inscParticipante.Evento.TemDepartamentalizacao,
                        TemDormitorios = inscParticipante.Evento.TemDormitorios,
                        TemOficinas = inscParticipante.Evento.TemOficinas,
                        ConfiguracaoSalaEstudo = inscParticipante.Evento.ConfiguracaoSalaEstudo,
                        ConfiguracaoEvangelizacao = inscParticipante.Evento.ConfiguracaoEvangelizacao,
                        ConfiguracaoSarau = inscParticipante.Evento.ConfiguracaoSarau,
                        IdadeMinimaInscricaoAdulto = inscParticipante.Evento.IdadeMinimaInscricaoAdulto,
                        PodeAlterar = true
                    },
                    Id = inscParticipante.Id,
                    NomeCracha = inscParticipante.NomeCracha,
                    NomeResponsavelCentro = inscParticipante.NomeResponsavelCentro,
                    NomeResponsavelLegal = inscParticipante.NomeResponsavelLegal,
                    Observacoes = inscParticipante.Observacoes,
                    PrimeiroEncontro = inscParticipante.PrimeiroEncontro,
                    TelefoneResponsavelCentro = inscParticipante.TelefoneResponsavelCentro,
                    TelefoneResponsavelLegal = inscParticipante.TelefoneResponsavelLegal,
                    TempoEspirita = inscParticipante.TempoEspirita,
                    TipoInscricao = inscParticipante.Tipo,
                    Situacao = inscParticipante.Situacao
                };

                var departamento = (AtividadeInscricaoDepartamento)inscParticipante.Atividades.FirstOrDefault(x => x is AtividadeInscricaoDepartamento);
                if (departamento != null)
                {
                    var dtoDep = new DTODepartamento
                    {
                        Id = departamento.DepartamentoEscolhido.Id,
                        Nome = departamento.DepartamentoEscolhido.Nome
                    };

                    dto.Departamento = new DTOInscricaoDepartamento
                    {
                        Coordenador = departamento.EhCoordenacao ? dtoDep : null,
                        Participante = !departamento.EhCoordenacao ? dtoDep : null,
                    };
                }

                var oficinaPart = (AtividadeInscricaoOficinas)inscParticipante.Atividades.FirstOrDefault(x => x is AtividadeInscricaoOficinas);
                if (oficinaPart != null)
                {
                    dto.Oficina = new DTOInscricaoOficina
                    {
                        Coordenador = null,
                        EscolhidasParticipante = oficinaPart.Oficinas.Select(x => new DTOOficina
                        {
                            Id = x.Id,
                            Nome = x.Nome,
                            DeveSerParNumeroTotalParticipantes = x.DeveSerParNumeroTotalParticipantes,
                            NumeroTotalParticipantes = x.NumeroTotalParticipantes
                        })
                        .ToList()
                    };
                }
                var oficinaCoord = (AtividadeInscricaoOficinasCoordenacao)inscParticipante.Atividades.FirstOrDefault(x => x is AtividadeInscricaoOficinasCoordenacao);
                if (oficinaCoord != null)
                {
                    dto.Oficina = new DTOInscricaoOficina
                    {
                        Coordenador = new DTOOficina
                        {
                            Id = oficinaCoord.OficinaEscolhida.Id,
                            Nome = oficinaCoord.OficinaEscolhida.Nome,
                            DeveSerParNumeroTotalParticipantes = oficinaCoord.OficinaEscolhida.DeveSerParNumeroTotalParticipantes,
                            NumeroTotalParticipantes = oficinaCoord.OficinaEscolhida.NumeroTotalParticipantes
                        },
                        EscolhidasParticipante = null
                    };
                }

                var salaPartSemEscolha = (AtividadeInscricaoSalaEstudo)inscParticipante.Atividades.FirstOrDefault(x => x is AtividadeInscricaoSalaEstudo);
                if (salaPartSemEscolha != null)
                    dto.SalasEstudo = new DTOInscricaoSalaEstudo();
                var salaPartComEscolha = (AtividadeInscricaoSalaEstudoOrdemEscolha)inscParticipante.Atividades.FirstOrDefault(x => x is AtividadeInscricaoSalaEstudoOrdemEscolha);
                if (salaPartComEscolha != null)
                {
                    dto.SalasEstudo = new DTOInscricaoSalaEstudo
                    {
                        Coordenador = null,
                        EscolhidasParticipante = salaPartComEscolha.Salas.Select(x => new DTOSalaEstudo
                        {
                            DeveSerParNumeroTotalParticipantes = x.DeveSerParNumeroTotalParticipantes,
                            Id = x.Id,
                            IdadeMaxima = x.FaixaEtaria?.IdadeMax,
                            IdadeMinima = x.FaixaEtaria?.IdadeMin,
                            Nome = x.Nome
                        })
                        .ToList()
                    };
                }
                var salaCoord = (AtividadeInscricaoSalaEstudoCoordenacao)inscParticipante.Atividades.FirstOrDefault(x => x is AtividadeInscricaoSalaEstudoCoordenacao);
                if (salaCoord != null)
                {
                    dto.SalasEstudo = new DTOInscricaoSalaEstudo
                    {
                        Coordenador = new DTOSalaEstudo
                        {
                            DeveSerParNumeroTotalParticipantes = salaCoord.SalaEscolhida.DeveSerParNumeroTotalParticipantes,
                            Id = salaCoord.SalaEscolhida.Id,
                            IdadeMaxima = salaCoord.SalaEscolhida.FaixaEtaria?.IdadeMax,
                            IdadeMinima = salaCoord.SalaEscolhida.FaixaEtaria?.IdadeMin,
                            Nome = salaCoord.SalaEscolhida.Nome
                        },
                        EscolhidasParticipante = null
                    };
                }

                if (inscParticipante.Pagamento != null)
                {
                    dto.Pagamento = new DTOPagamento
                    {
                        ComprovantesBase64 = inscParticipante.Pagamento.Comprovantes?.Select(x=> Convert.ToBase64String(x.Arquivo)).ToList(),
                        Forma = inscParticipante.Pagamento.Forma,
                        Observacao = inscParticipante.Pagamento.Observacao
                    };
                }

                // Sarau

                // Crianças (Inscricoes Infantis)

            });
            return dto;
        }

        public void AtualizarInscricao(int id, DTOInscricaoAtualizacao dtoInscricao)
        {
            throw new NotImplementedException();
        }

        public DTOSarau ObterSarau(int idEvento, string codigo)
        {
            throw new NotImplementedException();
        }

        public DTOCrianca ObterInscricaoInfantil(int idEvento, string codigo)
        {
            throw new NotImplementedException();
        }
    }
}
