using CostumerBase.Presentation.Mvc.Interfaces;
using CostumerBase.Presentation.Mvc.Models;
using CostumerBase.Presentation.Mvc.Models.Reponse;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;

namespace CostumerBase.Presentation.Mvc.Service
{
    public class CustomerBaseService : ICustomerBaseService
    {
        private readonly HttpClient _httpClient;
        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiRGFuaWxvIiwibmJmIjoxNjkzOTQyMTk4LCJleHAiOjE2OTM5NDkzOTgsImlhdCI6MTY5Mzk0MjE5OH0.6ktc7iBK1VXadThMg0aEjqSqMKUHbwU_St3lbiGnWqw";
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
                httpClient.BaseAddress = new Uri(url);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync($"CostumerBase/GetClientById?clientId={id}");

                    if (response.IsSuccessStatusCode)
                    {
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

        public async Task<Guid> CreateClient(ClientViewModel client)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                try
                {
                    string jsonClient = JsonConvert.SerializeObject(client);
                    var content = new StringContent(jsonClient, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync("CostumerBase/createClients", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var clients = await response.Content.ReadFromJsonAsync<ClientViewModel>();
                        return clients.Id;
                    }
                    else
                    {
                        // Lidar com uma resposta não autorizada (401) ou outros erros
                        // Você pode verificar o status code na resposta (response.StatusCode)
                        throw new Exception("Erro ao inserir cliente.");
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Lidar com exceções de requisição HTTP, se houver
                    throw; // Ou lidar com o erro de outra forma
                }
            }
        }


        public async Task<bool> UpdateClientWithAddress(ClientViewModel client, Guid id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = await httpClient.PutAsJsonAsync($"CostumerBase/updateClients/{id}", client);
                    response.EnsureSuccessStatusCode();
                    

                    return response.IsSuccessStatusCode;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Erro na requisição HTTP ao atualizar cliente.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Erro ao desserializar a resposta JSON.", ex);
            }
        }

        public async Task<bool> DeleteClientWithAddress(Guid id, bool deleteAddress)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    if (deleteAddress)
                    {
                        var client = await GetClientById(id);

                        if (client != null)
                        {
                            foreach (var address in client.AddressClient)
                            {
                                await DeleteAddress(address.Id);
                            }
                        }
                    }

                    var response = await httpClient.DeleteAsync($"clientes/{id}");
                    return response.IsSuccessStatusCode;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Erro na requisição HTTP ao excluir cliente.", ex);
            }
        }

        public async Task<Guid> CreateAddress(AddressViewModel address)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = await httpClient.PostAsJsonAsync("enderecos", address);
                    response.EnsureSuccessStatusCode();

                    var createdAddress = await response.Content.ReadFromJsonAsync<AddressViewModel>();
                    return createdAddress.Id;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Erro na requisição HTTP ao criar endereço.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Erro ao desserializar a resposta JSON.", ex);
            }
        }

        public async Task<bool> UpdateAddress(Guid addressId, AddressViewModel address)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = await httpClient.PutAsJsonAsync($"enderecos/{addressId}", address);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Erro na requisição HTTP ao atualizar endereço.", ex);
            }
        }

        public async Task<bool> DeleteAddress(Guid id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = await httpClient.DeleteAsync($"enderecos/{id}");
                    return response.IsSuccessStatusCode;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Erro na requisição HTTP ao excluir endereço.", ex);
            }
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
