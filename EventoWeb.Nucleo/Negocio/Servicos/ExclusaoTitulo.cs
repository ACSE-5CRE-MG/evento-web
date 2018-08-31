using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Servicos
{
    public class ExclusaoTitulo
    {
        private ITitulos mRepositorio;
        private AInscricoes mRepInscricoes;
        public ExclusaoTitulo(ITitulos repositorio, AInscricoes repInscricoes)
        {
            mRepositorio = repositorio;
            mRepInscricoes = repInscricoes;
        }

        public void Excluir(Titulo titulo)
        {
            if (titulo.Situacao != TipoSituacaoTitulo.Aberto)
                throw new InvalidOperationException("Há parcelas neste título que foram liquidadas, por isso é impossível excluí-lo.");

            if (mRepInscricoes.HaInscricaoVinculadaTitulo(titulo.Id))
                throw new InvalidOperationException("Não é possível excluir um título que esta vinculado a uma Inscrição.");

            mRepositorio.Excluir(titulo);
        }
    }
}
