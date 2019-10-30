using System;
using System.Collections.Generic;
using System.Text;
using EventoWeb.Nucleo.Negocio.Entidades;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface IServicoEmail
    {
        void Enviar(Evento evento, ModeloMensagem mensagem);
        void EnviarEmailCodigoInscricao(Inscricao inscricao, string codigo);
    }
}
