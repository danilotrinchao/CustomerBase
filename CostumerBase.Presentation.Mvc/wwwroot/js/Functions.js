import axios from 'axios';

namespace CostumerBase.Presentation.Mvc.wwwroot.js
{
    public class Functions
    {
        const clientId = '1DA0D650-0135-4061-A1C7-8EDBA4674179'; 

        function autenticarCliente(clientId) {
        const data = {
            id: clientId,
        };

        // Substitua a URL pelo endpoint real de autenticação da sua API
        const url = 'https://localhost:7108/Autenticar'; // Rota corrigida para 'Autenticar'

        return axios.post(url, data)
            .then(response => {
                const { user, token } = response.data;
                // Armazene o token em algum lugar seguro, como na sessão ou cookies
                sessionStorage.setItem('token', token);

                // Você pode fazer algo com os detalhes do usuário, se necessário
                console.log('Usuário autenticado:', user);

                // Retorne verdadeiro para indicar que a autenticação foi bem-sucedida
                return true;
            })
            .catch(error => {
                console.error('Erro ao autenticar:', error);
                return false; // Retorne falso para indicar que a autenticação falhou
            });
    }

       
       autenticarCliente(clientId)
       .then(authenticado => {
           if (authenticado) {
               console.log('Cliente autenticado com sucesso.');
           } else {
           console.log('Falha na autenticação do cliente.');
           }
        });


        function getClientes()
        {
            axios.get('https://localhost:7108/api/clientes')
               .then(response => {
                   // Processar a resposta da API
                   const clientes = response.data;
                   // Faça algo com os clientes, como exibí-los na tela
                   console.log(clientes);
               })
               .catch(error => {
                   // Lidar com erros
                   console.error(error);
               });
        }

        function criarClienteComEndereco(cliente)
        {
            axios.post('https://localhost:7108/api/clientes', cliente)
            .then(response => {
                // Processar a resposta da API, que pode conter o ID do novo cliente
                const novoClienteId = response.data;
                // Faça algo com o ID do novo cliente
                console.log(`Novo cliente criado com ID: ${novoClienteId}`);
            })
            .catch(error => {
                // Lidar com erros
                console.error(error);
            });
        }

        function atualizarClienteComEndereco(cliente)
        {
            axios.put(`https://localhost:7108/api/clientes/${cliente.id}`, cliente)
                .then(response => {
                    // Processar a resposta da API, se necessário
                    console.log('Cliente atualizado com sucesso');
                })
                .catch(error => {
                    // Lidar com erros
                    console.error(error);
                });
        }

        function excluirClienteComEndereco(idCliente, excluirEndereco)
        {
            axios.delete(`https://localhost:7108/api/clientes/${idCliente}`, { data: { excluirEndereco } })
                .then(response => {
                    // Processar a resposta da API, se necessário
                    console.log('Cliente excluído com sucesso');
                })
                .catch(error => {
                    // Lidar com erros
                    console.error(error);
                });
        }

        function criarEndereco(endereco)
        {
                axios.post('https://localhost:7108/api/enderecos', endereco)
                    .then(response => {
                        const novoEnderecoId = response.data;
                        console.log(`Novo endereço criado com ID: ${novoEnderecoId}`);
                    })
                    .catch(error => {
                        console.error(error);
                    });
            }

            // Função para Atualizar um Endereço
        function atualizarEndereco(endereco)
        {
                axios.put(`https://localhost:7108/api/enderecos/${endereco.id}`, endereco)
                    .then(response => {
                        console.log('Endereço atualizado com sucesso');
                    })
                    .catch(error => {
                        console.error(error);
                    });
            }

            // Função para Excluir um Endereço
        function excluirEndereco(idEndereco)
        {
                axios.delete(`https://localhost:7108/api/enderecos/${idEndereco}`)
                    .then(response => {
                        console.log('Endereço excluído com sucesso');
                    })
                    .catch(error => {
                        console.error(error);
                    });
        }
    }
}
