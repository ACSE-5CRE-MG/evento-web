using System;
using System.Collections.Generic;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Relatorios
{
    public static class MetodosUteisTextSharp
    {
        public static float MillimetersToPointsTextSharp(this float value)
        {
            return value / 25.4f * 72f;
        }
    }
}
