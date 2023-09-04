using CostumerBase.Presentation.Mvc.Interfaces;
using CostumerBase.Presentation.Mvc.Models;
using Microsoft.AspNetCore.Mvc;


namespace CostumerBase.Presentation.Mvc.Controllers
{
    public class ClientController : Controller
    {
        private readonly ILogger<ClientController> _logger;
        private readonly ICustomerBaseService _customerBaseService;
        public ClientController(ICustomerBaseService customerBaseService)
        {
            _customerBaseService = customerBaseService;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _customerBaseService.GetClients();
            return View(clientes);
        }
        [HttpPost]
        public async Task<JsonResult> AutenticarCliente(Guid clientId)
        {
            var result = await _customerBaseService.AuthenticateClient(clientId);
            return Json(result);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var cliente = await _customerBaseService.GetClientById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClient(ClientViewModel clientViewModel)
        {
            if (ModelState.IsValid)
            {
                await _customerBaseService.CreateClientWithAddress(clientViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(clientViewModel);
        }

        public async Task<IActionResult> GetClientById(Guid id)
        {
            var cliente = await _customerBaseService.GetClientById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClient(Guid id, ClientViewModel clientViewModel, bool updateAddress)
        {
            if (id != clientViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updated = await _customerBaseService.UpdateClientWithAddress(clientViewModel, updateAddress, id);
                if (!updated)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(clientViewModel);
        }

        public async Task<IActionResult> DeleteClient(Guid id)
        {
            var cliente = await _customerBaseService.GetClientById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var success = await _customerBaseService.DeleteClientWithAddress(id, true);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               // _customerBaseService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
