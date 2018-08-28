using EventoWeb.Nucleo.Negocio.Entidades;
using System;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AUsuarios
    {
        public abstract Usuario ObterUsuarioPeloLogin(String login);
    }
}
