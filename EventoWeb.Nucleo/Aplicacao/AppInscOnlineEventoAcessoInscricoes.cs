using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using System;

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
                    dtoInscricao = new DTOBasicoInscricao()
                    {
                        Email = inscricao.Pessoa.Email,
                        IdEvento = inscricao.Evento.Id,
                        IdInscricao = inscricao.Id,
                        NomeEvento = inscricao.Evento.Nome,
                        NomeInscrito = inscricao.Pessoa.Nome
                    };
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
                    int idInscricao = new AppInscOnLineCodigoInscricao().ExtrarId(identificacao);
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

        public DTODadosConfirmacao CriarInscricao(int idEvento, DTODadosCriarInscricao dadosInscricao)
        {
            DTODadosConfirmacao dto = null;
            ExecutarSeguramente(() =>
            {
                var evento = Contexto.RepositorioEventos.ObterEventoPeloId(idEvento);
                if (evento.PeriodoInscricaoOnLine.DataFinal < DateTime.Now || evento.PeriodoInscricaoOnLine.DataInicial > DateTime.Now)
                    throw new ExcecaoAplicacao("AppInscOnlineEventoAcessoInscricoes", "Evento encerrado");


                var pessoa = new Pessoa(dadosInscricao.Nome,
                    new Endereco(dadosInscricao.Cidade, dadosInscricao.UF), dadosInscricao.DataNascimento,
                    dadosInscricao.Sexo, dadosInscricao.Email);

                var novaInscricao = new InscricaoParticipante(evento, pessoa, DateTime.Now, dadosInscricao.TipoInscricao);
                Contexto.RepositorioInscricoes.Incluir(novaInscricao);

                string codigo = Contexto.ServicoGeradorCodigoSeguro.GerarCodigoInscricao(novaInscricao);

                m_AppEmail.EnviarCodigoCriacaoInscricao(novaInscricao, codigo);

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
