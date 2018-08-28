using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class InscritosAfrac
    {
        private Afrac mAfrac;
        private InscricaoParticipante[] mInscritos;

        public InscritosAfrac(Afrac afrac, InscricaoParticipante[] inscricao)
        {
            mAfrac = afrac;
            mInscritos = inscricao;
        }

        public Afrac Afrac { get { return mAfrac; } }
        public IEnumerable<InscricaoParticipante> Inscritos { get { return mInscritos; } }
    }
}
