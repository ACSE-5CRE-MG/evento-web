package com.eventoweb.ws.relatorios;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.HashMap;
import java.util.Map;
import java.util.Properties;

public class ConfiguracaoServico {
    
    private static IConfiguracoes configuracoes;
    
    public static synchronized void configurar() throws IOException, FileNotFoundException {
        if (configuracoes == null)
        {
            Map<EnumRelatorio, String> relatorios = new HashMap<>();
            relatorios.put(EnumRelatorio.ListagemSarau, "ListagemSarau.jasper");
            relatorios.put(EnumRelatorio.RelatorioDivisao, "RelatorioDivisao.jasper");

            Properties p = new Properties();
            p.load(new FileInputStream(
                    ConfiguracaoServico.class.getProtectionDomain().getCodeSource().getLocation().getPath() + 
                            "configuracao.ini"));

            configuracoes = new ConfiguracoesImpl(p.getProperty("endereco_rel"), relatorios);
        }
    }  
    
    public static IConfiguracoes getIntancia() {
        return configuracoes;
    }
}
