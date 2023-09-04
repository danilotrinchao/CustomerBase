using CostumerBase.Presentation.Mvc.Models;
using CostumerBase.Presentation.Mvc.Models.Reponse;

namespace CostumerBase.Presentation.Mvc.Interfaces
{
    public interface ICustomerBaseService
    {

        Task<List<ClientViewModel>> GetClients();
        Task<ClientViewModel> GetClientById(Guid id);
        Task<Guid> CreateClientWithAddress(ClientViewModel client);
        Task<bool> UpdateClientWithAddress(ClientViewModel client, bool updateAddress, Guid? addressId);
        Task<bool> DeleteClientWithAddress(Guid id, bool deleteAddress);
        Task<Guid> CreateAddress(AddressViewModel address);
        //Task<bool> UpdateAddress(Guid addressId, string country, string state, string city, string neighborhood, string road, string number, string complement);
        Task<bool> DeleteAddress(Guid id);
        Task<AuthenticationResponse> AuthenticateClient(Guid clientId);
        //void Dispose();
    }
}
