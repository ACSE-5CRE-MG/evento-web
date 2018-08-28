using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public class AlteracaoInscricao
    {
        private AInscricoes mInscricoes;
        private IList<IValidacaoInscricao> mValidacoes;

        public AlteracaoInscricao(AInscricoes aInscricoes, 
            IList<IValidacaoInscricao> validacoes)
        {
            this.mInscricoes = aInscricoes;
            mValidacoes = validacoes;
        }
        public void Persistir(Inscricao inscricao)
        {
            foreach (var validacao in mValidacoes)
            {
                if (validacao.PossoValidar(inscricao))
                    validacao.Validar(inscricao);
            }

            mInscricoes.Atualizar(inscricao);
        }
    }
}
