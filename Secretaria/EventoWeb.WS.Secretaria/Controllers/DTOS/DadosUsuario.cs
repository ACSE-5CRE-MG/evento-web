using EventoWeb.Nucleo.Aplicacao;
using System;

namespace EventoWeb.WS.Secretaria.Controllers.DTOS
{
    public class DTWDadosAutenticacao
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }

    public class DTWAutenticacao
    {
        public DTOUsuario Usuario { get; set; }
        public string TokenAutenticacao { get; set; }
        public DateTime Validade { get; set; }
    }

    public class DTOAlteracaoSenhaWS
    {
        public string NovaSenha { get; set; }
        public string NovaSenhaRepetida { get; set; }
    }

    public class DTOAlteracaoSenhaComumWS: DTOAlteracaoSenhaWS
    {
        public string SenhaAtual { get; set; }
    }
}