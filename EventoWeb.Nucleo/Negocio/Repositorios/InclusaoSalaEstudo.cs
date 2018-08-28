using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public class InclusaoSalaEstudo
    {
        private ASalasEstudo mSalasEstudo;

        public InclusaoSalaEstudo(ASalasEstudo salasEstudo)
        {
            if (salasEstudo == null)
                throw new ArgumentNullException("salasEstudo", "Parâmetro salasEstudo não pode ser vazio.");

            this.mSalasEstudo = salasEstudo;
        }

        public void Persistir(SalaEstudo sala)
        {
            if (sala.FaixaEtaria != null && mSalasEstudo.HaSalaComFaixaEtariaDefinida(sala))
                throw new ERepositorio("Já existe outra sala de estudo com faixa etária definida. O sistema somente aceita uma sala com faixa etária.");

            if (mSalasEstudo.HaParticipatesEmOutraSala(sala))
                throw new ERepositorio("Existem participantes nesta sala, como coordenadores ou participantes, que estão em outra sala.");

            mSalasEstudo.Incluir(sala);
        }
    }
}
