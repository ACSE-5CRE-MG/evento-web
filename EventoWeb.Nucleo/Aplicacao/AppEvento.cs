using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppEvento: AppBase
    {
        public AppEvento(IContexto contexto)
            : base(contexto) { }

        public DTOEventoCompleto ObterPorId(int id)
        {
            DTOEventoCompleto dtoEvento = null;
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(id);
                dtoEvento = evento?.ConverterApenasEvento();
            });

            return dtoEvento;
        }

        public IList<DTOEventoMinimo> ObterTodos()
        {
            IList<DTOEventoMinimo> dtoEventos = null;
            ExecutarSeguramente(() =>
            {
                var eventos = Contexto.RepositorioEventos.ObterTodosEventos();
                dtoEventos = eventos.Select(x => x.ConverterMinimo()).ToList();
            });

            return dtoEventos;
        }

        public DTOId Incluir(DTOEvento dto)
        {
            Evento evento = new Evento(dto.Nome, dto.PeriodoInscricao, dto.PeriodoRealizacao, 
                dto.IdadeMinima)
            {
                Logotipo = new ArquivoBinario(Convert.FromBase64String(dto.Logotipo), EnumTipoArquivoBinario.ImagemJPEG),
                TemDepartamentalizacao = dto.TemDepartamentalizacao,
                TemDormitorios = dto.TemDormitorios,
                TemOficinas = dto.TemOficinas,
                ConfiguracaoEvangelizacao = dto.ConfiguracaoEvangelizacao,
                ConfiguracaoSalaEstudo = dto.ConfiguracaoSalaEstudo,
                ConfiguracaoTempoSarauMin = dto.ConfiguracaoTempoSarauMin
            };

            ExecutarSeguramente(() =>
            {
                Contexto.RepositorioEventos.Incluir(evento);
            });

            return new DTOId
            {
                Id = evento.Id
            };
        }

        public void Atualizar(int id, DTOEvento dto)
        {
            ExecutarSeguramente(() =>
            {
                var evento = ObterEventoOuExcecaoSeNaoEncontrar(id);

                evento.Nome = dto.Nome;
                evento.PeriodoInscricaoOnLine = dto.PeriodoInscricao;
                evento.PeriodoRealizacaoEvento = dto.PeriodoRealizacao;
                evento.TemDepartamentalizacao = dto.TemDepartamentalizacao;
                evento.TemDormitorios = dto.TemDormitorios;
                evento.TemOficinas = dto.TemOficinas;
                evento.ConfiguracaoEvangelizacao = dto.ConfiguracaoEvangelizacao;
                evento.ConfiguracaoSalaEstudo = dto.ConfiguracaoSalaEstudo;
                evento.ConfiguracaoTempoSarauMin = dto.ConfiguracaoTempoSarauMin;

                if (evento.Logotipo == null)
                    evento.Logotipo = new ArquivoBinario(Convert.FromBase64String(dto.Logotipo), EnumTipoArquivoBinario.ImagemJPEG);
                else
                    evento.Logotipo.Arquivo = Convert.FromBase64String(dto.Logotipo);

                Contexto.RepositorioEventos.Atualizar(evento);
            });
        }

        public void Excluir(int id)
        {
            ExecutarSeguramente(() =>
            {
                var evento = ObterEventoOuExcecaoSeNaoEncontrar(id);
                Contexto.RepositorioEventos.Excluir(evento);
            });
        }

        private Evento ObterEventoOuExcecaoSeNaoEncontrar(int id)
        {
            Evento evento = Contexto.RepositorioEventos.ObterEventoPeloId(id);

            if (evento != null)
                return evento;
            else
                throw new ExcecaoAplicacao("AppEvento", "Não foi encontrado nenhum evento com o id informado.");
        }
    }
}
