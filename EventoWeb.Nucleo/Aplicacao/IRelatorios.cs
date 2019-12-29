using EventoWeb.Nucleo.Negocio.Entidades;
using System.Collections.Generic;
using System.IO;

namespace EventoWeb.Nucleo.Aplicacao
{
    public interface IRelatorios
    {
        IRelatorioDivisaoSalasEstudo RelatorioDivisaoSalasEstudo { get; }
    }

    public interface IRelatorioDivisaoSalasEstudo
    {
        Stream Gerar(IEnumerable<SalaEstudo> salas, IList<AtividadeInscricaoSalaEstudoCoordenacao> coordenadores);
    }
}
