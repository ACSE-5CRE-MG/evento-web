using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class InclusaoTransacao
    {
        private ITransacoes mRepTransacoes;
        public InclusaoTransacao(ITransacoes repTransacoes)
        {
            mRepTransacoes = repTransacoes;
        }

        public void Incluir(Transacao transacao)
        {
            if (transacao == null)
                throw new ArgumentNullException("transacao");

            if (transacao.Tipo == TipoTransacao.Despesa &&
                transacao.QualConta.SaldoInicial + mRepTransacoes.ObterTotalTransacoesPorData(transacao.QualConta.Id, DateTime.Now) < transacao.Valor)
                throw new InvalidOperationException(
                    String.Format("O saldo da conta {0} é insuficiente para efetivar a transação.", transacao.QualConta.Descricao));

            DateTime? dataUltimaTransacao = mRepTransacoes.ObterDataUltimaTransacaoDaConta(transacao.QualConta.Id);

            /*if (dataUltimaTransacao != null && transacao.DataHora < dataUltimaTransacao)
                throw new InvalidOperationException(
                    String.Format("Não é possível incluir uma transação com data anterior a {0}", 
                    dataUltimaTransacao.Value.ToString("dd/MM/yyyy hh:mm")));*/

            mRepTransacoes.Incluir(transacao);
        }
    }
}
