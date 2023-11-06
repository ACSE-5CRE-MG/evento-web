using System;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppUsuarioExclusao : AppBase
    {
        public AppUsuarioExclusao(IContexto contexto)
            : base(contexto) { }

        public string Login {  get; set; }

        public void Excluir()
        {
            if (string.IsNullOrWhiteSpace(Login))
                throw new Exception("Login precisa ser informado");

            ExecutarSeguramente(() =>
            {
                var repositorio = Contexto.RepositorioUsuarios;

                var usuario = repositorio.ObterPeloLogin(Login) ??
                    throw new Exception("Nenhum usuário foi encontrado com esse login!");

                Contexto.RepositorioUsuarios.Excluir(usuario);
            });
        }
    }
}
