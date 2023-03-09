using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class DivisaoAutomaticaInscricoesParticipantePorOficinaOrdem
    {
        private Evento mEvento;
        private AInscricoes mInscricoes;
        private AOficinas mOficinas;

        public DivisaoAutomaticaInscricoesParticipantePorOficinaOrdem(Evento evento, AInscricoes inscricoes, AOficinas oficinas)
        {
            mEvento = evento;
            mInscricoes = inscricoes;
            mOficinas = oficinas;
        }

        public IList<Oficina> Dividir()
        {
            var participantes =
                mInscricoes.ListarTodasInscricoesAceitasPorAtividade<AtividadeInscricaoOficinaSemEscolha>(mEvento);

            IList<Oficina> oficinas = mOficinas.ListarTodasPorEvento(mEvento.Id);
            if (!oficinas.Any())
                throw new InvalidOperationException("Não há oficinas para realizar a divisão.");

            foreach (var oficina in oficinas)
                oficina.RemoverTodosParticipantes();

            int indiceOficina = 0;

            IList<InscricaoParticipante> participantesComOficinaDefinida = new List<InscricaoParticipante>();

            foreach (var participante in 
                participantes
                    .OrderByDescending(x => x.Inscrito.Pessoa.DataNascimento)
                    .ThenBy(x=>x.Inscrito.Pessoa.Endereco.Cidade))
            {
                oficinas[indiceOficina].AdicionarParticipante(participante.Inscrito);

                indiceOficina++;
                if (indiceOficina == oficinas.Count)
                    indiceOficina = 0;
            }

            return oficinas;
        }
    }
}
