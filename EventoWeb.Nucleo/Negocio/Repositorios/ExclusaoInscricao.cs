using NegocioEventoWeb.AplicativosFinanceiro;
using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public class ExclusaoInscricao
    {
        private ISet<IValidacaoInscricao> mValidacoes;
        private AInscricoes mInscricoes;
        private ASituacoesConfirmacaoInscricao mSituacoesInsc;

        public ExclusaoInscricao(ISet<IValidacaoInscricao> validacoes, AInscricoes inscricoes, ASituacoesConfirmacaoInscricao situacoesInsc)
        {
            mValidacoes = validacoes;
            mInscricoes = inscricoes;
            mSituacoesInsc = situacoesInsc;
        }

        public void Excluir(Inscricao inscricao)
        {
            foreach (var validacao in mValidacoes)
            {
                if (validacao.PossoValidar(inscricao))
                    validacao.Validar(inscricao);
            }

            if (inscricao.SituacaoFinanceiro != TipoSituacaoPagamento.Isento)
            {
                var titulo = inscricao.TituloPagar;
                var transacao = inscricao.TransacaoPagamento;
                var situacao = inscricao.SituacaoFinanceiro;

                inscricao.IsentarInscricao();
                mInscricoes.Atualizar(inscricao);

                var appTitulo = new AplicativoTituloFinanceiro();
                switch (situacao)
                {
                    case TipoSituacaoPagamento.Pagar:
                        appTitulo.Excluir(titulo);
                        break;
                    case TipoSituacaoPagamento.Pago:
                        if (titulo != null)
                        {
                            appTitulo.EstornarParcela(titulo.Parcelas.First());
                            appTitulo.Excluir(titulo);
                        }
                        else
                            new AplicativoTransacao().Excluir(transacao);
                        break;
                }
            }

            mInscricoes.Excluir(inscricao);
        }
    }
}
