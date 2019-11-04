using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao.ConversoresDTO
{
    public static class ConversorInscricao
    {
        public static DTOInscricaoCompleta Converter(this InscricaoParticipante inscricao)
        {
            var dto = new DTOInscricaoCompleta
            {
                CentroEspirita = inscricao.InstituicoesEspiritasFrequenta,
                DadosPessoais = new DTOInscricaoDadosPessoais
                {
                    AlimentosAlergia = inscricao.Pessoa.AlergiaAlimentos,
                    CarnesNaoCome = inscricao.Pessoa.TiposCarneNaoCome,
                    Cidade = inscricao.Pessoa.Endereco.Cidade,
                    DataNascimento = inscricao.Pessoa.DataNascimento,
                    EhDiabetico = inscricao.Pessoa.EhDiabetico,
                    EhVegetariano = inscricao.Pessoa.EhVegetariano,
                    Email = inscricao.Pessoa.Email,
                    MedicamentosUsa = inscricao.Pessoa.MedicamentosUsados,
                    Nome = inscricao.Pessoa.Nome,
                    Sexo = inscricao.Pessoa.Sexo,
                    Uf = inscricao.Pessoa.Endereco.UF,
                    UsaAdocanteDiariamente = inscricao.Pessoa.UsaAdocanteDiariamente,
                    Celular = inscricao.Pessoa.Celular,
                    TelefoneFixo = inscricao.Pessoa.TelefoneFixo                    
                },
                Evento = inscricao.Evento.Converter(),
                Id = inscricao.Id,
                NomeCracha = inscricao.NomeCracha,
                NomeResponsavelCentro = inscricao.NomeResponsavelCentro,
                NomeResponsavelLegal = inscricao.NomeResponsavelLegal,
                Observacoes = inscricao.Observacoes,
                PrimeiroEncontro = inscricao.PrimeiroEncontro,
                TelefoneResponsavelCentro = inscricao.TelefoneResponsavelCentro,
                TelefoneResponsavelLegal = inscricao.TelefoneResponsavelLegal,
                TempoEspirita = inscricao.TempoEspirita,
                TipoInscricao = inscricao.Tipo,
                Situacao = inscricao.Situacao
            };

            dto.Departamento = ((AtividadeInscricaoDepartamento)inscricao.Atividades.FirstOrDefault(x => x is AtividadeInscricaoDepartamento))?.Converter();

            dto.Oficina = ((AtividadeInscricaoOficinas)inscricao.Atividades.FirstOrDefault(x => x is AtividadeInscricaoOficinas))?.Converter();
            if (dto.Oficina == null)
                dto.Oficina = ((AtividadeInscricaoOficinasCoordenacao)inscricao.Atividades.FirstOrDefault(x => x is AtividadeInscricaoOficinasCoordenacao))?.Converter();

            dto.SalasEstudo = ((AtividadeInscricaoSalaEstudo)inscricao.Atividades.FirstOrDefault(x => x is AtividadeInscricaoSalaEstudo))?.Converter();
            if (dto.SalasEstudo == null)
                dto.SalasEstudo = ((AtividadeInscricaoSalaEstudoOrdemEscolha)inscricao.Atividades.FirstOrDefault(x => x is AtividadeInscricaoSalaEstudoOrdemEscolha))?.Converter();
            if (dto.SalasEstudo == null)
                dto.SalasEstudo = ((AtividadeInscricaoSalaEstudoCoordenacao)inscricao.Atividades.FirstOrDefault(x => x is AtividadeInscricaoSalaEstudoCoordenacao))?.Converter();

            if (inscricao.Pagamento != null)
            {
                dto.Pagamento = new DTOPagamento
                {
                    ComprovantesBase64 = inscricao.Pagamento.Comprovantes?.Select(x => Convert.ToBase64String(x.Arquivo)).ToList(),
                    Forma = inscricao.Pagamento.Forma,
                    Observacao = inscricao.Pagamento.Observacao
                };
            }

            return dto;
        }

        public static DTOInscricaoSimplificada ConverterSimplificada(this Inscricao inscricao)
        {
            return new DTOInscricaoSimplificada
            {
                Id = inscricao.Id,
                IdEvento = inscricao.Evento.Id,
                Nome = inscricao.Pessoa.Nome
            };
        }

        public static DTOCrianca Converter(this InscricaoInfantil inscricao)
        {
            var crianca = new DTOCrianca
            {
                AlimentosAlergia = inscricao.Pessoa.AlergiaAlimentos,
                CarnesNaoCome = inscricao.Pessoa.TiposCarneNaoCome,
                DataNascimento = inscricao.Pessoa.DataNascimento,
                EhDiabetico = inscricao.Pessoa.EhDiabetico,
                EhVegetariano = inscricao.Pessoa.EhVegetariano,
                MedicamentosUsa = inscricao.Pessoa.MedicamentosUsados,
                Nome = inscricao.Pessoa.Nome,
                Sexo = inscricao.Pessoa.Sexo,
                UsaAdocanteDiariamente = inscricao.Pessoa.UsaAdocanteDiariamente,
                Id = inscricao.Id,
                Responsaveis = new List<DTOInscricaoSimplificada>() { inscricao.InscricaoResponsavel1.ConverterSimplificada() }
            };

            if (inscricao.InscricaoResponsavel2 != null)
                crianca.Responsaveis.Add(inscricao.InscricaoResponsavel2.ConverterSimplificada());

            return crianca;
        }

        public static DTOInscricaoSalaEstudo Converter(this AtividadeInscricaoSalaEstudo atividade)
        {
            return new DTOInscricaoSalaEstudo();
        }
        public static DTOInscricaoSalaEstudo Converter(this AtividadeInscricaoSalaEstudoCoordenacao atividade)
        {
            return new DTOInscricaoSalaEstudo
            {
                Coordenador = atividade.SalaEscolhida.Converter(),
                EscolhidasParticipante = null
            };
        }
        public static DTOInscricaoSalaEstudo Converter(this AtividadeInscricaoSalaEstudoOrdemEscolha atividade)
        {
            return new DTOInscricaoSalaEstudo
            {
                Coordenador = null,
                EscolhidasParticipante = atividade.Salas.Select(x => x.Converter()).ToList()
            };
        }
        public static DTOInscricaoOficina Converter(this AtividadeInscricaoOficinas atividade)
        {
            return new DTOInscricaoOficina
            {
                Coordenador = null,
                EscolhidasParticipante = atividade.Oficinas.Select(x => x.Converter()).ToList()
            };
        }
        public static DTOInscricaoOficina Converter(this AtividadeInscricaoOficinasCoordenacao atividade)
        {
            return new DTOInscricaoOficina
            {
                Coordenador = atividade.OficinaEscolhida.Converter(),
                EscolhidasParticipante = null
            };
        }
        public static DTOInscricaoDepartamento Converter(this AtividadeInscricaoDepartamento atividade)
        {
            var dtoDep = atividade.DepartamentoEscolhido.Converter();

            return new DTOInscricaoDepartamento
            {
                Coordenador = atividade.EhCoordenacao ? dtoDep : null,
                Participante = !atividade.EhCoordenacao ? dtoDep : null,
            };
        }
    }
}
