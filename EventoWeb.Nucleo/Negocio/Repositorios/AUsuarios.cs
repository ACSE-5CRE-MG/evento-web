using EventoWeb.Nucleo.Negocio.Entidades;
using System;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public abstract class AUsuarios: ARepositorio<Usuario>
    {
        public AUsuarios(IPersistencia<Usuario> persistencia) : base(persistencia)
        {
        }

        public override void Incluir(Usuario objeto)
        {
            if (ObterUsuarioPeloLogin(objeto.Login) != null)
                throw new Exception(String.Format("O login \"{0}\" já existe.", objeto.Login));

            base.Incluir(objeto);
        }

        public abstract Usuario ObterUsuarioPeloLogin(String login);
    }
}
