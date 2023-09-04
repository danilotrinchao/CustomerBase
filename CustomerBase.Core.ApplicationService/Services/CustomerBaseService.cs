using CustomerBase.Core.ApplicationService.Interfaces;
using CustomerBase.Core.Domain;
using CustomerBase.Core.Domain.Aggregates;
using CustomerBase.Core.Domain.Entities;
using CustomerBase.Core.Domain.Port;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomerBase.Core.ApplicationService.Services
{
    public class CustomerBaseService : ICustomerBaseService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAddressRepository _addressRepository;
        public CustomerBaseService(IClientRepository clientRepository, IAddressRepository addressRepository)
        {
            _clientRepository = clientRepository;
            _addressRepository = addressRepository;
        }

        public async Task<Guid> CreateClientWithAddress(Client client)
        {
            try
            {
                var existingClient = _clientRepository.GetClientByEmail(client.Email);
                if (existingClient != null)
                {
                    throw new InvalidOperationException("Já existe um cliente utilizando este e-mail.");
                }

                _clientRepository.InsertClient(client.Id, client.NameClient, client.Email, client.Logo);

                if (client.AddressClient != null && client.AddressClient.Count > 0)
                {
                    foreach (var address in client.AddressClient)
                    {
                        address.ClientId = client.Id;
                        _addressRepository.InsertAddress(address.Id, address.Country, address.State, address.City, address.Neighborhood, address.Road, address.Number, address.Complement, address.ClientId);
                    }
                }

                return client.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao criar o cliente.", ex);
            }
        }
        public async Task<Guid> CreateAddress(Address address)
        {
            try
            {
                var existingAddress = _addressRepository.GetById(address.Id);
                if (existingAddress != null)
                {
                    throw new InvalidOperationException("Este endereço já foi cadastrado.");
                }
                else
                {
                    _addressRepository.InsertAddress(address.Id, address.Country, address.State, address.City, address.Neighborhood, address.Road, address.Number, address.Complement, address.ClientId);
                }

                return address.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<bool> DeleteClientWithAddress(Guid id, bool deleteAddress)
        {
            try
            {
                var existingClient = await _clientRepository.GetById(id);

                if (existingClient != null)
                {
                    _clientRepository.DeleteClient(id, deleteAddress);

                    return true;
                }

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteAddress(Guid id)
        {
            var existingClient = _clientRepository.GetById(id);

            if (existingClient != null)
                _addressRepository.DeleteAddress(id);
        }

        public async Task<bool> UpdateClientWithAddress(Client client, bool updateAddress, Guid? addressId)
        {
            try
            {
                var isSuccess = false;
                var existingClient = await _clientRepository.GetById(client.Id);

                if (existingClient != null)
                {
                    // Atualize os dados do cliente
                    _clientRepository.UpdateClient(client.Id, client.NameClient, client.Email, client.Logo);
                    isSuccess = true;
                }

                if (updateAddress && addressId.HasValue)
                {
                    // Atualize o endereço se o cliente existir e o ID do endereço estiver especificado
                    var address = await _addressRepository.GetById((Guid)addressId);
                    if (address != null)
                    {
                        _addressRepository.UpdateAddress(address.Id, address.Country, address.State, address.City, address.Neighborhood, address.Road, address.Number, address.Complement);
                        isSuccess = true;
                    }
                }

                return isSuccess;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateAddress(Guid addressId, string country, string state, string city, string neighborhood, string road, string number, string complement)
        {
            try
            {
                var existingAddress =  _addressRepository.GetAddressById(addressId);

                if (existingAddress != null)
                {
                    _addressRepository.UpdateAddress(addressId, country, state, city, neighborhood, road, number, complement);
                    return true;
                }

                return false; 
            }
            catch (Exception)
            {
                throw; 
            }
        }


        public async Task<List<Client>> GetAll()
        {
            try
            {
               var clientsAll =  await _clientRepository.GetAll();
                foreach (var item in clientsAll)
                {
                    item.AddressClient = _addressRepository.GetAddressesByClient(item.Id);
                }
                return clientsAll;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Client> GetById(Guid id)
        {
            try
            {
                var existingClient = await _clientRepository.GetById(id);
                if(existingClient != null)
                {
                    existingClient.AddressClient = _addressRepository.GetAddressesByClient(id);
                    return existingClient;
                }
                else
                    throw new InvalidOperationException("Cliente não encontrado.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}
