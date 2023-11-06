using EventoWeb.Nucleo.Negocio.Entidades;

namespace EventoWeb.Nucleo.Aplicacao.ConversoresDTO
{
    public static class ConversorUsuario
    {
        public static DTOUsuario Converter(this Usuario usuario)
        {
            if (usuario == null)
                return null;
            else
                return new DTOUsuario
                {
                    Login = usuario.Login,
                    Nome = usuario.Nome,
                    EhAdministrador = usuario.EhAdministrador,
                };
        }
    }
}
