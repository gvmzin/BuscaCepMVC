using BuscaCepMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace BuscaCepMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://viacep.com.br/ws/");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Consultar(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8 || !cep.All(char.IsDigit))
            {
                var modelErro = new EnderecoViewModel { Erro = "CEP inválido. Digite 8 números." };
                return View("Index", modelErro);
            }

            var response = await _httpClient.GetAsync($"{cep}/json/");
            
            EnderecoViewModel? endereco;

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                if (content.Contains("\"erro\": true"))
                {
                    endereco = new EnderecoViewModel { Erro = "CEP não encontrado." };
                    return View("Index", endereco);
                }

                endereco = JsonSerializer.Deserialize<EnderecoViewModel>(content);
            }
            else
            {
                endereco = new EnderecoViewModel { Erro = "Ocorreu um erro ao buscar o CEP. Tente novamente." };
            }

            return View("Index", endereco);
        }
    }
}