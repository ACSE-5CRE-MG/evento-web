// See https://aka.ms/new-console-template for more information
using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Persistencia;
using EventoWeb.Nucleo.Persistencia.Comunicacao;

var sessionFactory = new ConfiguracaoNHibernate().GerarFabricaSessao();
var contexto = new Contexto(sessionFactory.OpenSession());

contexto.IniciarTransacao();
try
{
    var confEmail = contexto.RepositorioConfiguracoesEmail.Obter(11);
    var inscricoes = contexto.RepositorioInscricoes.ListarTodasPorEventoESituacao(11, EventoWeb.Nucleo.Negocio.Entidades.EnumSituacaoInscricao.Aceita);

    var bytes = File.ReadAllBytes("D:\\Mensagens Mediúnicas CEOMG 2023.pdf");
    var base64 = Convert.ToBase64String(bytes);

    var servicoEmail = new ServicoEmail()
    {
        Configuracao = confEmail,
    };


    foreach(var inscrito  in inscricoes)
    {
        servicoEmail.Enviar(
            new Email()
            {
                Assunto = "41º CEOMG - Avaliação CEOMG",
                Conteudo = "<p>Ol&aacute; querida(o) amiga(o)!!!</p>\r\n\r\n<p>Enviamos esta mensagem com <strong>tr&ecirc;s finalidades</strong>: <em>divulgar as mensagens medi&uacute;nicas recebidas</em> durante a <strong>41&ordm; CEOMG</strong>, <em>pedir que voc&ecirc;, dentro do poss&iacute;vel, possa nos responder um question&aacute;rio online com o prop&oacute;sito de avaliar o encontro e nos permitir melhor&aacute;-lo,</em> e o <em>link do drive com as fotos do evento</em> para voc&ecirc; poder matar saudades desses momentos.</p>\r\n\r\n<p>Para isso, <strong>em anexo, esta uma arquivo com as mensagens medi&uacute;nicas</strong> e abaixo segue os links para avalia&ccedil;&atilde;o e do drive de fotos.</p>\r\n\r\n<p><strong>Link da avalia&ccedil;&atilde;o do encontro: <a href=\"https://forms.gle/6gQpbykBcvGA3naQ8\">https://forms.gle/6gQpbykBcvGA3naQ8</a></strong></p>\r\n\r\n<p><strong>Link do drive de fotos: <a href=\"https://drive.google.com/drive/folders/1SiRXUbL7DlTX5ZJYebDMgWQD7Cl3gRaU?usp=sharing\">https://drive.google.com/drive/folders/1SiRXUbL7DlTX5ZJYebDMgWQD7Cl3gRaU?usp=sharing</a></strong></p>\r\n\r\n<p>Um grande abra&ccedil;o fraterno!!</p>\r\n\r\n<p><em><strong>Equipe CEOMG</strong></em></p>\r\n",
                Endereco = inscrito.Pessoa.Email,
                Anexos = new List<AnexoEmail>() { new AnexoEmail("Mensagens Mediúnicas CEOMG 2023.pdf", base64)  }
            }
        );

        Console.WriteLine($"Enviado para {inscrito.Pessoa.Nome}");
    }

    contexto.SalvarTransacao();
}
catch(Exception ex)
{
    contexto.CancelarTransacao();
    Console.WriteLine(ex.Message);
}

Console.WriteLine("Terminado. Aperte qualquer tecla para fechar o programa");
Console.ReadLine();

