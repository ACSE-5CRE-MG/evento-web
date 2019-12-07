﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventoWeb.WS.Secretaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscricoesController : ControllerBase
    {
        private AppInscricoes m_App;

        public InscricoesController(IContexto contexto)
        {
            m_App = new AppInscricoes(contexto);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/listarTodas")]
        public IEnumerable<DTOBasicoInscricao> ListarTodas(int idEvento, EnumSituacaoInscricao situacao)
        {
            return m_App.ListarTodas(idEvento, situacao);
        }

        [Authorize("Bearer")]
        [HttpDelete("evento/{idEvento}/excluir/{idInscricao}")]
        public void Excluir(int idEvento, int idInscricao)
        {
            m_App.Excluir(idEvento, idInscricao);
        }

        [Authorize("Bearer")]
        [HttpGet("evento/{idEvento}/obter/{idInscricao}")]
        public DTOInscricaoCompleta Obter(int idEvento, int idInscricao)
        {
            return m_App.Obter(idEvento, idInscricao);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/aceitar/{idInscricao}")]
        public void Aceitar(int idEvento, int idInscricao, [FromBody] DTOInscricaoAtualizacao atualizacao)
        {
            m_App.Aceitar(idEvento, idInscricao, atualizacao);
        }

        [Authorize("Bearer")]
        [HttpPut("evento/{idEvento}/rejeitar/{idInscricao}")]
        public void Rejeitar(int idEvento, int idInscricao)
        {
            m_App.Rejeitar(idEvento, idInscricao);
        }
    }
}