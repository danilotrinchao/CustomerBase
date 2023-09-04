using CostumerBase.Presentation.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CostumerBase.Presentation.Mvc.Controllers
{
    public class ClientController : Controller
    {
        private readonly ILogger<ClientController> _logger;
        public ClientController(ILogger<ClientController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Client()
        {
            // Crie uma instância do modelo ClientViewModel e popule-a com os dados
            var model = new ClientViewModel
            {
                // Preencha as propriedades do modelo com os dados necessários
                Id = 1,
                Name = "Exemplo de Cliente",
                Logotipo = "logotipo.png",
                Email = "cliente@example.com",
                Addresses = new List<AddressViewModel>
                {
                    new AddressViewModel
                    {
                        Country = "País",
                        City = "Cidade",
                        State = "Estado",
                        // Preencha outras propriedades do endereço
                    }
                    // Adicione mais endereços, se necessário
                }
            };
           
            // Retorne a página Razor com o modelo populado
            return View(model);
        }
    }
}
