using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class DadosTransferencia
    {
        public Evento QualEvento { get; set; }
        public DateTime DataTransferencia { get; set; }
        public Categoria CategoriaDebito { get; set; }
        public Conta DaConta { get; set; }
        public Categoria CategoriaCredito { get; set; }
        public Conta ParaConta { get; set; }
        public Decimal Valor { get; set; }
    }

    public class RegistroTransferencia
    {
        private ITransacoes mTransacoes;
        private ITransferencias mTransferencias;

        public RegistroTransferencia(ITransacoes transacoes, ITransferencias transferencias)
        {
            mTransacoes = transacoes;
            mTransferencias = transferencias;
        }

        public void Transferir(DadosTransferencia dados)
        {
            if (dados.DaConta == null)
                throw new ArgumentNullException("daConta");

            if (dados.ParaConta == null)
                throw new ArgumentNullException("paraConta");

            if (dados.Valor <= 0)
                throw new ArgumentException("Valor da transferência deve ser maior que zero.","valor");

            if (dados.DaConta == dados.ParaConta)
                throw new InvalidOperationException("A conta de origem e destino devem ser diferentes.");

            if (dados.DaConta.SaldoInicial + mTransacoes.ObterTotalTransacoesPorData(dados.DaConta.Id, DateTime.Now) < dados.Valor)
                throw new InvalidOperationException(
                    String.Format("O saldo da conta {0} é insuficiente para realizar a transferência.", dados.DaConta.Descricao));

            var transferencia = GerarTransferencia(dados);

            mTransacoes.Incluir(transferencia.TransacaoCredito);
            mTransacoes.Incluir(transferencia.TransacaoDebito);

            mTransferencias.Incluir(transferencia);
        }

        protected Transferencia GerarTransferencia(DadosTransferencia dados)
        {
            var dataAtual = DateTime.Now;
            var movimentoDebito = new Transacao(dados.QualEvento, dados.DaConta, dados.CategoriaDebito, dataAtual, "Transferência - Débito",
                dados.Valor, TipoTransacao.Despesa);
            var movimentoCredito = new Transacao(dados.QualEvento, dados.ParaConta, dados.CategoriaCredito, dataAtual, "Transferência - Crédito",
                dados.Valor, TipoTransacao.Receita);

            return new Transferencia(dados.QualEvento, movimentoDebito, movimentoCredito);
        }
    }
}
