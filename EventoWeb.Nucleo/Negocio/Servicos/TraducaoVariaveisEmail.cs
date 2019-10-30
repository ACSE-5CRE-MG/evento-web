using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class TraducaoVariaveisEmail
    {
        private IEnumerable<VariavelEmailInscricao> m_Variaveis;
        private Inscricao m_Inscricao;

        public TraducaoVariaveisEmail(IEnumerable<VariavelEmailInscricao> variaveis, Inscricao inscricao)
        {
            Variaveis = variaveis;
            Inscricao = inscricao;
        }

        public IEnumerable<VariavelEmailInscricao> Variaveis 
        {
            get => m_Variaveis;
            set
            {
                m_Variaveis = value ?? throw new ExcecaoNegocioAtributo("TraducaoVariaveisEmail", "Variaveis", "Variáveis deve ser informada");
            }
        }
        public Inscricao Inscricao
        {
            get => m_Inscricao;
            set
            {
                m_Inscricao = value ?? throw new ExcecaoNegocioAtributo("TraducaoVariaveisEmail", "Inscricao", "Inscricao deve ser informada");
            }
        }

        public string ObterValor(string variavel)
        {
            var variavelEmail = m_Variaveis.FirstOrDefault(x => x.Variavel.ToUpper() == variavel.ToUpper());
            if (variavelEmail != null)
            {
                String[] vPropriedades = variavelEmail.CampoInscricao.Split(new char[] { '.' });
                PropertyInfo vPropriedadeAtual = null;
                Object vObjetoAtual = m_Inscricao;

                if (vPropriedades.Count() == 1)
                    vPropriedadeAtual = vObjetoAtual.GetType().GetProperty(variavelEmail.CampoInscricao);
                else
                {
                    int vIndiceUltimaPropriedade = vPropriedades.Count() - 1;
                    for (int vCont = 0; vCont < vIndiceUltimaPropriedade; vCont++)
                    {
                        if (vObjetoAtual != null)
                        {
                            vPropriedadeAtual = vObjetoAtual.GetType().GetProperty(vPropriedades[vCont]);
                            vObjetoAtual = vPropriedadeAtual.GetValue(vObjetoAtual, null);
                        }
                    }
                    if (vObjetoAtual != null)
                        vPropriedadeAtual = vObjetoAtual.GetType().GetProperty(vPropriedades[vIndiceUltimaPropriedade]);
                    else
                        vPropriedadeAtual = null;
                }

                if (vPropriedadeAtual != null)
                    return (String)vPropriedadeAtual.GetValue(vObjetoAtual, null);
                else
                    return "";
            }
            else
                throw new ExcecaoNegocioAtributo("TraducaoVariaveisEmail", "variavel", "Variável não existe");
        }
    }
}
