﻿using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Aplicacao
{
    public interface IContexto : IDisposable
    {
        void IniciarTransacao();

        void SalvarTransacao();

        void CancelarTransacao();

        AEventos RepositorioEventos { get; }
        AUsuarios RepositorioUsuarios { get; }
        ADepartamentos RepositorioDepartamentos { get; }
        ASalasEstudo RepositorioSalasEstudo { get; }
        AAfracs RepositorioAfracs { get; }
        AInscricoes RepositorioInscricoes { get; }
        AConfiguracoesEmail RepositorioConfiguracoesEmail { get; }
        AMensagensEmailPadrao RepositorioMensagensEmailPadrao { get; }
        ACodigosAcessoInscricao RepositorioCodigosAcessoInscricao { get; }
        AVariaveisEmailInscricao RepositorioVariaveisEmailIncricao { get; }

        IServicoGeradorCodigoSeguro ServicoGeradorCodigoSeguro { get; }
        IServicoEmail ServicoEmail { get; }
    }
}
