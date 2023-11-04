using EventoWeb.Nucleo.Negocio.Entidades;
using System;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AUsuarios: ARepositorio<Usuario>
    {
        public AUsuarios(IPersistencia<Usuario> persistencia) : base(persistencia)
        {
        }

        public abstract Usuario ObterPeloLogin(String login);
    }
}
