using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using nuget_fiap_app_producao_common.Models;
using nuget_fiap_app_producao_common.Models.Enum;
using System.Net.Http.Json;

namespace nuget_fiap_app_producao_test_bdd.StepDefinitions
{
    [Binding]
    public class ProducaoSteps
    {
        private readonly HttpClient _client;
        private HttpResponseMessage _response;
        private Producao _producaoCriado;
        private readonly string _baseUrl = "/producao";

        public ProducaoSteps(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Given(@"que eu adicionei uma produção de pedido do cliente ""(.*)""")]
        public async Task GivenQueEuAdicioneiUmaProducaoDePedidoDoCliente(string nomeCliente)
        {
            var novoProducao = new Producao
            {
                Pedido = new Pedido() { 
                    Cliente = new Cliente { Nome = nomeCliente },
                    Itens = new List<Item> { new() { Id = 1, Descricao = "Hambúrguer", Quantidade = 1 } },
                    Data = DateTime.Now
                },
                Status = StatusProducao.Recebido
            };
            _response = await _client.PostAsJsonAsync(_baseUrl, novoProducao);
            _response.EnsureSuccessStatusCode();

            var locationHeader = _response.Headers.Location.ToString();
            var producaoId = locationHeader.Split('/').Last();

            _response = await _client.GetAsync($"{_baseUrl}/{producaoId}");
            _response.EnsureSuccessStatusCode();

            _producaoCriado = await _response.Content.ReadFromJsonAsync<Producao>();
            _producaoCriado.Should().NotBeNull();
            _producaoCriado.Id.Should().Be(producaoId);
        }

        [When(@"eu solicito a produção pelo seu ID")]
        public async Task WhenEuSolicitoAProducaoPeloSeuID()
        {
            _response = await _client.GetAsync($"{_baseUrl}/{_producaoCriado.Id}");
        }

        [When(@"eu atualizo a produção para alterar o status ""(.*)""")]
        public async Task WhenEuAtualizoAProducaoParaAlterarOStatus(ushort status)
        {
            var producaoAtualizada = new Producao
            {
                Pedido = new Pedido()
                {
                    Cliente = _producaoCriado.Pedido?.Cliente,
                    Itens = new List<Item> { new() { Id = 1, Descricao = "Hambúrguer", Quantidade = 1 } }
                },
                Status = (StatusProducao)status
            };
            _response = await _client.PutAsJsonAsync($"{_baseUrl}/{_producaoCriado.Id}", producaoAtualizada);
        }

        [When(@"eu excluo a produção")]
        public async Task WhenEuExcluoAProducaoDoCliente()
        {
            _response = await _client.DeleteAsync($"{_baseUrl}/{_producaoCriado.Id}");
        }

        [When(@"eu tento atualizar uma produção com o ID inexistente ""(.*)""")]
        public async Task WhenEuTentoOperarComUmIDInexistente(string producaoId)
        {
            var producaoAtualizado = new Producao
            {
                Pedido = new Pedido()
                {
                    Cliente = new Cliente { Nome = "Cliente Inexistente" },
                    Itens = new List<Item> { new() { Id = 999, Descricao = "Item Inexistente", Quantidade = 1, Preco = 10.00m } },
                    Data = DateTime.Now
                },
                Status = StatusProducao.Recebido
            };
            _response = await _client.PutAsJsonAsync($"{_baseUrl}/{producaoId}", producaoAtualizado);
        }

        [When(@"eu tento excluir uma produção com o ID inexistente ""(.*)""")]
        public async Task WhenEuTentoExcluirComUmIDInexistente(string producaoId)
        {
            _response = await _client.DeleteAsync($"{_baseUrl}/{producaoId}");
        }

        [When(@"eu solicito a lista de produções")]
        public async Task WhenEuSolicitoAListagemDeProducoes()
        {
            _response = await _client.GetAsync($"{_baseUrl}");
        }

        [Then(@"eu devo receber uma lista contendo as produções prontas")]
        public async Task ThenEuDevoReceberUmaListaContendoAsProducoesProntas()
        {
            _response.EnsureSuccessStatusCode();
            var producoes = await _response.Content.ReadFromJsonAsync<List<Producao>>();
            producoes.Should().Contain(producao => producao.Status == StatusProducao.Pronto);
        }

        [Then(@"a produção deve ser adicionada com sucesso e contendo os itens ""(.*)"" e ""(.*)""")]
        public async void ThenAProducaoDeveSerAdicionadaComSucessoEContendoOsItens(int item1, int item2)
        {
            _producaoCriado.Should().NotBeNull();
            _producaoCriado.Pedido?.Itens[0].Id.Should().Be(item1);
            _producaoCriado.Pedido?.Itens[1].Id.Should().Be(item2);
        }

        [Then(@"eu devo receber a produção do cliente ""(.*)""")]
        public async Task ThenEuDevoReceberOProducaoDoCliente(string nomeCliente)
        {
            _response.EnsureSuccessStatusCode();
            var producao = await _response.Content.ReadFromJsonAsync<Producao>();
            producao.Pedido?.Cliente.Nome.Should().Be(nomeCliente);
        }

        [Then(@"eu devo receber a produção atualizada com o item ""(.*)""")]
        public async Task ThenEuDevoReceberAProducaoAtualizadaComOItem(int item)
        {
            _response.EnsureSuccessStatusCode();
            var producao = await _response.Content.ReadFromJsonAsync<Producao>();
            producao.Pedido?.Itens.Should().Contain(i => i.Id == item);
        }

        [Then(@"a produção do cliente ""(.*)"" não deve mais existir")]
        public void ThenAProducaoDoClienteNaoDeveMaisExistir(string nomeCliente)
        {
            _response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Then(@"eu devo receber uma mensagem de erro informando que a produção não existe")]
        public void ThenEuDevoReceberUmaMensagemDeErroInformandoQueAProducaoNaoExiste()
        {
            _response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
