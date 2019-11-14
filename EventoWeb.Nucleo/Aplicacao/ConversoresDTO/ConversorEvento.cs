using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao.ConversoresDTO
{
    public static class ConversorEvento
    {
        public static DTOEventoCompleto Converter(this Evento evento)
        {
            return new DTOEventoCompleto
            {
                Id = evento.Id,
                PeriodoInscricao = evento.PeriodoInscricaoOnLine,
                PeriodoRealizacao = evento.PeriodoRealizacaoEvento,
                DataRegistro = evento.DataRegistro,
                Logotipo = evento.Logotipo != null ? Convert.ToBase64String(evento.Logotipo.Arquivo) : null,
                Nome = evento.Nome,
                TemDepartamentalizacao = evento.TemDepartamentalizacao,
                TemDormitorios = evento.TemDormitorios,
                TemOficinas = evento.TemOficinas,
                ConfiguracaoSalaEstudo = evento.ConfiguracaoSalaEstudo,
                ConfiguracaoEvangelizacao = evento.ConfiguracaoEvangelizacao,
                ConfiguracaoSarau = evento.ConfiguracaoSarau,
                IdadeMinimaInscricaoAdulto = evento.IdadeMinimaInscricaoAdulto,
                PodeAlterar = true
            };
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
