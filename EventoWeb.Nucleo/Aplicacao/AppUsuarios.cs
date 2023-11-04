using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppUsuarios : AppBase
    {
        public AppUsuarios(IContexto contexto)
            : base(contexto) { }

        public Usuario BuscarPeloLogin(string login)
        {
            Usuario usuario = null;
            ExecutarSeguramente(() =>
            {
                usuario = Contexto.RepositorioUsuarios.ObterPeloLogin(login);
            });

            return usuario;
        }
    }
}
