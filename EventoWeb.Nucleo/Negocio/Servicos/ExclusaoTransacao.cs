using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class ExclusaoTransacao
    {
        private ITransacoes mRepTransacoes;
        private ITitulos mRepTitulos;
        private ITransferencias mRepTransferencias;
        private AInscricoes mRepInscricoes;

        public ExclusaoTransacao(ITransacoes repTransacoes, ITitulos repTitulos, ITransferencias repTransferencias,
            AInscricoes repInscricoes)
        {
            mRepTransacoes = repTransacoes;
            mRepTitulos = repTitulos;
            mRepTransferencias = repTransferencias;
            mRepInscricoes = repInscricoes;
        }

        public void Excluir(Transacao transacao)
        {
            if (transacao == null)
                throw new ArgumentNullException("transacao");

            if (mRepTitulos.HaTituloVinculadoTransacao(transacao.Id))
                throw new InvalidOperationException("Não é possível excluir uma transação que esta vinculada a um Título.");

            if (mRepTransferencias.HaTransferenciaVinculadaTransacao(transacao.Id))
                throw new InvalidOperationException("Não é possível excluir uma transação que faz parte de uma transferência.");

            if (mRepInscricoes.HaInscricaoVinculadaTransacao(transacao.Id))
                throw new InvalidOperationException("Não é possível excluir uma transação que é pagamento de Inscrição.");

            mRepTransacoes.Excluir(transacao);
        }
    }
}
