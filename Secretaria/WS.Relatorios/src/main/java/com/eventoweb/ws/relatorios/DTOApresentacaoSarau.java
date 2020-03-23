package com.eventoweb.ws.relatorios;

import com.fasterxml.jackson.annotation.JsonProperty;

public class DTOApresentacaoSarau {
    private String nomeEvento;
    private int tempoSarauMin;
    private int idApresentacao;
    private String tipoApresentacao;
    private int duracaoApresentacaoMin;
    private String inscritos;  
    private int idEvento;

    @JsonProperty("nomeEvento")
    public String getNomeEvento() {
        return nomeEvento;
    }

    public void setNomeEvento(String nomeEvento) {
        this.nomeEvento = nomeEvento;
    }

    @JsonProperty("tempoSarauMin")
    public int getTempoSarauMin() {
        return tempoSarauMin;
    }

    public void setTempoSarauMin(int tempoSarauMin) {
        this.tempoSarauMin = tempoSarauMin;
    }

    @JsonProperty("idApresentacao")
    public int getIdApresentacao() {
        return idApresentacao;
    }

   public void setIdApresentacao(int idApresentacao) {
        this.idApresentacao = idApresentacao;
    }

    @JsonProperty("tipoApresentacao")
    public String getTipoApresentacao() {
        return tipoApresentacao;
    }
    
    public void setTipoApresentacao(String tipoApresentacao) {
        this.tipoApresentacao = tipoApresentacao;
    }

    @JsonProperty("duracaoApresentacaoMin")
    public int getDuracaoApresentacaoMin() {
        return duracaoApresentacaoMin;
    }
    
    public void setDuracaoApresentacaoMin(int duracaoApresentacaoMin) {
        this.duracaoApresentacaoMin = duracaoApresentacaoMin;
    }

    @JsonProperty("inscritos")
    public String getInscritos() {
        return inscritos;
    }

    public void setInscritos(String inscritos) {
        this.inscritos = inscritos;
    }

    @JsonProperty("idEvento")
    public int getIdEvento() {
        return idEvento;
    }

    public void setIdEvento(int idEvento) {
        this.idEvento = idEvento;
    }
}
