using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public class ValidacaoExclusaoResponsavelPorCrianca
    {
        private AInscricoes mInscricoes;

        public ValidacaoExclusaoResponsavelPorCrianca(AInscricoes inscricoes)
        {
            mInscricoes = inscricoes;
        }

        public void Validar(Inscricao inscricao)
        {
            var criancas = mInscricoes.ListarInscricoesInfantisDoResponsavel(inscricao);
            if (criancas != null && criancas.Count > 0)
                throw new ERepositorio(GerarMensagemExcecaoResponsavelPorCriancas(criancas));
        }

        private string GerarMensagemExcecaoResponsavelPorCriancas(IList<InscricaoInfantil> criancas)
        {
            String mensagem = "";
            if (criancas.Count == 1)
                mensagem = "Este participante é responsável por criança. Para excluí-lo, exclua a criança primeiro.";
            else
                mensagem = "Este participante é responsável por crianças. Para excluí-lo, exclua as crianças primeiro. ";

            foreach (var crianca in criancas)
                mensagem = mensagem + "\n  - " + crianca.Pessoa.Nome;

            return mensagem;
        }
    }
}
