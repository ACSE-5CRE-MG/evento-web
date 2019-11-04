using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao.ConversoresDTO
{
    public static class ConversorAtividades
    {
        public static DTOSalaEstudo Converter(this SalaEstudo sala)
        {
            return new DTOSalaEstudo
            {
                DeveSerParNumeroTotalParticipantes = sala.DeveSerParNumeroTotalParticipantes,
                Id = sala.Id,
                IdadeMaxima = sala.FaixaEtaria?.IdadeMax,
                IdadeMinima = sala.FaixaEtaria?.IdadeMin,
                Nome = sala.Nome
            };
        }
        public static DTOOficina Converter(this Oficina oficina)
        {
            return new DTOOficina
            {
                Id = oficina.Id,
                Nome = oficina.Nome,
                DeveSerParNumeroTotalParticipantes = oficina.DeveSerParNumeroTotalParticipantes,
                NumeroTotalParticipantes = oficina.NumeroTotalParticipantes
            };
        }
        public static DTODepartamento Converter(this Departamento departamento)
        {
            return new DTODepartamento
            {
                Id = departamento.Id,
                Nome = departamento.Nome
            };
        }

        public static DTOSarau Converter(this ApresentacaoSarau sarau)
        {
            return new DTOSarau
            {
                DuracaoMin = sarau.DuracaoMin,
                Id = sarau.Id,
                Participantes = sarau.Inscritos.Select(y => y.ConverterSimplificada()).ToList(),
                Tipo = sarau.Tipo
            };
        }
    }
}
  
