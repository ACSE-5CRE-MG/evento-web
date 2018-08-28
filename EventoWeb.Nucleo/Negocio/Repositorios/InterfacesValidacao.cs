using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface IValidacaoInscricao
    {
        Boolean PossoValidar(Inscricao inscricao);
        void Validar(Inscricao inscricao);
    }
}
