using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace BuscaCepMvc.Models
{
    public class EnderecoViewModel
    {
        // Usamos o atributo [JsonPropertyName] para mapear o nome do campo JSON
        // para a nossa propriedade em C#, mesmo que tenham nomes diferentes.
        // Neste caso, eles são iguais, mas é uma boa prática.
        [JsonPropertyName("cep")]
        public string? Cep { get; set; }

        [JsonPropertyName("logradouro")]
        public string? Logradouro { get; set; }

        [JsonPropertyName("bairro")]
        public string? Bairro { get; set; }

        [JsonPropertyName("localidade")]
        public string? Localidade { get; set; }

        [JsonPropertyName("uf")]
        public string? Uf { get; set; }

        // Propriedade extra para controlar mensagens de erro na tela
        public string? Erro { get; set; }
    }
}