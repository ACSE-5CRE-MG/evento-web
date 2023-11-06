using System;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppUsuarioAlteracaoSenhaPeloUsuario : AppBase
    {
        public AppUsuarioAlteracaoSenhaPeloUsuario(IContexto contexto)
            : base(contexto) { }

        public string Login { get; set; }
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
        public string NovaSenhaRepeticao { get; set; }

        public void Alterar()
        {
            if (string.IsNullOrWhiteSpace(Login))
                throw new Exception("Login precisa ser informado");

            ExecutarSeguramente(() =>
            {
                var usuario = Contexto.RepositorioUsuarios.ObterPeloLogin(Login) ?? 
                    throw new Exception("Nenhum usuário foi encontrado com esse login!");

                if (!usuario.Senha.EhIgual(SenhaAtual))
                    throw new Exception("A senha informada não é igual a que temos cadastrada!");
                
                usuario.Senha.AlterarSenha(NovaSenha, NovaSenhaRepeticao);
                Contexto.RepositorioUsuarios.Atualizar(usuario);
            });
        }
    }
}
