using EventoWeb.Nucleo.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

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
                            var repCodigo = Contexto.RepositorioCodigosAcessoInscricao;
                            repCodigo.ExcluirCodigosVencidos();

                            var codigoAcesso = repCodigo.ObterIdInscricao(idInscricao);
                            if (codigoAcesso != null)
                            {
                                // Envia email
                            }
                            else
                            {
                                var codigo = "";
                                do
                                {
                                    codigo = Contexto.ServicoGeradorCodigoSeguro.GerarCodigo5Caracteres();
                                } while (repCodigo.ObterPeloCodigo(codigo) != null);
                                
                                codigoAcesso = new CodigoAcessoInscricao(codigo, inscricao, DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59));
                                repCodigo.Incluir(codigoAcesso);
                                //Envia email
                            }
                        }
                    }
                }
                else
                    dto.Resultado = EnumResultadoEnvio.IdentificacaoInvalida;
            });

            return dto;
        }
    }
}
