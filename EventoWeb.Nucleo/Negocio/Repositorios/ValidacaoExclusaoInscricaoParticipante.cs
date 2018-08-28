using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public class ValidacaoExclusaoInscricaoParticipante: IValidacaoInscricao
    {
        private AInscricoes mInscricoes;

        public ValidacaoExclusaoInscricaoParticipante(AInscricoes inscricoes)
        {
            mInscricoes = inscricoes;
        }

        public bool PossoValidar(Inscricao inscricao)
        {
            return inscricao is InscricaoParticipante;
        }

        public void Validar(Inscricao inscricao)
        {
            if (!PossoValidar(inscricao))
                throw new ERepositorio("A inscrição deve ser do tipo Participante.");

            var validacaoRespCrianca = new ValidacaoExclusaoResponsavelPorCrianca(mInscricoes);
            validacaoRespCrianca.Validar(inscricao);
        }
    }
}
