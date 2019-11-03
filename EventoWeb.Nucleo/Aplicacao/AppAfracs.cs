﻿using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppAfracs : AppBase
    {
        public AppAfracs(IContexto contexto) : base(contexto)
        {
        }

        public IList<DTOOficina> ObterTodos(int idEvento)
        {
            var lista = new List<DTOOficina>();
            ExecutarSeguramente(() =>
            {
                var afracs = Contexto.RepositorioOficinas.ListarTodasPorEvento(idEvento);
                if (afracs.Count > 0)
                    lista.AddRange(afracs.Select(x => Converter(x)));
            });

            return lista;
        }

        public DTOOficina ObterPorId(int id)
        {
            DTOOficina dto = null;
            ExecutarSeguramente(() =>
            {
                var afrac = Contexto.RepositorioOficinas.ObterPorId(id);

                if (afrac != null)
                    dto = Converter(afrac);
            });

            return dto;
        }

        private DTOOficina Converter(Oficina afrac)
        {
            return new DTOOficina
            {
                Id = afrac.Id,
                Nome = afrac.Nome,
                DeveSerParNumeroTotalParticipantes = afrac.DeveSerParNumeroTotalParticipantes,
                NumeroTotalParticipantes = afrac.NumeroTotalParticipantes                
            };
        }

        public DTOId Incluir(int idEvento, DTOOficina dto)
        {
            DTOId retorno = new DTOId();

            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
                var afrac = new Oficina(evento, dto.Nome)
                {
                    DeveSerParNumeroTotalParticipantes = dto.DeveSerParNumeroTotalParticipantes,
                    NumeroTotalParticipantes = dto.NumeroTotalParticipantes
                };

                Contexto.RepositorioOficinas.Incluir(afrac);
                retorno.Id = afrac.Id;
            });

            return retorno;
        }

        public void Atualizar(int id, DTOOficina dto)
        {
            ExecutarSeguramente(() =>
            {
                var afrac = ObterAfracOuExcecaoSeNaoEncontrar(id);
                afrac.Nome = dto.Nome;
                afrac.DeveSerParNumeroTotalParticipantes = dto.DeveSerParNumeroTotalParticipantes;
                afrac.NumeroTotalParticipantes = dto.NumeroTotalParticipantes;

                Contexto.RepositorioOficinas.Atualizar(afrac);
            });
        }

        public void Excluir(int id)
        {
            ExecutarSeguramente(() =>
            {
                var sala = ObterAfracOuExcecaoSeNaoEncontrar(id);

                Contexto.RepositorioOficinas.Excluir(sala);
            });
        }

        private Oficina ObterAfracOuExcecaoSeNaoEncontrar(int id)
        {
            var afrac = Contexto.RepositorioOficinas.ObterPorId(id);

            if (afrac != null)
                return afrac;
            else
                throw new ExcecaoAplicacao("AppAfracs", "Não foi encontrado nenhuma afrac com o id informado.");
        }
    }
}
