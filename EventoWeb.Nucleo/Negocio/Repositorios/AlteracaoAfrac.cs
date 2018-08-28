using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public class AlteracaoAfrac
    {
        private AAfracs mAfracs;

        public AlteracaoAfrac(AAfracs afracs)
        {
            if (afracs == null)
                throw new ArgumentNullException("afracs", "Parâmetro afracs não pode ser vazio.");

            this.mAfracs = afracs;
        }

        public void Persistir(Afrac afrac)
        {
            if (mAfracs.HaParticipatesOuResponsaveisEmOutraAfrac(afrac))
                throw new ERepositorio("Existem participantes nesta afrac, como responsáveis ou participantes, que estão em outra afrac.");

            mAfracs.Atualizar(afrac);
        }
    }
}
