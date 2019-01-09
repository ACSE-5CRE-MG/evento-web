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
                if (evento != null)
                    dtoEvento = new DTOEventoCompleto
                    {
                        Id = evento.Id,
                        DataFimInscricao = evento.DataFimInscricao.ToString("dd/MM/yyyy HH:mm"),
                        DataInicioInscricao = evento.DataInicioInscricao.ToString("dd/MM/yyyy HH:mm"),
                        DataFimEvento = evento.DataFimEvento.ToString("dd/MM/yyyy HH:mm"),
                        DataInicioEvento = evento.DataInicioEvento.ToString("dd/MM/yyyy HH:mm"),
                        DataRegistro = evento.DataRegistro.ToString("dd/MM/yyyy HH:mm"),
                        Logotipo = evento.Logotipo,
                        Nome = evento.Nome,
                        ValorInscricao = evento.ValorInscricao,
                        Situacao = evento.Situacao,
                        PodeAlterar = evento.Situacao != SituacaoEvento.Concluido,
                        EnderecoEmail = evento.ConfiguracaoEmail?.EnderecoEmail,
                        UsuarioEmail = evento.ConfiguracaoEmail?.UsuarioEmail,
                        SenhaEmail = evento.ConfiguracaoEmail?.SenhaEmail,
                        ServidorEmail = evento.ConfiguracaoEmail?.ServidorEmail,
                        PortaServidor = evento.ConfiguracaoEmail?.PortaServidor,
                        TipoSeguranca = evento.ConfiguracaoEmail?.TipoSeguranca == TipoSegurancaEmail.Nenhuma ? "Nenhuma" : "SSL",
                        TituloEmailConfirmacaoInscricao = evento.ConfiguracaoEmail?.TituloEmailConfirmacaoInscricao,
                        MensagemEmailConfirmacaoInscricao = evento.ConfiguracaoEmail?.MensagemEmailConfirmacaoInscricao,
                        TemDepartamentalizacao = evento.TemDepartamentalizacao,
                        TemDormitorios = evento.TemDormitorios,
                        TemOficinas = evento.TemOficinas,
                        TemSalasEstudo = evento.TemSalasEstudo,
                        TemEvangelizacao = evento.TemEvangelizacao,
                        PublicoEvangelizacao = evento.PublicoEvangelizacao,
                        TemSarau = evento.TemSarau,
                        TempoDuracaoSarauMin = evento.TempoDuracaoSarauMin,
                        ModeloDivisaoSalaEstudo = evento.ModeloDivisaoSalasEstudo
                    };
            });

            return dtoEvento;
        }

        public void Concluir(int id)
        {
            ExecutarSeguramente(() =>
            {
                var evento = ObterEventoOuExcecaoSeNaoEncontrar(id);

                evento.concluir();

                Contexto.RepositorioEventos.Atualizar(evento);
            });
        }

        public IList<DTOEventoMinimo> ObterTodos()
        {
            IList<DTOEventoMinimo> dtoEventos = null;
            ExecutarSeguramente(() =>
            {
                var eventos = Contexto.RepositorioEventos.ObterTodosEventos();
                dtoEventos = eventos.Select(x => new DTOEventoMinimo()
                {
                    Id = x.Id,
                    DataFimInscricao = x.DataFimInscricao,
                    DataInicioInscricao = x.DataInicioInscricao,
                    Situacao = x.Situacao,
                    Nome = x.Nome,
                    Logotipo = x.Logotipo
                }).ToList();
            });

            return dtoEventos;
        }

        public DTOEventoIncluido Incluir(DTOEvento eventoDTO)
        {
            Evento evento = new Evento(eventoDTO.Nome,
                DateTime.Parse(eventoDTO.DataInicioInscricao), DateTime.Parse(eventoDTO.DataFimInscricao),
                DateTime.Parse(eventoDTO.DataInicioEvento), DateTime.Parse(eventoDTO.DataFimEvento),
                eventoDTO.ValorInscricao)
            {
                Logotipo = eventoDTO.Logotipo,
                TemDepartamentalizacao = eventoDTO.TemDepartamentalizacao,
                TemDormitorios = eventoDTO.TemDormitorios,
                TemOficinas = eventoDTO.TemOficinas
            };

            if (eventoDTO.TemSalasEstudo)
                evento.UtilizaSalasEstudo(eventoDTO.ModeloDivisaoSalaEstudo.Value);
            else
                evento.NaoUtilizaSalasEstudo();

            evento.ConfigurarEvangelizacao(eventoDTO.TemEvangelizacao, eventoDTO.PublicoEvangelizacao);
            evento.ConfigurarSarau(eventoDTO.TemSarau, eventoDTO.TempoDuracaoSarauMin);

            evento.ConfiguracaoEmail.EnderecoEmail = eventoDTO.EnderecoEmail;
            evento.ConfiguracaoEmail.UsuarioEmail = eventoDTO.UsuarioEmail;
            evento.ConfiguracaoEmail.SenhaEmail = eventoDTO.SenhaEmail;
            evento.ConfiguracaoEmail.ServidorEmail = eventoDTO.ServidorEmail;
            evento.ConfiguracaoEmail.PortaServidor = eventoDTO.PortaServidor;
            evento.ConfiguracaoEmail.TituloEmailConfirmacaoInscricao = eventoDTO.TituloEmailConfirmacaoInscricao;
            evento.ConfiguracaoEmail.MensagemEmailConfirmacaoInscricao = eventoDTO.MensagemEmailConfirmacaoInscricao;

            if (String.IsNullOrEmpty(eventoDTO.TipoSeguranca))
                evento.ConfiguracaoEmail.TipoSeguranca = null;
            else
                evento.ConfiguracaoEmail.TipoSeguranca = eventoDTO.TipoSeguranca == "Nenhuma" ? TipoSegurancaEmail.Nenhuma : TipoSegurancaEmail.SSL;

            ExecutarSeguramente(() =>
            {
                Contexto.RepositorioEventos.Incluir(evento);
            });

            return new DTOEventoIncluido
            {
                IdEvento = evento.Id
            };
        }

        public void Atualizar(int id, DTOEvento eventoDTO)
        {
            ExecutarSeguramente(() =>
            {
                var evento = ObterEventoOuExcecaoSeNaoEncontrar(id);

                evento.Nome = eventoDTO.Nome;
                evento.DefinirDataInscricao(DateTime.Parse(eventoDTO.DataInicioInscricao), DateTime.Parse(eventoDTO.DataFimInscricao));
                evento.DefinirDataEvento(DateTime.Parse(eventoDTO.DataInicioEvento), DateTime.Parse(eventoDTO.DataFimEvento));
                evento.Logotipo = eventoDTO.Logotipo;
                evento.ValorInscricao = eventoDTO.ValorInscricao;
                evento.TemDepartamentalizacao = eventoDTO.TemDepartamentalizacao;
                evento.TemDormitorios = eventoDTO.TemDormitorios;
                evento.TemOficinas = eventoDTO.TemOficinas;

                if (eventoDTO.TemSalasEstudo)
                    evento.UtilizaSalasEstudo(eventoDTO.ModeloDivisaoSalaEstudo.Value);
                else
                    evento.NaoUtilizaSalasEstudo();

                evento.ConfigurarEvangelizacao(eventoDTO.TemEvangelizacao, eventoDTO.PublicoEvangelizacao);
                evento.ConfigurarSarau(eventoDTO.TemSarau, eventoDTO.TempoDuracaoSarauMin);

                evento.ConfiguracaoEmail.EnderecoEmail = eventoDTO.EnderecoEmail;
                evento.ConfiguracaoEmail.UsuarioEmail = eventoDTO.UsuarioEmail;
                evento.ConfiguracaoEmail.SenhaEmail = eventoDTO.SenhaEmail;
                evento.ConfiguracaoEmail.ServidorEmail = eventoDTO.ServidorEmail;
                evento.ConfiguracaoEmail.PortaServidor = eventoDTO.PortaServidor;
                evento.ConfiguracaoEmail.TituloEmailConfirmacaoInscricao = eventoDTO.TituloEmailConfirmacaoInscricao;
                evento.ConfiguracaoEmail.MensagemEmailConfirmacaoInscricao = eventoDTO.MensagemEmailConfirmacaoInscricao;

                if (String.IsNullOrEmpty(eventoDTO.TipoSeguranca))
                    evento.ConfiguracaoEmail.TipoSeguranca = null;
                else
                    evento.ConfiguracaoEmail.TipoSeguranca = eventoDTO.TipoSeguranca == "Nenhuma" ? TipoSegurancaEmail.Nenhuma : TipoSegurancaEmail.SSL;

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
