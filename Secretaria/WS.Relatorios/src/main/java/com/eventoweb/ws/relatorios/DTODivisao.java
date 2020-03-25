package com.eventoweb.ws.relatorios;

import com.fasterxml.jackson.annotation.JsonProperty;

public class DTODivisao {
    
    private int idDivisao;
    private String nomeDivisao;
    private String descricaoDivisao;
    private String coordenadores;  
    private String nomeInscrito;
    private int totalParticipantes;

    @JsonProperty("idDivisao")
    public int getIdDivisao() {
        return idDivisao;
    }

    public void setIdDivisao(int idDivisao) {
        this.idDivisao = idDivisao;
    }

    @JsonProperty("nomeDivisao")
    public String getNomeDivisao() {
        return nomeDivisao;
    }

    public void setNomeDivisao(String nomeDivisao) {
        this.nomeDivisao = nomeDivisao;
    }

    @JsonProperty("coordenadores")
    public String getCoordenadores() {
        return coordenadores;
    }

    public void setCoordenadores(String coordenadores) {
        this.coordenadores = coordenadores;
    }

    @JsonProperty("nomeInscrito")
    public String getNomeInscrito() {
        return nomeInscrito;
    }

    public void setNomeInscrito(String nomeInscrito) {
        this.nomeInscrito = nomeInscrito;
    }

    public int getTotalParticipantes() {
        return totalParticipantes;
    }

    @JsonProperty("totalParticipantes")
    public void setTotalParticipantes(int totalParticipantes) {
        this.totalParticipantes = totalParticipantes;
    }

    @JsonProperty("descricaoDivisao")
    public String getDescricaoDivisao() {
        return descricaoDivisao;
    }

    public void setDescricaoDivisao(String descricaoDivisao) {
        this.descricaoDivisao = descricaoDivisao;
    }
}
