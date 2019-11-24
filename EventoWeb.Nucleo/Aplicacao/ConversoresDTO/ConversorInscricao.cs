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
            var dto = new DTOInscricaoCompleta();
            dto.Converter(inscricao);
            return dto;
        }

        public static DTOInscricaoCompletaCodigo ConverterComCodigo(this InscricaoParticipante inscricao)
        {
            var dto = new DTOInscricaoCompletaCodigo();
            dto.Converter(inscricao);
            return dto;
        }

        private static ADTOInscricaoCompleta<TSarau, TCrianca> Converter<TSarau, TCrianca>(this ADTOInscricaoCompleta<TSarau, TCrianca> dto, 
            InscricaoParticipante inscricao)
            where TSarau: DTOSarau
            where TCrianca: DTOCrianca
        {
            dto.CentroEspirita = inscricao.InstituicoesEspiritasFrequenta;
            dto.DadosPessoais = new DTOInscricaoDadosPessoais
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
                TelefoneFixo = inscricao.Pessoa.TelefoneFixo,
            };
            dto.Evento = inscricao.Evento.ConverterParaInsOnLine();
            dto.Id = inscricao.Id;
            dto.NomeCracha = inscricao.NomeCracha;
            dto.NomeResponsavelCentro = inscricao.NomeResponsavelCentro;
            dto.NomeResponsavelLegal = inscricao.NomeResponsavelLegal;
            dto.Observacoes = inscricao.Observacoes;
            dto.PrimeiroEncontro = inscricao.PrimeiroEncontro;
            dto.TelefoneResponsavelCentro = inscricao.TelefoneResponsavelCentro;
            dto.TelefoneResponsavelLegal = inscricao.TelefoneResponsavelLegal;
            dto.TempoEspirita = inscricao.TempoEspirita;
            dto.TipoInscricao = inscricao.Tipo;
            dto.Situacao = inscricao.Situacao;
            
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
                    ComprovantesBase64 = inscricao.Pagamento.Comprovantes?.Select(x => Convert.ToBase64String(x.ArquivoComprovante.Arquivo)).ToList(),
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
            var crianca = new DTOCrianca();
            crianca.Converter(inscricao);

            return crianca;
        }

        public static DTOCriancaCodigo ConverterComCodigo(this InscricaoInfantil inscricao)
        {
            var crianca = new DTOCriancaCodigo();
            crianca.Converter(inscricao);

            return crianca;
        }

        private static DTOCrianca Converter(this DTOCrianca dto, InscricaoInfantil inscricao)
        {
            dto.AlimentosAlergia = inscricao.Pessoa.AlergiaAlimentos;
            dto.CarnesNaoCome = inscricao.Pessoa.TiposCarneNaoCome;
            dto.DataNascimento = inscricao.Pessoa.DataNascimento;
            dto.EhDiabetico = inscricao.Pessoa.EhDiabetico;
            dto.EhVegetariano = inscricao.Pessoa.EhVegetariano;
            dto.MedicamentosUsa = inscricao.Pessoa.MedicamentosUsados;
            dto.Nome = inscricao.Pessoa.Nome;
            dto.Sexo = inscricao.Pessoa.Sexo;
            dto.UsaAdocanteDiariamente = inscricao.Pessoa.UsaAdocanteDiariamente;
            dto.Id = inscricao.Id;
            dto.Responsaveis = new List<DTOInscricaoSimplificada>() { inscricao.InscricaoResponsavel1.ConverterSimplificada() };
            dto.Cidade = inscricao.Pessoa.Endereco.Cidade;
            dto.Email = inscricao.Pessoa.Email;
            dto.NomeCracha = inscricao.NomeCracha;
            dto.PrimeiroEncontro = inscricao.PrimeiroEncontro;
            dto.Uf = inscricao.Pessoa.Endereco.UF;
            

            if (inscricao.InscricaoResponsavel2 != null)
                dto.Responsaveis.Add(inscricao.InscricaoResponsavel2.ConverterSimplificada());

            return dto;
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
