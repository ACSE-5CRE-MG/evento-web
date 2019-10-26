using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventoWeb.Nucleo.AplicacaoInscricaoOnLine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.WS.Inscricao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private AppEvento mAppEvento;

        public EventosController(AppEvento appEvento)
        {
            mAppEvento = appEvento;
        }

        [HttpGet("disponiveis")]
        public IEnumerable<DTOEventoListagem> ListarEventosDisponiveis()
        {
            return mAppEvento.ListarEventosDisponiveis();
        }

        [HttpGet("{id}")]
        public DTOEventoListagem ObterEventos(int id)
        {
            return mAppEvento.ObterPorId(id);
        }
    }
}