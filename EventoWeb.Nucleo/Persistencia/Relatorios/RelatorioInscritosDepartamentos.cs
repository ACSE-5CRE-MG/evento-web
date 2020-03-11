using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventoWeb.Nucleo.Persistencia.Relatorios
{
    public class RelatorioInscritosDepartamentos : IRelatorioInscritosDepartamentos
    {
        public Stream Gerar(IEnumerable<AtividadeInscricaoDepartamento> inscritos)
        {
            MemoryStream arquivo = null;

            using (var fluxoArquivo = new MemoryStream())
            using (var escritorPDF = new PdfWriter(fluxoArquivo))
            using (var pdfDocumento = new PdfDocument(escritorPDF))
            using (var documento = new Document(pdfDocumento, PageSize.A4))
            {
                documento.SetMargins(10, 10, 10, 10);

                foreach (var departamento in inscritos.GroupBy(x => x.DepartamentoEscolhido))
                {
                    var pagina = pdfDocumento.AddNewPage();
                    var pdfCanvasPag = new PdfCanvas(pagina);

                    var canvasPag = new Canvas(pagina, documento.GetPageEffectiveArea(PageSize.A4));

                    var fonteTitulo = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                    var cabecalho = new Paragraph();
                    cabecalho.SetFontSize(18);
                    cabecalho.SetFont(fonteTitulo);
                    cabecalho.Add("Departamento: " + departamento.Key.Nome);
                    cabecalho.SetTextAlignment(TextAlignment.CENTER);
                    canvasPag.Add(cabecalho);

                    string nomesCoordenadores = "";
                    foreach (var coordenador in departamento.Where(x => x.EhCoordenacao))
                    {
                        if (string.IsNullOrEmpty(nomesCoordenadores))
                            nomesCoordenadores = coordenador.Inscrito.Pessoa.Nome;
                        else
                            nomesCoordenadores = nomesCoordenadores + ", " + coordenador.Inscrito.Pessoa.Nome;
                    }

                    var fonteResponsaveis = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);
                    var cabecalhoResponsaveis = new Paragraph();
                    cabecalhoResponsaveis.SetFontSize(12);
                    cabecalhoResponsaveis.SetFont(fonteResponsaveis);
                    cabecalhoResponsaveis.Add("Coordenadores: " + nomesCoordenadores);
                    cabecalhoResponsaveis.SetTextAlignment(TextAlignment.CENTER);
                    canvasPag.Add(cabecalhoResponsaveis);

                    var cabecalhoTotais = new Paragraph();
                    cabecalhoTotais.SetTextAlignment(TextAlignment.CENTER);
                    cabecalhoTotais.SetFont(fonteResponsaveis);
                    cabecalhoTotais.Add("Participantes: " + departamento.Count().ToString());
                    canvasPag.Add(cabecalhoTotais);

                    var linha = new SolidLine(1f);
                    linha.SetColor(ColorConstants.BLACK);

                    var separador = new Paragraph();
                    separador.Add(new LineSeparator(linha));
                    separador.Add("\n");

                    canvasPag.Add(new LineSeparator(linha));

                    var fonteLista = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                    foreach (var participante in departamento.Where(x => !x.EhCoordenacao).OrderBy(x => x.Inscrito.Pessoa.Nome))
                    {
                        var nome = participante.Inscrito.Pessoa.Nome/* + " - " +
                            participante.Pessoa.CalcularIdadeEmAnos(participante.Evento.DataInicioEvento).ToString() +
                            " Ano(s) idade"*/ ;

                        var paragrafoListagem = new Paragraph();
                        paragrafoListagem.SetFontSize(14);
                        paragrafoListagem.SetFont(fonteLista);
                        paragrafoListagem.SetTextAlignment(TextAlignment.CENTER);
                        paragrafoListagem.Add(nome);
                        canvasPag.Add(paragrafoListagem);
                    }

                    canvasPag.Close();
                }

                documento.Close();

                arquivo = new MemoryStream(fluxoArquivo.GetBuffer());
            }

            return arquivo;
        }
    }
}
