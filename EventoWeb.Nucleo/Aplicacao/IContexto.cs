using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public interface IContexto: IDisposable
    {
        void IniciarTransacao();

        void SalvarTransacao();

        void CancelarTransacao();

        AEventos RepositorioEventos { get; }
        AUsuarios RepositorioUsuarios { get; }
    }
}
