using System;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppUsuarioAlteracaoSenhaPeloAdm : AppBase
    {
        public AppUsuarioAlteracaoSenhaPeloAdm(IContexto contexto)
            : base(contexto) { }

        public string Login { get; set; }
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
               
                usuario.Senha.AlterarSenha(NovaSenha, NovaSenhaRepeticao);
                Contexto.RepositorioUsuarios.Atualizar(usuario);
            });
        }
    }
}
