using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class Endereco
    {
        private string mLogradouro;
        private string mNumero;
        private string mBairro;
        private string mCidade;
        private string mUF;
        private string mComplemento;
        private string mCEP;

        public Endereco(string logradouro, string numero, string bairro, string cidade, string uf)
        {
            Logradouro = logradouro;
            Numero = numero;
            Bairro = bairro;
            Cidade = cidade;
            UF = uf;
        }

        protected Endereco() { }

        public virtual string Logradouro 
        {
            get { return mLogradouro; }
            set
            {
                ValidarSeValorNuloOuVazio(value, "Logradouro");
                mLogradouro = value;
            }
        }

        public virtual string Numero 
        {
            get { return mNumero; }
            set
            {
                ValidarSeValorNuloOuVazio(value, "Numero");
                mNumero = value;
            }
        }

        public virtual string Bairro 
        {
            get { return mBairro; }
            set
            {
                ValidarSeValorNuloOuVazio(value, "Bairro");
                mBairro = value;
            }
        }

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
            if (valor == null)
                throw new ArgumentNullException(nomeCampo);

            if (valor.Trim().Length == 0)
                throw new ArgumentException("Valor vazio", nomeCampo);

        }

        public virtual string Complemento 
        {
            get { return mComplemento; }
            set { mComplemento = value; }
        }

        public virtual string CEP 
        {
            get { return mCEP; }
            set
            {
                if (!String.IsNullOrEmpty(value) && value.Trim().Length != 8)
                    throw new ArgumentException("CEP deve ter 8 digitos.", "CEP");

                mCEP = value;
            }
        }
    }
}
