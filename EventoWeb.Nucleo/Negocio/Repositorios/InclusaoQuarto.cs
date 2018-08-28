using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public class InclusaoQuarto
    {
        private AQuartos mQuartos;
        public InclusaoQuarto(AQuartos quartos)
        {
            mQuartos = quartos;
        }

        public void Persistir(Quarto quarto)
        {
            if (quarto.Capacidade == null && mQuartos.HaOutroQuartoComCapacidadeInfinita(quarto))
                throw new ERepositorio("Ao não informar a capacidade do quarto, o mesmo é considerado com " +
                    "capacidade infinita. E já há um quarto nessa condição.");

            mQuartos.Incluir(quarto);
        }
    }
}
