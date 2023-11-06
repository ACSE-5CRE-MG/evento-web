﻿using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Security.Cryptography;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class SenhaUsuario
    {
        private String m_Senha;

        private const int NUM_MIN_CARC_SENHA = 3;

        private SenhaUsuario() { }

        public SenhaUsuario(String senha, String senhaRep)
        {
            AlterarSenha(senha, senhaRep);
        }

        public Boolean EhIgual(String senha)
        {
            return m_Senha.ToUpper() == CodificarSenha(senha).ToUpper();
        }

        public void AlterarSenha(String senha, String senhaRep)
        {
            ValidarSenha(senha, senhaRep);
            m_Senha = CodificarSenha(senha);
        }

        protected virtual void ValidarSenha(String senha, String senhaRep)
        {
            if (string.IsNullOrWhiteSpace(senha))
                throw new ExcecaoNegocioAtributo(nameof(SenhaUsuario), nameof(senha), "A senha não pode ser nula ou vazia.");

            if (string.IsNullOrWhiteSpace(senhaRep))
                throw new ExcecaoNegocioAtributo(nameof(SenhaUsuario), nameof(senhaRep), "A confirmação de senha não pode ser nula ou vazia.");

            if (senha != senhaRep)
                throw new ExcecaoNegocioAtributo(nameof(SenhaUsuario), nameof(senha), "A senha e a confirmação da senha devem ser iguais.");
            else if (senha.Length < NUM_MIN_CARC_SENHA)
                throw new ExcecaoNegocioAtributo(nameof(SenhaUsuario), nameof(senha),
                    String.Format("A senha deve ter no mínimo {0:d} caracteres.", NUM_MIN_CARC_SENHA));
        }

        private String CodificarSenha(String pSenha)
        {
            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(pSenha);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));

            return sb.ToString();
        }
    }
}
