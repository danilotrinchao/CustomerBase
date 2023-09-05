using CustomerBase.Adapter.Adapters.Services;
using CustomerBase.Core.ApplicationService.Interfaces;
using CustomerBase.Core.Domain.Aggregates;
using CustomerBase.Core.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CustomerBase.Adapter.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    //[Authorize(Policy = "Administrators")]
    public class CostumerBaseController : Controller
    {
        private readonly ICustomerBaseService _costumerBaseService;

        public CostumerBaseController(ICustomerBaseService costumerBaseService)
        {
            _costumerBaseService= costumerBaseService;
        }

        [HttpPost]
        [Route("Autenticar")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate(Guid id)
        {
            var user = await _costumerBaseService.GetById(id);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);
            
            return new
            {
                user = user,
                token = token
            };
        }


        [HttpGet("GetClientById")]
        [Authorize]
        public async Task<ActionResult<Client>> GetById(Guid clientId)
        {
            try
            {
                var client = await _costumerBaseService.GetById(clientId);
                return Ok(client);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        
        public async Task<ActionResult> GetAll()
        {          
            var listClient = await _costumerBaseService.GetAll();
            return Ok(listClient);
        }

        [HttpPost("createClients")]
        public async Task<ActionResult> RegisterCostumer([FromBody] Client client)
        {
           var register = await _costumerBaseService.CreateClientWithAddress(client);
            return Ok(register);
        }
        
        [HttpPut("updateClients/{clientId}")]
        public async Task<ActionResult> UpdateCliente([FromBody] Client client)
        {
            var clientUpdate = await _costumerBaseService.UpdateClientWithAddress(client); 
            return Ok(clientUpdate);
        }

        [HttpDelete("clients/{clientId}")]
        public async Task<ActionResult> DeleteClientWithAddress(Guid clientId, bool deleteAddress)
        {
            var client = _costumerBaseService.DeleteClientWithAddress(clientId, deleteAddress);
            return Ok(client);
        }

        [HttpPost("addresses/{clientId}")]
        public async Task<ActionResult> CreateAddress([FromBody] Address address)
        {
            var createdAddress = await _costumerBaseService.CreateAddress(address);
            return CreatedAtAction("GetAddressById", new { addressId = createdAddress }, createdAddress);
        }

        [HttpDelete("addresses/{addressId}")]
        public async Task<ActionResult> DeleteAddress(Guid addressId)
        {
            _costumerBaseService.DeleteAddress(addressId);
            return Ok();
        }

        [HttpPut("addresses/{addressId}")]
        public async Task<ActionResult> UpdateAddress(Guid addressId, [FromBody] Address address)
        {
            var isSuccess = await _costumerBaseService.UpdateAddress(addressId, address.Country, address.State, address.City, address.Neighborhood, address.Road, address.Number, address.Complement);
            if (isSuccess)
            {
                return Ok("Endereço atualizado com sucesso.");
            }
            else
            {
                return NotFound("Endereço não encontrado.");
            }
        }
    }

}
