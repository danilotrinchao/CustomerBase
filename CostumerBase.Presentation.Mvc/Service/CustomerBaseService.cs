using CostumerBase.Presentation.Mvc.Interfaces;
using CostumerBase.Presentation.Mvc.Models;
using CostumerBase.Presentation.Mvc.Models.Reponse;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CostumerBase.Presentation.Mvc.Service
{
    public class CustomerBaseService : ICustomerBaseService
    {
        private readonly HttpClient _httpClient;
        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiRGFuaWxvIiwibmJmIjoxNjkzODMwMzE1LCJleHAiOjE2OTM4Mzc1MTUsImlhdCI6MTY5MzgzMDMxNX0.gScquXl1_TphRM_mwujDEzvEfP0OA8mEXu2HDl6LBE0";
        const string url = "https://localhost:7108/api/";

        public CustomerBaseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ClientViewModel>> GetClients()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Defina o endereço base da API
                httpClient.BaseAddress = new Uri(url);

                // Defina o cabeçalho "Authorization" com o token
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                try
                {
                    // Faça uma solicitação GET para a rota de clientes
                    HttpResponseMessage response = await httpClient.GetAsync("CostumerBase");

                    // Verifique se a resposta foi bem-sucedida
                    if (response.IsSuccessStatusCode)
                    {
                        // Processar a resposta aqui
                        var clients = await response.Content.ReadFromJsonAsync<List<ClientViewModel>>();
                        return clients;
                    }
                    else
                    {
                        // Lidar com uma resposta não autorizada (401) ou outros erros
                        // Você pode verificar o status code na resposta (response.StatusCode)
                        return null; // Ou lidar com o erro de outra forma
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Lidar com exceções de requisição HTTP, se houver
                    return null; // Ou lidar com o erro de outra forma
                }
            }
        }


        public async Task<ClientViewModel> GetClientById(Guid id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Defina o endereço base da API
                httpClient.BaseAddress = new Uri(url);

                // Defina o cabeçalho "Authorization" com o token
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                try
                {
                    // Faça uma solicitação GET para a rota de clientes
                    HttpResponseMessage response = await httpClient.GetAsync("CostumerBase/GetClientById?clientId=/{id}");

                    // Verifique se a resposta foi bem-sucedida
                    if (response.IsSuccessStatusCode)
                    {
                        // Processar a resposta aqui
                        var clients = await response.Content.ReadFromJsonAsync<ClientViewModel>();
                        return clients;
                    }
                    else
                    {
                        // Lidar com uma resposta não autorizada (401) ou outros erros
                        // Você pode verificar o status code na resposta (response.StatusCode)
                        return null; // Ou lidar com o erro de outra forma
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Lidar com exceções de requisição HTTP, se houver
                    return null; // Ou lidar com o erro de outra forma
                }
            }
        }

        public async Task<Guid> CreateClientWithAddress(ClientViewModel client)
        {
            var response = await _httpClient.PostAsJsonAsync("clientes", client);
            response.EnsureSuccessStatusCode();
            var clientId = await response.Content.ReadFromJsonAsync<Guid>();
            return clientId;
        }

        public async Task<bool> UpdateClientWithAddress(ClientViewModel client, bool updateAddress, Guid? addressId)
        {
            var response = await _httpClient.PutAsJsonAsync($"clientes/{client.Id}", client);
            response.EnsureSuccessStatusCode();

            if (updateAddress)
            {
                foreach (var address in client.AddressClient)
                {
                    await UpdateClientWithAddress(client, updateAddress, addressId);
                }
            }

            return true;
        }

        public async Task<bool> DeleteClientWithAddress(Guid id, bool deleteAddress)
        {
            if (deleteAddress)
            {
                var client = await GetClientById(id);
                if (client != null)
                {
                    foreach (var address in client.AddressClient)
                    {
                        await _httpClient.DeleteAsync($"enderecos/{address.Id}");
                    }
                }
            }

            var response = await _httpClient.DeleteAsync($"clientes/{id}");
            return response.IsSuccessStatusCode;
        }


        public async Task<Guid> CreateAddress(AddressViewModel address)
        {
            var response = await _httpClient.PostAsJsonAsync("enderecos", address);
            response.EnsureSuccessStatusCode();
            var addressId = await response.Content.ReadFromJsonAsync<Guid>();
            return addressId;
        }

        public async Task<bool> UpdateAddress(Guid addressId, AddressViewModel address)
        {
            var response = await _httpClient.PutAsJsonAsync($"enderecos/{addressId}", address);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAddress(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"enderecos/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<AuthenticationResponse> AuthenticateClient(Guid clientId)
        {
            // Aqui, você pode construir a URL correta para o seu endpoint de autenticação
            string apiUrl = $"/api/CostumerBase/Autenticar?id=C72E3F27-9A24-4EDA-9B40-DC7D7502F9C5";
        
            // Adicione o token de autenticação no cabeçalho da solicitação, se você já tiver obtido o token
            // Substitua 'seuTokenAqui' pelo token real
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiRGFuaWxvIiwibmJmIjoxNjkzODE3MTY0LCJleHAiOjE2OTM4MjQzNjQsImlhdCI6MTY5MzgxNzE2NH0.fHK_23f_s7pK6pZ4zVeS_XPZPBtacgZXMsEMVAWdlNc");
        
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, null);
        
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    AuthenticationResponse authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(responseContent);
                    return authenticationResponse;
                }
                else
                {
                    // Lida com erros de autenticação aqui, por exemplo, se o cliente não foi encontrado
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Lida com exceções aqui
                return null;
            }
        }
    }

}
