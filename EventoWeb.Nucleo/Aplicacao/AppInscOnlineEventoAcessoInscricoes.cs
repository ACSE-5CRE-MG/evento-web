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
                            string codigo = Contexto.ServicoGeradorCodigoSeguro.GerarCodigoInscricao(inscricao);

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
            throw new NotImplementedException();
        }

        public void EnviarCodigoEmail(string identificacao, string email)
        {
            string codigo = Contexto.ServicoGeradorCodigoSeguro.GerarCodigoInscricao(identificacao);

            m_AppEmail.EnviarCodigoCriacaoInscricao(novaInscricao, codigo);
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

                new AppInscricaoInfantil(Contexto)
                    .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(inscParticipante, dtoInscricao.Criancas);

                var appApresentacaoSarau = new AppApresentacaoSarau(Contexto);
                appApresentacaoSarau
                    .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(inscParticipante, dtoInscricao.Sarais);

                foreach (var dtoCrianca in dtoInscricao.Criancas.Where(x => x.Sarais?.Count > 0))
                {
                    var crianca = repInscricoes.ObterInscricaoPeloIdEventoEInscricao(inscParticipante.Evento.Id, dtoCrianca.Id.Value);
                    appApresentacaoSarau
                        .IncluirOuAtualizarPorParticipanteSemExecucaoSegura(crianca, dtoCrianca.Sarais);
                }

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
