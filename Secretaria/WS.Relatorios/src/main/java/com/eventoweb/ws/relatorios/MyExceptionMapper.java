/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.eventoweb.ws.relatorios;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import javax.ws.rs.ext.ExceptionMapper;

public class MyExceptionMapper
               implements ExceptionMapper<Exception> {

  @Override
  public Response toResponse(final Exception exception) {
      return Response.status(Response.Status.BAD_REQUEST)
                     .entity(prepareMessage(exception))
                     .type(MediaType.APPLICATION_JSON)
                     .build();
  }

  private String prepareMessage(Exception exception) {
      String dadosJson = "";
      try {
          ObjectMapper conversorJson = new ObjectMapper();
          dadosJson = conversorJson.writeValueAsString(exception);
          return dadosJson;
      } catch (JsonProcessingException ex) {
          Logger.getLogger(MyExceptionMapper.class.getName()).log(Level.SEVERE, null, ex);
      }
      
      return dadosJson;
  }
}
