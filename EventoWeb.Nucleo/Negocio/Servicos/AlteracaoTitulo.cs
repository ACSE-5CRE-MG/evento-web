using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventoWeb.Nucleo.Negocio.Entidades;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class AlteracaoTitulo
    {
        private ITitulos mRepositorio;
        private AInscricoes mRepInscricoes;
        public AlteracaoTitulo(ITitulos repositorio, AInscricoes repInscricoes)
        {
            mRepositorio = repositorio;
            mRepInscricoes = repInscricoes;
        }

        public void Alterar(Titulo titulo)
        {
            /*if (mRepInscricoes.HaInscricaoVinculadaTitulo(titulo.Id))
                throw new InvalidOperationException("Não é possível alterar um título que esta vinculado a uma Inscrição.");*/

            mRepositorio.Atualizar(titulo);
        }
    }
}
