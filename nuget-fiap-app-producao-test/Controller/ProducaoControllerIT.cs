using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using nuget_fiap_app_producao_common.Models;
using nuget_fiap_app_producao_common.Models.Enum;
using System.Net;
using System.Text;

namespace nuget_fiap_app_producao_test.Controller
{
    public class ProducaoControllerIT : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProducaoControllerIT(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        private async Task<string> CreateProducaoAsync()
        {
            var newPedido = new Producao
            {
                Pedido = new Pedido {
                    Cliente = new Cliente { Nome = "Guilherme Arana", CPF = "12345678901", Email = "guilherme.arana@example.com" },
                    Itens = new List<Item> { new Item { Id = 1, Descricao = "Hambúrguer", Quantidade = 10 } }
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(newPedido), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/Producao", content);
            response.EnsureSuccessStatusCode();

            var location = response.Headers.Location.ToString();
            var id = location.Substring(location.LastIndexOf('/') + 1);
            return id;
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetAllPedidos_ShouldReturn200OK_WhenPedidosExist()
        {
            var id = await CreateProducaoAsync();
            var response = await _client.GetAsync("/Producao");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetProducaoById_ShouldReturn200OK_WhenProducaoExists()
        {
            var id = await CreateProducaoAsync();
            var response = await _client.GetAsync($"/Producao/{id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task UpdateProducao_ShouldReturn200OK_WhenProducaoIsUpdated()
        {
            var id = await CreateProducaoAsync();
            var updatedProducao = new Producao
            {
                Pedido = new Pedido
                {
                    Cliente = new Cliente { Nome = "Matías Zaracho", CPF = "12345678901", Email = "matias.zaracho@example.com" },
                    Itens = new List<Item> { new Item { Id = 1, Descricao = "Hambúrguer", Quantidade = 5 } }
                },
                Status = StatusProducao.EmPreparacao
            };
            var content = new StringContent(JsonConvert.SerializeObject(updatedProducao), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/Producao/{id}", content);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task DeleteProducao_ShouldReturn204NoContent_WhenProducaoIsDeleted()
        {
            var id = await CreateProducaoAsync();
            var response = await _client.DeleteAsync($"/Producao/{id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task DeleteProducao_ShouldReturn404NotFound_WhenProducaoDoesNotExist()
        {
            var response = await _client.DeleteAsync("/Producao/777777"); // Assumindo que este ID não existe
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
