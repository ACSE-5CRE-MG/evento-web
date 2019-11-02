using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public class InclusaoAfrac
    {
        private AOficinas mAfracs;

        public InclusaoAfrac(AOficinas afracs)
        {
            if (afracs == null)
                throw new ArgumentNullException("afracs", "Parâmetro afracs não pode ser vazio.");

            this.mAfracs = afracs;
        }

        public void Persistir(Oficina afrac)
        {
            if (mAfracs.HaParticipatesOuResponsaveisEmOutraOficina(afrac))
                throw new ERepositorio("Existem participantes nesta afrac, como responspaveis ou participantes, que estão em outra afrac.");

            mAfracs.Incluir(afrac);
        }
    }
}
