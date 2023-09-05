using CostumerBase.Presentation.Mvc.Interfaces;
using CostumerBase.Presentation.Mvc.Models;
using Microsoft.AspNetCore.Mvc;


namespace CostumerBase.Presentation.Mvc.Controllers
{
    public class ClientController : Controller
    {
        
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

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var client = await _customerBaseService.GetClientById(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        
        }
        [HttpGet]
        public IActionResult Create(ClientViewModel client)
        {
            client.Id = Guid.NewGuid();
            return View("Create", client);
        }
        [HttpGet]
        public IActionResult Edit(Guid id,ClientViewModel client)
        {
            client.Id = id;
            return View("Edit", client);
        }
        public async Task<IActionResult> OnPostAsync(ClientViewModel clientViewModel)
        {
            if (ModelState.IsValid)
            {
                await _customerBaseService.CreateClient(clientViewModel);
                return RedirectToAction(nameof(Index));
            }
            return Ok(new { clientId = clientViewModel.Id });
           // return View(clientViewModel);
        }

        public async Task<IActionResult> EditClient(ClientViewModel clientViewModel, Guid id)
        {
            if (id != clientViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updated = await _customerBaseService.UpdateClientWithAddress(clientViewModel, id);
                if (!updated)
                {
                    return NotFound();
                }
               // return RedirectToAction("/Edit", new { Guid = clientViewModel.Id = id });
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
