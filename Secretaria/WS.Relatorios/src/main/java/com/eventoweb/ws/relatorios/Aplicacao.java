package com.eventoweb.ws.relatorios;

import java.io.IOException;
import java.net.URI;
import java.util.HashSet;
import java.util.Set;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.glassfish.grizzly.http.server.HttpServer;
import org.glassfish.jersey.grizzly2.httpserver.GrizzlyHttpServerFactory;
import org.glassfish.jersey.server.ResourceConfig;

public class Aplicacao {

    private static final URI BASE_URI = URI.create("http://localhost:8989/api/");
    public static final String ROOT_PATH = "relatorios";
    
    public static void main(String[] args) {
        try {
            System.out.println("\"Hello World\" Jersey Example App");
            
            final Set<Class<?>> recursos = new HashSet<>();
            recursos.add(RelatoriosController.class);
            recursos.add(MyExceptionMapper.class);

            final ResourceConfig resourceConfig = new ResourceConfig(recursos);
            final HttpServer server = GrizzlyHttpServerFactory.createHttpServer(BASE_URI, resourceConfig, false);
            Runtime.getRuntime().addShutdownHook(new Thread(() -> {
                server.shutdownNow();
            }));
            server.start();

            System.out.println(String.format("Application started.\nTry out %s%s\nStop the application using CTRL+C",
                    BASE_URI, ROOT_PATH));
            Thread.currentThread().join();
        } catch (IOException | InterruptedException ex) {
            Logger.getLogger(Aplicacao.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
}
