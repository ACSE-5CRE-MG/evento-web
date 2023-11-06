using EventoWeb.Nucleo.Negocio.Entidades;
using System;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppUsuarioInclusao : AppBase
    {
        public AppUsuarioInclusao(IContexto contexto)
            : base(contexto) { }

        public DTOUsuarioInclusao DadosUsuario {  get; set; }

        public void Incluir()
        {
            if (string.IsNullOrWhiteSpace(DadosUsuario.Login))
                throw new Exception("Login precisa ser informado");

            ExecutarSeguramente(() =>
            {
                var repositorio = Contexto.RepositorioUsuarios;

                if (repositorio.ObterPeloLogin(DadosUsuario.Login) != null)
                    throw new Exception("Já existe um usuário com este login!");

                var usuario = new Usuario(
                    DadosUsuario.Login,
                    DadosUsuario.Nome,
                    new SenhaUsuario(DadosUsuario.Senha, DadosUsuario.RepeticaoSenha));
                usuario.EhAdministrador = DadosUsuario.EhAdministrador;

                Contexto.RepositorioUsuarios.Incluir(usuario);
            });
        }
    }
}
