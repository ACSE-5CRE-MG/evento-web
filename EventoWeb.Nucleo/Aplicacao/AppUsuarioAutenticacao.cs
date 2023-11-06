using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using System;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppUsuarioAutenticacao : AppBase
    {
        public AppUsuarioAutenticacao(IContexto contexto)
            : base(contexto) { }

        public string Login { get; set; }
        public string Senha { get; set; }

        public DTOUsuario Autenticar()
        {
            if (string.IsNullOrWhiteSpace(Login))
                throw new Exception("Login precisa ser informado");

            DTOUsuario usuario = null;
            ExecutarSeguramente(() =>
            {
                var usr = Contexto.RepositorioUsuarios.ObterPeloLogin(Login);
                if (usr != null && usr.Senha.EhIgual(Senha))
                    usuario = usr.Converter();

            });

            return usuario;
        }
    }
}
