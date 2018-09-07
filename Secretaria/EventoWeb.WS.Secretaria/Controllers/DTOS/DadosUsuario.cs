using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventoWeb.WS.Secretaria.Controllers.DTOS
{
    public class DadosUsuario
    {
        [JsonProperty("email")]
        public String Login { get; set; }
        [JsonProperty("name")]
        public String Nome { get; set; }
        [JsonProperty("picture")]
        public Data Imagem { get; set; }
        [JsonProperty("imagem64")]
        public String Imagem64 { get; set; }
        public string TokenApi { get; set; }
    }

    public class Data
    {
        [JsonProperty("data")]
        public ImgUsuario Imagem { get; set; }
    }

    public class ImgUsuario
    {
        [JsonProperty("is_silhouette")]
        public Boolean Silhueta { get; set; }
        [JsonProperty("url")]
        public String Url { get; set; }

    }
}