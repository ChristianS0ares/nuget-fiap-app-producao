using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Testing;
using nuget_fiap_app_producao_common.Models;
using nuget_fiap_app_producao_common.Models.Enum;
using System.Net.Http.Json;
using System.Runtime.ConstrainedExecution;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace nuget_fiap_app_producao_test_bdd.StepDefinitions
{
    [Binding]
    public sealed class ProducaoStepDefinitions
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef
        private readonly HttpClient _client;
        private HttpResponseMessage _response;
        private Producao _producaoCriado;
        private readonly string _baseUrl = "/producao";

        public ProducaoStepDefinitions(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Given(@"que eu adicionei na esteira de produção um pedido do cliente ""(.*)""")]
        public async Task GivenQueEuAdicioneiUmaProducaoDePedidoDoCliente(string nomeCliente)
        {
            var novoProducao = new Producao
            {
                Pedido = new Pedido()
                {
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

        [When(@"eu solicito a lista de pedidos enviados para produção")]
        public async Task WhenEuSolicitoAListaDePedidosEnviadosParaProducao()
        {
            _response = await _client.GetAsync($"{_baseUrl}");
        }

        [Then(@"eu devo receber uma lista contendo o pedido do cliente ""(.*)""")]
        public void ThenEuDevoReceberUmaListaContendoOPedidoDoCliente(string p0)
        { 
            _response.EnsureSuccessStatusCode();
            var producoes = _response.Content.ReadFromJsonAsync<List<Producao>>().Result.Where(p => p.Pedido?.Cliente?.Nome == p0)?.ToList();
            foreach (var producao in producoes)
            {
                producao.Pedido?.Cliente?.Nome.Should().Be(p0);
            }
        }



        [Given(@"que o pagamento está OK pela API de Pagamento")]
        public async Task GivenQueOPagamentoEstaOKPelaAPIDePagamento()
        {
            var novoProducao = new Producao
            {
                Pedido = new Pedido()
                {
                    Cliente = new Cliente { Nome = "Eduardo Vargas" },
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
            _producaoCriado.Status.Should().Be(StatusProducao.Recebido); // O status RECEBIDO indica que o pedido está OK para produção;
        }

        [When(@"eu solicito o pedido da esteira de produção")]
        public async Task WhenEuSolicitoOPedidoDaEsteiraDeProducao()
        {
            _response = await _client.GetAsync($"{_baseUrl}/{_producaoCriado.Id}");
        }

        [Then(@"o pedido deve ser adicionado com sucesso com o status ""(.*)""")]
        public void ThenOPedidoDeveSerAdicionadoComSucessoComOStatus(string p0)
        {
            _response.EnsureSuccessStatusCode();
            var producao = _response.Content.ReadFromJsonAsync<Producao>().Result;
            producao.Status.Should().Be(StatusProducaoUtil.StatusProducaoStringToEnum(p0)); // O status RECEBIDO indica que o pedido está OK para produção;
        }



        [When(@"eu atualizo o status de produção daquele pedido para alterar o status de Recebido para ""(.*)""")]
        public async Task WhenEuAtualizoOStatusDeProduçãoDaquelePedidoParaAlterarOStatusDeRecebidoPara(string p0)
        {
            var producaoAtualizada = new Producao
            {
                Pedido = _producaoCriado.Pedido,
                Status = StatusProducaoUtil.StatusProducaoStringToEnum(p0)
            };
            _response = await _client.PutAsJsonAsync($"{_baseUrl}/{_producaoCriado.Id}", producaoAtualizada);
        }

        [When(@"eu solicito a produção do pedido pelo seu ID")]
        public async Task WhenEuSolicitoAProducaoDoPedidoPeloSeuID()
        {
            _response = await _client.GetAsync($"{_baseUrl}/{_producaoCriado.Id}");
            _producaoCriado = await _response.Content.ReadFromJsonAsync<Producao>();
        }

        [Then(@"eu devo receber o status da produção do pedido atualizado para ""(.*)""")]
        public void ThenEuDevoReceberOStatusDaProducaoDoPedidoAtualizadoPara(string p0)
        {
            _producaoCriado.Should().NotBeNull();
            _producaoCriado.Status.Should().Be(StatusProducaoUtil.StatusProducaoStringToEnum(p0));
        }

       

        [When(@"eu excluo a produção do pedido do cliente pelo ID da produção")]
        public async Task WhenEuExcluoAProducaoDoPedidoDoClientePeloIDDaProducao()
        {
            _response = await _client.DeleteAsync($"{_baseUrl}/{_producaoCriado.Id}");
        }

        [Then(@"a produção do pedido do cliente ""(.*)"" não deve mais existir")]
        public void ThenAProducaoDoPedidoDoClienteNaoDeveMaisExistir(string nomeCliente)
        {
            _response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }



        [Given(@"eu tento atualizar o status de produção de um pedido com o ID inexistente ""(.*)""")]
        public async Task WhenEuTentoAtualizarUmaProducaoComUmIDInexistente(string producaoId)
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

        [Then(@"eu devo receber uma mensagem de erro informando que a produção do pedido não existe")]
        public void ThenEuDevoReceberUmaMensagemDeErroInformandoQueAProducaoDoProdutoNaoExiste()
        {
            _response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }



        [Given(@"eu tento excluir a produção de um pedido com o ID inexistente ""(.*)""")]
        public async Task WhenEuTentoExcluirComUmIDInexistente(string producaoId)
        {
            _response = await _client.DeleteAsync($"{_baseUrl}/{producaoId}");
        }
    }
}
