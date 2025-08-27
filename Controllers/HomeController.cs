using BuscaCepMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace BuscaCepMvc.Controllers
{
    public class HomeController : Controller
    {
        // HttpClient é a classe que usamos para fazer requisições HTTP (chamar APIs)
        private readonly HttpClient _httpClient;

        // Injeção de dependência: o .NET nos fornece uma instância do HttpClient
        public HomeController()
        {
            _httpClient = new HttpClient();
            // A URL base da API que vamos consumir
            _httpClient.BaseAddress = new Uri("https://viacep.com.br/ws/");
        }

        // Action que exibe a tela inicial
        public IActionResult Index()
        {
            return View();
        }

        // Action que será chamada pelo formulário de busca
        [HttpPost]
        public async Task<IActionResult> Consultar(string cep)
        {
            // Validação simples do CEP
            if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8 || !cep.All(char.IsDigit))
            {
                // Se o CEP for inválido, retornamos para a View Index com uma mensagem de erro
                var modelErro = new EnderecoViewModel { Erro = "CEP inválido. Digite 8 números." };
                return View("Index", modelErro);
            }

            // Fazendo a chamada GET para a API
            // Ex: https://viacep.com.br/ws/01001000/json/
            var response = await _httpClient.GetAsync($"{cep}/json/");
            
            EnderecoViewModel? endereco;

            if (response.IsSuccessStatusCode)
            {
                // Lendo o conteúdo da resposta (o JSON)
                string content = await response.Content.ReadAsStringAsync();

                // Verificando se a API retornou um erro interno
                if (content.Contains("\"erro\": true"))
                {
                    endereco = new EnderecoViewModel { Erro = "CEP não encontrado." };
                    return View("Index", endereco);
                }

                // Desserializando o JSON para o nosso objeto EnderecoViewModel
                endereco = JsonSerializer.Deserialize<EnderecoViewModel>(content);
            }
            else
            {
                // Se a API retornou um erro (ex: 404, 500), criamos um modelo de erro
                endereco = new EnderecoViewModel { Erro = "Ocorreu um erro ao buscar o CEP. Tente novamente." };
            }

            // Retornamos para a View "Index" passando o objeto "endereco" como Model
            return View("Index", endereco);
        }
    }
}