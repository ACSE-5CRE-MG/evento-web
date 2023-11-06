using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppUsuarioListagem : AppBase
    {
        public AppUsuarioListagem(IContexto contexto)
            : base(contexto) { }

        public DTOUsuarioInclusao DadosUsuario {  get; set; }

        public IList<DTOUsuario> ListarTodos()
        {
            var lista = new List<DTOUsuario>();

            ExecutarSeguramente(() =>
            {
                var repositorio = Contexto.RepositorioUsuarios;
                
                lista = repositorio
                    .ListarTodos()
                    .Select(x=> x.Converter())
                    .ToList();
            });

            return lista;
        }
    }
}
