using System;
using System.Collections.Generic;
using System.Text;
using EventoWeb.Nucleo.Negocio.Entidades;

namespace EventoWeb.Nucleo.Negocio.Repositorios
{
    public interface IServicoEmail
    {
        void Enviar(Evento evento, ModeloMensagem mensagem, string emailDestino);
        void EnviarEmailCodigoInscricao(Inscricao inscricao, string codigo);
        void EnviarEmailInscricaoRegistrada();
    }
}
