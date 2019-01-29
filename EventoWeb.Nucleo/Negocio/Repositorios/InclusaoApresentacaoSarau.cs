using EventoWeb.Nucleo.Negocio.Entidades;
using System;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public class InclusaoApresentacaoSarau
    {
        private AApresentacoesSarau mRepositorioApresentacoes;

        public InclusaoApresentacaoSarau(AApresentacoesSarau repositorioApresentacoes)
        {
            if (repositorioApresentacoes == null)
                throw new ArgumentNullException("repositorioApresentacoes", "Parâmetro repositorioApresentacoes não pode ser vazio.");

            this.mRepositorioApresentacoes = repositorioApresentacoes;
        }

        public void Persistir(ApresentacaoSarau apresentacao)
        {
            if (mRepositorioApresentacoes.ObterTempoTotalApresentacoes(apresentacao.Evento) + apresentacao.DuracaoMin > 
                  apresentacao.Evento.ConfiguracaoSarau.TempoDuracaoMin)
                throw new ERepositorio("A soma do tempo de todas as apresentações, inclusive com esta, ultrapassa o tempo definido para o evento.");

            mRepositorioApresentacoes.Incluir(apresentacao);
        }
    }
}
