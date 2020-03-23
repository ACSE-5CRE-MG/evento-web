/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.eventoweb.ws.relatorios.motor;

import java.io.OutputStream;

public interface IGeracaoRelatorioPDF {    
    <TDadosRelatorio> byte[] Gerar(String enderecoRelatorio, TDadosRelatorio dados) throws Exception;
}
