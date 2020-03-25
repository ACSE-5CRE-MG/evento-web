package com.eventoweb.ws.relatorios;

import com.eventoweb.ws.relatorios.motor.GeracaoRelatorioPDFJasper;
import com.eventoweb.ws.relatorios.motor.IGeracaoRelatorioPDF;
import java.util.List;
import javax.ws.rs.Consumes;
import javax.ws.rs.PUT;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

@Path("/relatorios")
public class RelatoriosController {
    
    private final IGeracaoRelatorioPDF geradorRelatorio;
    
     public RelatoriosController() {
        geradorRelatorio = new GeracaoRelatorioPDFJasper();
    }
    
    @PUT
    @Path("/rel-sarau")
    @Consumes(MediaType.APPLICATION_JSON)
    @Produces("application/pdf")
    public Response gerarListagemSarau(List<DTOApresentacaoSarau> apresentacoes) throws Exception  {
        
        byte[] relatorioPdf = geradorRelatorio.Gerar(
                obterEnderecoRelatorio(EnumRelatorio.ListagemSarau), apresentacoes);
        
        return Response
                .ok(relatorioPdf, "application/pdf")
                .build();
    }
    
    @PUT
    @Path("/rel-divisao")
    @Consumes(MediaType.APPLICATION_JSON)
    @Produces("application/pdf")
    public Response gerarDivisao(List<DTODivisao> divisoes) throws Exception  {
        
        byte[] relatorioPdf = geradorRelatorio.Gerar(
                obterEnderecoRelatorio(EnumRelatorio.RelatorioDivisao), divisoes);
        
        return Response
                .ok(relatorioPdf, "application/pdf")
                .build();
    }    
    
    private String obterEnderecoRelatorio(EnumRelatorio relatorio) {
        String localRelatorio = ConfiguracaoServico.getIntancia().getLocalRelatorios();
        
        String nomeArquivoRel = ConfiguracaoServico.getIntancia().getRelatorios().get(relatorio);
        
        return localRelatorio + "\\" + nomeArquivoRel;
    }
}
