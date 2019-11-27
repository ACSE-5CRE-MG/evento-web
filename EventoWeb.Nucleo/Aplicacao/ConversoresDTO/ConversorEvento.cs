using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao.ConversoresDTO
{
    public static class ConversorEvento
    {
        public static DTOEventoCompleto ConverterApenasEvento(this Evento evento)
        {
            var dto = new DTOEventoCompleto();
            dto.Converter(evento);

            return dto;
        }

        public static DTOEventoCompletoInscricao ConverterParaInsOnLine(this Evento evento)
        {
            var dto = new DTOEventoCompletoInscricao();
            dto.Converter(evento);

            return dto;
        }

        private static DTOEventoCompleto Converter(this DTOEventoCompleto dto, Evento evento)
        {
            dto.Id = evento.Id;
            dto.PeriodoInscricao = evento.PeriodoInscricaoOnLine;
            dto.PeriodoRealizacao = evento.PeriodoRealizacaoEvento;
            dto.DataRegistro = evento.DataRegistro;
            dto.Logotipo = evento.Logotipo != null ? Convert.ToBase64String(evento.Logotipo.Arquivo) : null;
            dto.Nome = evento.Nome;
            dto.TemDepartamentalizacao = evento.TemDepartamentalizacao;
            dto.TemDormitorios = evento.TemDormitorios;
            dto.TemOficinas = evento.TemOficinas;
            dto.ConfiguracaoSalaEstudo = evento.ConfiguracaoSalaEstudo;
            dto.ConfiguracaoEvangelizacao = evento.ConfiguracaoEvangelizacao;
            dto.ConfiguracaoTempoSarauMin = evento.ConfiguracaoTempoSarauMin;
            dto.IdadeMinima = evento.IdadeMinimaInscricaoAdulto;
            dto.ValorInscricaoAdulto = evento.ValorInscricaoAdulto;
            dto.ValorInscricaoCrianca = evento.ValorInscricaoCrianca;

            return dto;
        }

        public static DTOEventoMinimo ConverterMinimo(this Evento evento)
        {
            return new DTOEventoMinimo()
            {
                Id = evento.Id,
                PeriodoInscricao = evento.PeriodoInscricaoOnLine,
                Nome = evento.Nome,
                Logotipo = evento.Logotipo != null ? Convert.ToBase64String(evento.Logotipo.Arquivo) : null,
                IdadeMinima = evento.IdadeMinimaInscricaoAdulto,
                PeriodoRealizacao = evento.PeriodoRealizacaoEvento
            };
        }
    }
}
