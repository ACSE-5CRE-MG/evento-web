using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class RegistroTransacaoTitulo
    {
        private InclusaoTransacao mInclusaoTransacao;
        private ITitulos mTituloFinanceiros;
        private ExclusaoTransacao mExclusaoTransacao;
        private AInscricoes mInscricoes;

        public RegistroTransacaoTitulo(InclusaoTransacao inclusaoTransacao,
            ExclusaoTransacao exclusaoTransacao,
            ITitulos titulosFinanceiros,
            AInscricoes inscricoes)
        {
            mInclusaoTransacao = inclusaoTransacao;
            mExclusaoTransacao = exclusaoTransacao;
            mTituloFinanceiros = titulosFinanceiros;
            mInscricoes = inscricoes;
        }

        public Transacao Registrar(ParcelaTitulo parcela, DateTime dataTransacao, Conta conta)
        {
            if (parcela == null)
                throw new ArgumentNullException("parcela");

            Inscricao inscricao = mInscricoes.ObterInscricaoVinculadaTitulo(parcela.TituloOrigem.Id);

            Transacao movimento = new Transacao(parcela.TituloOrigem.QualEvento, conta, parcela.TituloOrigem.QualCategoria,
                dataTransacao, parcela.TituloOrigem.Descricao + " - Liquidação de Parcela", parcela.Valor,
                parcela.TituloOrigem.Tipo);
            movimento.Quem = parcela.TituloOrigem.Quem;

            mInclusaoTransacao.Incluir(movimento);

            parcela.RegistrarTransacao(movimento);
            mTituloFinanceiros.Atualizar(parcela.TituloOrigem);

            if (inscricao != null)
            {
                inscricao.PagarAgora(movimento);
                mInscricoes.Atualizar(inscricao);
            }

            return movimento;
        }

        public Transacao Estornar(ParcelaTitulo parcela)
        {
            if (parcela == null)
                throw new ArgumentNullException("parcela");

            Inscricao inscricao = mInscricoes.ObterInscricaoVinculadaTitulo(parcela.TituloOrigem.Id);
            if (inscricao != null)
            {
                inscricao.IsentarInscricao();
                inscricao.PagarDepois(parcela.TituloOrigem);
                mInscricoes.Atualizar(inscricao);
            }

            var movimentoEstornado = parcela.EstornarRegistroTransacao();

            mTituloFinanceiros.Atualizar(parcela.TituloOrigem);
            mExclusaoTransacao.Excluir(movimentoEstornado);

            return movimentoEstornado;
        }
    }
}
