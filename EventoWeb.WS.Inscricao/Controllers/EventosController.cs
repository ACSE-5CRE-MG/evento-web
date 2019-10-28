using EventoWeb.Nucleo.Aplicacao;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventoWeb.WS.Inscricao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly AppInscOnlineEvento mAppEvento;

        public EventosController(IContexto contexto)
        {
            mAppEvento = new AppInscOnlineEvento(contexto);
        }

        [HttpGet("disponiveis")]
        public IEnumerable<DTOEventoListagem> ListarEventosDisponiveis()
        {
            return mAppEvento.ListarEventosDisponiveisInscricaoOnline();
        }

        [HttpGet("{id}")]
        public DTOEventoListagem ObterEventos(int id)
        {
            return mAppEvento.ObterPorIdDisponivelInscricaoOnline(id);
        }
    }
}