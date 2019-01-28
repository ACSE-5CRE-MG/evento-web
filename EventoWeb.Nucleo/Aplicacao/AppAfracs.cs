using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppAfracs : AppBase
    {
        public AppAfracs(IContexto contexto) : base(contexto)
        {
        }

        public IList<DTOAfrac> ObterTodos(int idEvento)
        {
            var lista = new List<DTOAfrac>();
            ExecutarSeguramente(() =>
            {
                var afracs = Contexto.RepositorioAfracs.ListarTodasPorEvento(idEvento);
                if (afracs.Count > 0)
                    lista.AddRange(afracs.Select(x => Converter(x)));
            });

            return lista;
        }

        public DTOAfrac ObterPorId(int id)
        {
            DTOAfrac dto = null;
            ExecutarSeguramente(() =>
            {
                var afrac = Contexto.RepositorioAfracs.ObterPorId(id);

                if (afrac != null)
                    dto = Converter(afrac);
            });

            return dto;
        }

        private DTOAfrac Converter(Afrac afrac)
        {
            return new DTOAfrac
            {
                Id = afrac.Id,
                Nome = afrac.Nome,
                DeveSerParNumeroTotalParticipantes = afrac.DeveSerParNumeroTotalParticipantes,
                NumeroTotalParticipantes = afrac.NumeroTotalParticipantes                
            };
        }

        public DTOId Incluir(int idEvento, DTOAfrac dto)
        {
            DTOId retorno = new DTOId();

            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
                var afrac = new Afrac(evento, dto.Nome)
                {
                    DeveSerParNumeroTotalParticipantes = dto.DeveSerParNumeroTotalParticipantes,
                    NumeroTotalParticipantes = dto.NumeroTotalParticipantes
                };

                Contexto.RepositorioAfracs.Incluir(afrac);
                retorno.Id = afrac.Id;
            });

            return retorno;
        }

        public void Atualizar(int id, DTOAfrac dto)
        {
            ExecutarSeguramente(() =>
            {
                var afrac = ObterAfracOuExcecaoSeNaoEncontrar(id);
                afrac.Nome = dto.Nome;
                afrac.DeveSerParNumeroTotalParticipantes = dto.DeveSerParNumeroTotalParticipantes;
                afrac.NumeroTotalParticipantes = dto.NumeroTotalParticipantes;

                Contexto.RepositorioAfracs.Atualizar(afrac);
            });
        }

        public void Excluir(int id)
        {
            ExecutarSeguramente(() =>
            {
                var sala = ObterAfracOuExcecaoSeNaoEncontrar(id);

                Contexto.RepositorioAfracs.Excluir(sala);
            });
        }

        private Afrac ObterAfracOuExcecaoSeNaoEncontrar(int id)
        {
            var afrac = Contexto.RepositorioAfracs.ObterPorId(id);

            if (afrac != null)
                return afrac;
            else
                throw new ExcecaoAplicacao("AppAfracs", "Não foi encontrado nenhuma afrac com o id informado.");
        }
    }
}
