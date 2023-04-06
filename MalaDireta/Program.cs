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

    var servicoEmail = new ServicoEmail()
    {
        Configuracao = confEmail,
    };
    
    foreach(var inscrito  in inscricoes)
    {
        servicoEmail.Enviar(
            new Email()
            {
                Assunto = "41º CEOMG - É amanhã o encontro, prepare-se!!!!",
                Conteudo = "<div dir=\"ltr\">Olá! Minhas companheiras e companheiros de vida💓🌻<br><br>Nossa deliciosa 41º CEOMG está chegando! Que delícia e que saudades!<br><br>Para você passar o encontro com mais tranquilidade vamos deixar aqui alguns lembretes importantes!!!!<br><br>Leve na sua mala!<br><br>💓 Afeto<br>▶️ Repelente<br>▶️ Desodorante<br>▶️ Escova de Dentes<div>▶️&nbsp;Sabonete<br>▶️ Colchão <br>▶️ Roupa de cama</div><div>▶️&nbsp;Travesseiro<br>▶️ Se seu colchão for de ar, um pano para colocar embaixo ( isso evita furos)</div><div>▶️ Se você sente frio, leve uma \"brusinha\" meio termo.<br>▶️ Garrafinha de água (para ir enchendo pelos bebedouros da escola)<br>😁Bom humor <br><br>🗺️ &nbsp;Endereço da Escola: Av. Cel. Pacífico Pinto, 221 - Fausto Pinto da Fonseca, Nova Serrana <br>🚙 Os veículos poderão estacionar no ABC ( que está em frente), se precisar, quando chegar procure a recepção/coordenação do encontro para indicação da área delimitada.<br></div><div><br></div><div>Que tenhamos um excelente encontro, que será temperado pela vibração do teu coração, que o Cristo permaneça vivo a nos iluminar e até breve!💓💓💓🌻<br><div><br></div></div><div>Abraços quentinhos, coordenação e Equipe de Secretaria CEOMG!<br></div></div>",
                Endereco = inscrito.Pessoa.Email
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

