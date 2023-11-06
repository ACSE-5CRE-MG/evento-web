using System;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppUsuarioAlteracaoDados : AppBase
    {
        public AppUsuarioAlteracaoDados(IContexto contexto)
            : base(contexto) { }

        public DTOUsuario DadosUsuario { get; set; }

        public void Alterar()
        {
            if (DadosUsuario == null)
                throw new Exception("Dados do usuário não foram informados!");

            if (string.IsNullOrWhiteSpace(DadosUsuario.Login))
                throw new Exception("Login precisa ser informado");

            ExecutarSeguramente(() =>
            {
                var usuario = Contexto.RepositorioUsuarios.ObterPeloLogin(DadosUsuario.Login) ?? 
                    throw new Exception("Nenhum usuário foi encontrado com esse login!");

                usuario.Nome = DadosUsuario.Nome;
                usuario.EhAdministrador = DadosUsuario.EhAdministrador;

                Contexto.RepositorioUsuarios.Atualizar(usuario);
            });
        }
    }
}
