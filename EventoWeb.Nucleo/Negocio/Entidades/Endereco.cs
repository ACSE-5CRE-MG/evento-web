using EventoWeb.Nucleo.Negocio.Excecoes;
using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Endereco
    {
        private string mCidade;
        private string mUF;
        private string mCEP;

        public Endereco(string cidade, string uf)
        {
            Cidade = cidade;
            UF = uf;
        }

        protected Endereco() { }

        public virtual string Logradouro { get; set; }

        public virtual string Numero { get; set; }

        public virtual string Bairro { get; set; }

        public virtual string Cidade 
        {
            get { return mCidade; }
            set
            {
                ValidarSeValorNuloOuVazio(value, "Cidade");
                mCidade = value;
            }
        }

        public virtual string UF 
        {
            get { return mUF; }
            set
            {
                ValidarSeValorNuloOuVazio(value, "UF");
                mUF = value;
            }
        }

        private void ValidarSeValorNuloOuVazio(String valor, String nomeCampo)
        {
            if (String.IsNullOrEmpty(valor))
                throw new ExcecaoNegocioAtributo("Endereco", nomeCampo, nomeCampo + " não foi informado");
        }

        public virtual string Complemento { get; set; }

        public virtual string CEP 
        {
            get { return mCEP; }
            set
            {
                if (!String.IsNullOrEmpty(value) && value.Trim().Length != 8)
                    throw new ExcecaoNegocioAtributo("Endereco", "CEP", "CEP deve ter 8 digitos.");

                mCEP = value;
            }
        }
    }
}
