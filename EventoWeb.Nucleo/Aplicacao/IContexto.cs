using EventoWeb.Nucleo.Negocio.Repositorios;
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
        AOficinas RepositorioOficinas { get; }
        AInscricoes RepositorioInscricoes { get; }
        AConfiguracoesEmail RepositorioConfiguracoesEmail { get; }
        AMensagensEmailPadrao RepositorioMensagensEmailPadrao { get; }
        ACodigosAcessoInscricao RepositorioCodigosAcessoInscricao { get; }
        AApresentacoesSarau RepositorioApresentacoesSarau { get; }
        IServicoGeradorCodigoSeguro ServicoGeradorCodigoSeguro { get; }
        AArquivosBinarios RepositorioArquivosBinarios { get; }

        ATitulos RepositorioTitulosFinanceiros { get; }
        ATransacoes RepositorioTransacoesFinanceiras { get; }
        AContas RepositorioContasBancarias { get; }
        AFaturamentos RepositorioFaturamentos { get; }
    }
}
