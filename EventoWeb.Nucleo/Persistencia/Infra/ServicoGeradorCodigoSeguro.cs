using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Infra
{
    public class ServicoGeradorCodigoSeguro : IServicoGeradorCodigoSeguro
    {
        private readonly ACodigosAcessoInscricao m_RepCodigosAcessoInscricao;

        public ServicoGeradorCodigoSeguro(ACodigosAcessoInscricao repCodigosAcessoInscricao)
        {
            m_RepCodigosAcessoInscricao = repCodigosAcessoInscricao;
        }

        public string GerarCodigo5Caracteres()
        {
            return GetUniqueToken(5);
        }

        public string GerarCodigoInscricao(Inscricao inscricao)
        {
            m_RepCodigosAcessoInscricao.ExcluirCodigosVencidos();

            var codigoAcesso = m_RepCodigosAcessoInscricao.ObterIdInscricao(inscricao.Id);
            if (codigoAcesso != null)
                return codigoAcesso.Codigo;
            else
            {
                string codigo;
                do
                {
                    codigo = GerarCodigo5Caracteres();
                } while (m_RepCodigosAcessoInscricao.ObterPeloCodigo(codigo) != null);

                codigoAcesso = new CodigoAcessoInscricao(codigo, inscricao, DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59));
                m_RepCodigosAcessoInscricao.Incluir(codigoAcesso);

                return codigo;
            }
        }

        //https://blog.bitscry.com/2018/04/13/cryptographically-secure-random-string/
        private string GetUniqueToken(int length)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];

                // If chars.Length isn't a power of 2 then there is a bias if we simply use the modulus operator. The first characters of chars will be more probable than the last ones.
                // buffer used if we encounter an unusable random byte. We will regenerate it in this buffer
                byte[] buffer = null;

                // Maximum random number that can be used without introducing a bias
                int maxRandom = byte.MaxValue - ((byte.MaxValue + 1) % chars.Length);

                crypto.GetBytes(data);

                char[] result = new char[length];

                for (int i = 0; i < length; i++)
                {
                    byte value = data[i];

                    while (value > maxRandom)
                    {
                        if (buffer == null)
                        {
                            buffer = new byte[1];
                        }

                        crypto.GetBytes(buffer);
                        value = buffer[0];
                    }

                    result[i] = chars[value % chars.Length];
                }

                return new string(result);
            }
        }
    }
}
