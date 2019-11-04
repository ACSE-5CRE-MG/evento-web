using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppOficinas : AppBase
    {
        public AppOficinas(IContexto contexto) : base(contexto)
        {
        }

        public IList<DTOOficina> ObterTodos(int idEvento)
        {
            var lista = new List<DTOOficina>();
            ExecutarSeguramente(() =>
            {
                var afracs = Contexto.RepositorioOficinas.ListarTodasPorEvento(idEvento);
                if (afracs.Count > 0)
                    lista.AddRange(afracs.Select(x => x.Converter()));
            });

            return lista;
        }

        public DTOOficina ObterPorId(int id)
        {
            DTOOficina dto = null;
            ExecutarSeguramente(() =>
            {
                var oficina = Contexto.RepositorioOficinas.ObterPorId(id);

                if (oficina != null)
                    dto = oficina.Converter();
            });

            return dto;
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
                var oficina = ObterOficinaOuExcecaoSeNaoEncontrar(id);
                oficina.Nome = dto.Nome;
                oficina.DeveSerParNumeroTotalParticipantes = dto.DeveSerParNumeroTotalParticipantes;
                oficina.NumeroTotalParticipantes = dto.NumeroTotalParticipantes;

                Contexto.RepositorioOficinas.Atualizar(oficina);
            });
        }

        public void Excluir(int id)
        {
            ExecutarSeguramente(() =>
            {
                var oficina = ObterOficinaOuExcecaoSeNaoEncontrar(id);

                Contexto.RepositorioOficinas.Excluir(oficina);
            });
        }

        private Oficina ObterOficinaOuExcecaoSeNaoEncontrar(int id)
        {
            var oficina = Contexto.RepositorioOficinas.ObterPorId(id);

            if (oficina != null)
                return oficina;
            else
                throw new ExcecaoAplicacao("AppAfracs", "Não foi encontrado nenhuma afrac com o id informado.");
        }
    }
}
