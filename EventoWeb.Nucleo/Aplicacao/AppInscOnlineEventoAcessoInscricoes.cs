using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Aplicacao.ConversoresDTO;
using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppInscOnlineEventoAcessoInscricoes : AppBase
    {
        private AppEmailMsgPadrao m_AppEmail;

        public AppInscOnlineEventoAcessoInscricoes(IContexto contexto, AppEmailMsgPadrao appEmail)
            : base(contexto) 
        {
            m_AppEmail = appEmail;
        }

        public DTOBasicoInscricao ObterPorId(int id)
        {
            DTOBasicoInscricao dtoInscricao = null;
            ExecutarSeguramente(() =>
            {
                var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(id);
                if (inscricao != null && inscricao is InscricaoParticipante)
                    dtoInscricao = inscricao.ConverterBasico();
            });

            return dtoInscricao;
        }

        public bool ValidarCodigo(int id, string codigo)
        {
            bool valido = false;
            ExecutarSeguramente(() =>
            {
                var repCodigos = Contexto.RepositorioCodigosAcessoInscricao;
                repCodigos.ExcluirCodigosVencidos();

                var codigoAcesso = repCodigos.ObterPeloCodigo(codigo);
                valido = codigoAcesso != null && codigoAcesso.Inscricao.Id == id;
            });

            return valido;
        }

        public DTOEnvioCodigoAcessoInscricao EnviarCodigo(string identificacao)
        {
            DTOEnvioCodigoAcessoInscricao dto = new DTOEnvioCodigoAcessoInscricao
            {
                IdInscricao = null,
                Resultado = EnumResultadoEnvio.InscricaoNaoEncontrada
            };
            ExecutarSeguramente(() =>
            {
                try
                {
                    int idInscricao = new AppInscOnLineIdentificacaoInscricao().ExtrarId(identificacao);
                    var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(idInscricao);

                    if (inscricao != null && inscricao is InscricaoParticipante)
                    {
                        dto.IdInscricao = inscricao.Id;
                        if (inscricao.Evento.PeriodoInscricaoOnLine.DataFinal < DateTime.Now || inscricao.Evento.PeriodoInscricaoOnLine.DataInicial > DateTime.Now)
                            dto.Resultado = EnumResultadoEnvio.EventoEncerradoInscricao;
                        else
                            dto.Resultado = EnumResultadoEnvio.InscricaoOK;

                        if (dto.Resultado == EnumResultadoEnvio.InscricaoOK)
                        {
                            string codigo = GerarCodigoUnico();
                            var codigoAcesso = new CodigoAcessoInscricao(codigo, inscricao, DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59));
                            Contexto.RepositorioCodigosAcessoInscricao.Incluir(codigoAcesso);

                            m_AppEmail.EnviarCodigoAcompanhamentoInscricao((InscricaoParticipante)inscricao, codigo);                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is ExcecaoAplicacao)
                        dto.Resultado = EnumResultadoEnvio.IdentificacaoInvalida;
                    else
                        throw ex;
                }
            });

            return dto;
        }

        public bool ValidarCodigoEmail(string identificacao, string codigo)
        {
            bool valido = false;
            ExecutarSeguramente(() =>
            {
                var repCodigos = Contexto.RepositorioCodigosAcessoInscricao;
                repCodigos.ExcluirCodigosVencidos();

                var codigoAcesso = repCodigos.ObterPeloCodigo(codigo);
                valido = codigoAcesso != null && codigoAcesso.Identificacao.ToUpper() == identificacao.ToUpper();
            });

            return valido;
        }

        public void EnviarCodigoEmail(int idEvento, string identificacao, string email)
        {
            ExecutarSeguramente(() =>
            {
                string codigo = GerarCodigoUnico();
                var codigoAcesso = new CodigoAcessoInscricao(codigo, identificacao, DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59));
                Contexto.RepositorioCodigosAcessoInscricao.Incluir(codigoAcesso);

                m_AppEmail.EnviarCodigoValidacaoEmail(idEvento, email, codigo);
            });
        }

        private string GerarCodigoUnico()
        {
            var m_RepCodigosAcessoInscricao = Contexto.RepositorioCodigosAcessoInscricao;
            m_RepCodigosAcessoInscricao.ExcluirCodigosVencidos();

            string codigo;
            do
            {
                codigo = Contexto.ServicoGeradorCodigoSeguro.GerarCodigo5Caracteres();
            } while (m_RepCodigosAcessoInscricao.ObterPeloCodigo(codigo) != null);

            return codigo;
        }


        public DTODadosConfirmacao CriarInscricao(int idEvento, DTOInscricaoAtualizacao dtoInscricao)
        {
            DTODadosConfirmacao dto = null;
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
                if (evento.PeriodoInscricaoOnLine.DataFinal < DateTime.Now || evento.PeriodoInscricaoOnLine.DataInicial > DateTime.Now)
                    throw new ExcecaoAplicacao("AppInscOnlineEventoAcessoInscricoes", "Evento encerrado");

                var pessoa = new Pessoa(dtoInscricao.DadosPessoais.Nome,
                    new Endereco(dtoInscricao.DadosPessoais.Cidade, dtoInscricao.DadosPessoais.Uf), dtoInscricao.DadosPessoais.DataNascimento,
                    dtoInscricao.DadosPessoais.Sexo, dtoInscricao.DadosPessoais.Email);

                var novaInscricao = new InscricaoParticipante(evento, pessoa, DateTime.Now, dtoInscricao.TipoInscricao);
                var inscParticipante = (InscricaoParticipante)novaInscricao;
                inscParticipante.AtribuirDados(dtoInscricao);

                inscParticipante.RemoverTodasAtividade();
                inscParticipante.AtribuirAtividadeOficina(dtoInscricao.Oficina, Contexto.RepositorioOficinas);
                inscParticipante.AtribuirAtividadeSalaEstudo(dtoInscricao.SalasEstudo, Contexto.RepositorioSalasEstudo);
                inscParticipante.AtribuirAtividadeDepartamento(dtoInscricao.Departamento, Contexto.RepositorioDepartamentos);

                inscParticipante.TornarPendente();

                var repInscricoes = Contexto.RepositorioInscricoes;
                repInscricoes.Incluir(inscParticipante);

                var appApresentacaoSarau = new AppApresentacaoSarau(Contexto);
                appApresentacaoSarau
                    .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(inscParticipante, dtoInscricao.Sarais);

                m_AppEmail.EnviarInscricaoRegistrada(inscParticipante);

                dto = new DTODadosConfirmacao
                {
                    EnderecoEmail = novaInscricao.Pessoa.Email,
                    IdInscricao = novaInscricao.Id
                };
            });

            return dto;
        }
    }
}
