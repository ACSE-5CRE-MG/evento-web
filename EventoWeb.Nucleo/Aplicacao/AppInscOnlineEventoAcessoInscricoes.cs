using EventoWeb.Nucleo.Negocio.Entidades;
using System;

namespace EventoWeb.Nucleo.Aplicacao
{
    public class AppInscOnlineEventoAcessoInscricoes : AppBase
    {
        public AppInscOnlineEventoAcessoInscricoes(IContexto contexto)
            : base(contexto) { }

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
                int idInscricao = -1;

                if (identificacao.Length == 10 &&
                    identificacao.Substring(0, 4) == "INSC" &&
                    int.TryParse(identificacao.Substring(4, 6), out idInscricao))
                {
                    var inscricao = Contexto.RepositorioInscricoes.ObterInscricaoPeloId(idInscricao);

                    if (inscricao != null)
                    {
                        dto.IdInscricao = inscricao.Id;
                        if (inscricao.Evento.PeriodoInscricaoOnLine.DataFinal < DateTime.Now || inscricao.Evento.PeriodoInscricaoOnLine.DataInicial > DateTime.Now)
                            dto.Resultado = EnumResultadoEnvio.EventoEncerradoInscricao;
                        else
                            dto.Resultado = EnumResultadoEnvio.InscricaoOK;

                        if (dto.Resultado == EnumResultadoEnvio.InscricaoOK)
                        {
                            string codigo = Contexto.ServicoGeradorCodigoSeguro.GerarCodigoInscricao(inscricao);
                            Contexto.ServicoEmail.EnviarEmailCodigoInscricao(inscricao, codigo);
                        }
                    }
                    else
                        dto.Resultado = EnumResultadoEnvio.InscricaoNaoEncontrada;
                }
                else
                    dto.Resultado = EnumResultadoEnvio.IdentificacaoInvalida;
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
                Contexto.ServicoEmail.EnviarEmailCodigoInscricao(novaInscricao, codigo);

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
