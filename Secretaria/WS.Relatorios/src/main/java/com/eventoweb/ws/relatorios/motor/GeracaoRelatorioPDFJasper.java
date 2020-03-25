/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.eventoweb.ws.relatorios.motor;

import com.fasterxml.jackson.databind.ObjectMapper;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;
import java.util.logging.Level;
import java.util.logging.Logger;
import net.sf.jasperreports.engine.JRDataSource;
import net.sf.jasperreports.engine.JRParameter;
import net.sf.jasperreports.engine.JasperFillManager;
import net.sf.jasperreports.engine.JasperPrint;
import net.sf.jasperreports.engine.data.JsonDataSource;
import net.sf.jasperreports.engine.export.JRPdfExporter;
import net.sf.jasperreports.engine.query.JsonQueryExecuterFactory;
import net.sf.jasperreports.export.SimpleExporterInput;
import net.sf.jasperreports.export.SimpleOutputStreamExporterOutput;

public class GeracaoRelatorioPDFJasper implements IGeracaoRelatorioPDF {

    @Override
    public <TDadosRelatorio> byte[] Gerar(String enderecoRelatorio, TDadosRelatorio dados) {
        
        try {
            File vArquivoRelatorio = new File(enderecoRelatorio);
            
            Map params = new HashMap();
            params.put(JsonQueryExecuterFactory.JSON_DATE_PATTERN, "yyyy-MM-dd");
            params.put(JsonQueryExecuterFactory.JSON_NUMBER_PATTERN, "#,##0.##");
            params.put(JsonQueryExecuterFactory.JSON_LOCALE, Locale.ENGLISH);
            params.put(JRParameter.REPORT_LOCALE, Locale.US);
            
            ObjectMapper conversorJson = new ObjectMapper();
            String dadosJson = conversorJson.writeValueAsString(dados);
            
            JRDataSource fonteDadosJson = new JsonDataSource(
                    new ByteArrayInputStream(dadosJson.getBytes("UTF-8")));
            JasperPrint relatorio = JasperFillManager.fillReport(new FileInputStream(vArquivoRelatorio),
                    params, fonteDadosJson);
            
            ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
            
            JRPdfExporter conversorJasperPDF = new JRPdfExporter();
            conversorJasperPDF.setExporterInput(new SimpleExporterInput(relatorio));
            conversorJasperPDF.setExporterOutput(new SimpleOutputStreamExporterOutput(outputStream));
            conversorJasperPDF.exportReport();
            
            return outputStream.toByteArray();
        } catch (Exception ex) {
            Logger.getLogger(GeracaoRelatorioPDFJasper.class.getName()).log(Level.SEVERE, null, ex);
            return null;
        }        
    }    
}
