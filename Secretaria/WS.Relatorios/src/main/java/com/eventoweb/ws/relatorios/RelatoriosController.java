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
    @Path("/relsarau")
    @Consumes(MediaType.APPLICATION_JSON)
    @Produces("application/pdf")
    public Response getSarau(List<DTOApresentacaoSarau> apresentacoes) throws Exception  {
        
        byte[] relatorioPdf = geradorRelatorio.Gerar(
                "D:\\Projetos\\EventoWeb-Git\\Secretaria\\EventoWeb.Relatorios\\ListagemSarau.jasper", apresentacoes);
        
        return Response
                .ok(relatorioPdf, "application/pdf")
                .build();
    }
}
