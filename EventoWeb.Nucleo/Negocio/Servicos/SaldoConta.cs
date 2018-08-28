using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class SaldoConta
    {
        private ITransacoes mRepositorio;

        public SaldoConta(ITransacoes repositorio)
        {
            mRepositorio = repositorio;
        }

        public Decimal Calcular(Conta conta, DateTime data)
        {
            return conta.SaldoInicial + mRepositorio.ObterTotalTransacoesPorData(conta.Id, data);
        }
    }
}
