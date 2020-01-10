using EventoWeb.Nucleo.Aplicacao;
using EventoWeb.Nucleo.Negocio.Entidades;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Events;
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
        private class CabecalhoPagina : IEventHandler
        {
            private Dictionary<bool, string> m_DescricaoQuartos;
            private PdfFont m_FonteTitulo;
            private PdfFont m_FonteDescricao;
            private IDictionary<PdfPage, Quarto> m_QuartosPagina;

            public CabecalhoPagina()
            {
                m_DescricaoQuartos = new Dictionary<Boolean, String>() { { true, "Quarto Família" }, { false, "Quarto Geral" } };
                m_FonteTitulo = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                m_FonteDescricao = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

                m_QuartosPagina = new Dictionary<PdfPage, Quarto>();
            }

            public void AdicionarQuarto(PdfPage pagina, Quarto quarto)
            {
                if (!m_QuartosPagina.ContainsKey(pagina))
                    m_QuartosPagina.Add(pagina, quarto);
            }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) @event;
                PdfPage page = docEvent.GetPage();
                Canvas canvas = new Canvas(page, page.GetPageSize());

                Quarto quartoAtual = m_QuartosPagina[page];

                var cabecalho = new Paragraph();
                cabecalho.SetFontSize(18);
                cabecalho.SetFont(m_FonteTitulo);
                cabecalho.SetTextAlignment(TextAlignment.CENTER);
                cabecalho.Add("Quarto: " + quartoAtual.Nome);
                canvas.Add(cabecalho);

                var cabecalhoDescricao = new Paragraph();
                cabecalhoDescricao.SetFontSize(12);
                cabecalhoDescricao.SetFont(m_FonteDescricao);
                cabecalhoDescricao.SetTextAlignment(TextAlignment.CENTER);
                cabecalhoDescricao.Add(m_DescricaoQuartos[quartoAtual.EhFamilia] + " - " + quartoAtual.Sexo.ToString());
                canvas.Add(cabecalhoDescricao);

                var cabecalhoTotais = new Paragraph();
                cabecalhoTotais.SetFontSize(12);
                cabecalhoTotais.SetFont(m_FonteDescricao);
                cabecalhoTotais.SetTextAlignment(TextAlignment.CENTER);
                cabecalhoTotais.Add("Participantes: " + quartoAtual.Inscritos.Count().ToString());
                canvas.Add(cabecalhoTotais);

                var linha = new SolidLine(1f);
                linha.SetColor(ColorConstants.BLACK);
                canvas.Add(new LineSeparator(linha));
            }
        }

        private PdfFont m_FonteTitulo;
        private PdfFont m_FonteDescricao;
        private Dictionary<bool, string> m_DescricaoQuartos;

        public Stream Gerar(IEnumerable<Quarto> quartos)
        {
            m_DescricaoQuartos = new Dictionary<Boolean, String>() { { true, "Quarto Família" }, { false, "Quarto Geral" } };

            m_FonteTitulo = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            m_FonteDescricao = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

            using (var stream = new MemoryStream())
            using (var escritorPDF = new PdfWriter(stream))
            using (var pdfDocumento = new PdfDocument(escritorPDF))
            using (var documento = new Document(pdfDocumento, PageSize.A4))
            {
                //var cabecalhoPagina = new CabecalhoPagina();
                //pdfDocumento.AddEventHandler(PdfDocumentEvent.END_PAGE, cabecalhoPagina);

                documento.SetMargins(10, 10, 10, 10);

                var fonteTitulo = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var fonteDescricao = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

                var fonteListaNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                var fonteListaNegrito = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                for (var indice = 0; indice < quartos.Count(); indice++)
                {
                    var quarto = quartos.ElementAt(indice);

                    GerarCabecalho(documento, quarto);

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
                        paragrafoListagem.SetFontSize(14);
                        paragrafoListagem.SetFont(fonte);
                        paragrafoListagem.SetTextAlignment(TextAlignment.CENTER);
                        paragrafoListagem.Add(nome);
                        documento.Add(paragrafoListagem);
                        
                        //cabecalhoPagina.AdicionarQuarto(pdfDocumento.GetLastPage(), quarto);
                    }

                    if (indice + 1 < quartos.Count())
                        documento.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }

                documento.Close();
                return new MemoryStream(stream.GetBuffer());
            }            
        }

        public void GerarCabecalho(Document documento, Quarto quartoAtual)
        {
            var cabecalho = new Paragraph();
            cabecalho.SetFontSize(18);
            cabecalho.SetFont(m_FonteTitulo);
            cabecalho.SetTextAlignment(TextAlignment.CENTER);
            cabecalho.Add("Quarto: " + quartoAtual.Nome);
            documento.Add(cabecalho);

            var cabecalhoDescricao = new Paragraph();
            cabecalhoDescricao.SetFontSize(12);
            cabecalhoDescricao.SetFont(m_FonteDescricao);
            cabecalhoDescricao.SetTextAlignment(TextAlignment.CENTER);
            cabecalhoDescricao.Add(m_DescricaoQuartos[quartoAtual.EhFamilia] + " - " + quartoAtual.Sexo.ToString());
            documento.Add(cabecalhoDescricao);

            var cabecalhoTotais = new Paragraph();
            cabecalhoTotais.SetFontSize(12);
            cabecalhoTotais.SetFont(m_FonteDescricao);
            cabecalhoTotais.SetTextAlignment(TextAlignment.CENTER);
            cabecalhoTotais.Add("Participantes: " + quartoAtual.Inscritos.Count().ToString());
            documento.Add(cabecalhoTotais);

            var linha = new SolidLine(1f);
            linha.SetColor(ColorConstants.BLACK);
            documento.Add(new LineSeparator(linha));
        }
    }

    public class RelDivisaoQuarto
    {
        public int IdQuarto { get; set; }
        public string NomeQuarto { get; set; }
        public string DescricaoQuarto { get; set; }
        public string DescricaoQtdeParticipantes { get; set; }
        public string NomeParticipante { get; set; }
        public bool ParticipanteEhCoordenador { get; set; }
    }
}
