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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventoWeb.Nucleo.Persistencia.Relatorios
{
    public class RelatorioDivisaoQuartos : IRelatorioDivisaoQuartos
    {
        public Stream Gerar(IEnumerable<Quarto> quartos)
        {
            var descricaoQuartos = new Dictionary<Boolean, String>() { { true, "Quarto Família" }, { false, "Quarto Geral" } };

            using (var stream = new MemoryStream())
            using (var escritorPDF = new PdfWriter(stream))
            using (var pdfDocumento = new PdfDocument(escritorPDF))
            using (var documento = new Document(pdfDocumento, PageSize.A4))
            {
                documento.SetMargins(10, 10, 10, 10);

                foreach (var quarto in quartos)
                {
                    var pagina = pdfDocumento.AddNewPage();
                    var pdfCanvasPag = new PdfCanvas(pagina);
                    var canvasPag = new Canvas(pagina, documento.GetPageEffectiveArea(new PageSize(pagina.GetPageSize())));

                    var fonteTitulo = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                    var cabecalho = new Paragraph();
                    cabecalho.SetFontSize(18);
                    cabecalho.SetFont(fonteTitulo);
                    cabecalho.SetTextAlignment(TextAlignment.CENTER);
                    cabecalho.Add("Quarto: " + quarto.Nome);
                    canvasPag.Add(cabecalho);

                    var fonteDescricao = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

                    var cabecalhoDescricao = new Paragraph();
                    cabecalhoDescricao.SetFontSize(12);
                    cabecalhoDescricao.SetFont(fonteDescricao);
                    cabecalhoDescricao.SetTextAlignment(TextAlignment.CENTER);
                    cabecalhoDescricao.Add(descricaoQuartos[quarto.EhFamilia] + " - " + quarto.Sexo.ToString());
                    canvasPag.Add(cabecalhoDescricao);

                    var cabecalhoTotais = new Paragraph();
                    cabecalhoTotais.SetFontSize(12);
                    cabecalhoTotais.SetFont(fonteDescricao);
                    cabecalhoTotais.SetTextAlignment(TextAlignment.CENTER);
                    cabecalhoTotais.Add("Participantes: " + quarto.Inscritos.Count().ToString());
                    canvasPag.Add(cabecalhoTotais);

                    var linha = new SolidLine(1f);
                    linha.SetColor(ColorConstants.BLACK);

                    var separador = new Paragraph();
                    separador.Add(new LineSeparator(linha));
                    separador.Add("\n");

                    var fonteListaNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);  
                    var fonteListaNegrito = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                    foreach (var participante in quarto.Inscritos.OrderBy(x => x.Inscricao.Pessoa.Nome))
                    {
                        var nome = participante.Inscricao.Pessoa.Nome;
                        PdfFont fonte = null;
                        if (participante.EhCoordenador)
                        {
                            fonte = fonteListaNegrito;
                            nome += " (Coordenador)";
                        }
                        else
                            fonte = fonteListaNormal;
                        
                        //nome = nome + " - " +
                        //    participante.Inscricao.Pessoa.CalcularIdadeEmAnos(participante.Inscricao.Evento.DataInicioEvento).ToString() +
                        //    " Ano(s) idade";
                        var paragrafoListagem = new Paragraph();
                        paragrafoListagem.SetFontSize(10);
                        paragrafoListagem.SetFont(fonte);
                        paragrafoListagem.SetTextAlignment(TextAlignment.CENTER);
                        paragrafoListagem.Add(nome);
                        canvasPag.Add(paragrafoListagem);
                    }
                }

                documento.Close();
                return new MemoryStream(stream.GetBuffer());
            }
        }
    }
}
