using EventoWeb.Nucleo.Negocio.Entidades;
using System;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public class AlteracaoApresentacaoSarau
    {
        private AApresentacoesSarau mRepositorioApresentacoes;

        public AlteracaoApresentacaoSarau(AApresentacoesSarau repositorioApresentacoes)
        {
            if (repositorioApresentacoes == null)
                throw new ArgumentNullException("repositorioApresentacoes", "Parâmetro repositorioApresentacoes não pode ser vazio.");

            this.mRepositorioApresentacoes = repositorioApresentacoes;
        }

        public void Persistir(ApresentacaoSarau apresentacao)
        {
            if (mRepositorioApresentacoes.ObterTempoTotalApresentacoes(apresentacao.Evento, apresentacao) + apresentacao.DuracaoMin > apresentacao.Evento.TempoDuracaoSarauMin)
                throw new ERepositorio("A soma do tempo de todas as apresentações, inclusive com esta, ultrapassa o tempo definido para o evento.");

            mRepositorioApresentacoes.Atualizar(apresentacao);
        }
    }
}
