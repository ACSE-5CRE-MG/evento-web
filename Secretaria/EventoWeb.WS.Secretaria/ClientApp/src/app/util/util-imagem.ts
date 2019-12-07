export class OperacoesImagem {
  static obterImagemOuSemImagem(imagemBase64: string): string {
    if (imagemBase64 == null || imagemBase64.trim().length == 0)
      return 'assets/semimagem.jpg';
    else
        return 'data:image/jpeg;base64,' + imagemBase64;
  }
}
