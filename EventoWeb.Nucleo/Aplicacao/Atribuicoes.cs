using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public static class AtribuicaoInscricao
    {
        public static void AtribuirDados(this InscricaoParticipante inscParticipante, DTOInscricaoAtualizacao dtoInscricao)
        {
            inscParticipante.InstituicoesEspiritasFrequenta = dtoInscricao.CentroEspirita;
            inscParticipante.NomeCracha = dtoInscricao.NomeCracha;
            inscParticipante.NomeResponsavelCentro = dtoInscricao.NomeResponsavelCentro;
            inscParticipante.NomeResponsavelLegal = dtoInscricao.NomeResponsavelLegal;
            inscParticipante.Observacoes = dtoInscricao.Observacoes;
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

            inscParticipante.Pessoa.AtribuirDados(dtoInscricao.DadosPessoais);
        }

        public static void AtribuirDados(this Pessoa pessoa, DTOInscricaoDadosPessoais dadosPessoais)
        {
            pessoa.AlergiaAlimentos = dadosPessoais.AlimentosAlergia;
            pessoa.Celular = dadosPessoais.Celular;
            pessoa.DataNascimento = dadosPessoais.DataNascimento;
            pessoa.EhDiabetico = dadosPessoais.EhDiabetico;
            pessoa.EhVegetariano = dadosPessoais.EhVegetariano;
            pessoa.Email = dadosPessoais.Email;
            pessoa.Endereco.Cidade = dadosPessoais.Cidade;
            pessoa.Endereco.UF = dadosPessoais.Uf;
            pessoa.MedicamentosUsados = dadosPessoais.MedicamentosUsa;
            pessoa.Nome = dadosPessoais.Nome;
            pessoa.Sexo = dadosPessoais.Sexo;
            pessoa.TelefoneFixo = dadosPessoais.TelefoneFixo;
            pessoa.TiposCarneNaoCome = dadosPessoais.CarnesNaoCome;
            pessoa.UsaAdocanteDiariamente = dadosPessoais.UsaAdocanteDiariamente;
        }

        public static void AtribuirAtividadeOficina(this InscricaoParticipante inscParticipante, DTOInscricaoOficina dto, AOficinas repOficinas)
        {
            if (dto != null)
            {
                if (dto.Coordenador != null)
                    inscParticipante.AdicionarAtividade(
                        new AtividadeInscricaoOficinasCoordenacao(
                            inscParticipante,
                            repOficinas.ObterPorId(dto.Coordenador.Id)));
                else if (dto.EscolhidasParticipante != null)
                {
                    var oficinas = repOficinas.ListarTodasPorEvento(inscParticipante.Evento.Id);
                    var escolhas = new OficinasEscolhidas(inscParticipante.Evento);
                    foreach (var dtoOficina in dto.EscolhidasParticipante)
                    {
                        if (escolhas.Oficinas.Count() == 0)
                            escolhas.DefinirPrimeiraPosicao(oficinas.FirstOrDefault(x => x.Id == dtoOficina.Id));
                        else
                            escolhas.DefinirProximaPosicao(oficinas.FirstOrDefault(x => x.Id == dtoOficina.Id));
                    }

                    var gestaoOficinas = new GestaoOficinasEscolhidas(
                        repOficinas,
                        escolhas);

                    inscParticipante.AdicionarAtividade(
                        new AtividadeInscricaoOficinas(
                            inscParticipante,
                            gestaoOficinas));
                }
                else
                    throw new ExcecaoAplicacao("AtribuicaoInscricao", "As informações para as oficinas estão incompletas");
            }
        }

        public static void AtribuirAtividadeSalaEstudo(this InscricaoParticipante inscParticipante, DTOInscricaoSalaEstudo dto, ASalasEstudo repSalas)
        {
            if (dto != null)
            {
                if (dto.Coordenador != null)
                    inscParticipante.AdicionarAtividade(
                        new AtividadeInscricaoSalaEstudoCoordenacao(
                            inscParticipante,
                            repSalas.ObterPorId(dto.Coordenador.Id)));
                else if (dto.EscolhidasParticipante != null)
                {
                    var salas = repSalas.ListarTodasPorEvento(inscParticipante.Evento.Id);
                    var escolhas = new SalasEstudoEscolhidas(inscParticipante.Evento);
                    foreach (var dtoOficina in dto.EscolhidasParticipante)
                    {
                        if (escolhas.Salas.Count() == 0)
                            escolhas.DefinirPrimeiraPosicao(salas.FirstOrDefault(x => x.Id == dtoOficina.Id));
                        else
                            escolhas.DefinirProximaPosicao(salas.FirstOrDefault(x => x.Id == dtoOficina.Id));
                    }

                    var gestaoOficinas = new GestaoSalasEstudoEscolhidas(
                        repSalas,
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
        }

        public static void AtribuirAtividadeDepartamento(this InscricaoParticipante inscParticipante, DTOInscricaoDepartamento dto, ADepartamentos repDepartamento)
        {
            if (dto != null)
            {
                Departamento departamento = null;
                bool ehCoordenador = false;
                if (dto.Coordenador != null)
                {
                    departamento = repDepartamento.ObterPorId(dto.Coordenador.Id);
                    ehCoordenador = true;
                }
                else if (dto.Participante != null)
                    departamento = repDepartamento.ObterPorId(dto.Participante.Id);

                if (departamento == null)
                    throw new ExcecaoAplicacao("AtribuicaoInscricao", "As informações para departamento estão incompletas");

                inscParticipante.AdicionarAtividade(
                    new AtividadeInscricaoDepartamento(inscParticipante, departamento) { EhCoordenacao = ehCoordenador });
            }
        }

        public static void AtribuirDados(this InscricaoInfantil inscricao, DTOCrianca dto)
        {
            inscricao.Pessoa.AlergiaAlimentos = dto.AlimentosAlergia;
            inscricao.Pessoa.DataNascimento = dto.DataNascimento;
            inscricao.Pessoa.EhDiabetico = dto.EhDiabetico;
            inscricao.Pessoa.EhVegetariano = dto.EhVegetariano;
            inscricao.Pessoa.Email = dto.Email;
            inscricao.Pessoa.Endereco.Cidade = dto.Cidade;
            inscricao.Pessoa.Endereco.UF = dto.Uf;
            inscricao.Pessoa.MedicamentosUsados = dto.MedicamentosUsa;
            inscricao.Pessoa.Nome = dto.Nome;
            inscricao.Pessoa.Sexo = dto.Sexo;
            inscricao.Pessoa.TiposCarneNaoCome = dto.CarnesNaoCome;
            inscricao.Pessoa.UsaAdocanteDiariamente = dto.UsaAdocanteDiariamente;
            inscricao.NomeCracha = dto.NomeCracha;
            inscricao.PrimeiroEncontro = dto.PrimeiroEncontro;
        }
    }
}
