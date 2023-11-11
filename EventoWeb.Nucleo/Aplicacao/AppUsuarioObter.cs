using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppUsuarioObter : AppBase
    {
        public AppUsuarioObter(IContexto contexto)
            : base(contexto) { }

        public string Login { get; set; }

        public DTOUsuario Obter()
        {
            DTOUsuario dto = null;

            ExecutarSeguramente(() =>
            {
                var repositorio = Contexto.RepositorioUsuarios;

                dto = repositorio
                    .ObterPeloLogin(Login)?
                    .Converter();
            });

            return dto;
        }
    }
}
