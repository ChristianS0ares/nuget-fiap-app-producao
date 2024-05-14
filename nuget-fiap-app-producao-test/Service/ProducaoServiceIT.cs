using nuget_fiap_app_producao_common.Interfaces.Repository;
using nuget_fiap_app_producao_repository.DB;
using nuget_fiap_app_producao_repository;
using nuget_fiap_app_producao.Services;
using nuget_fiap_app_producao_common.Models;
using FluentAssertions;
using nuget_fiap_app_producao_common.Models.Enum;

namespace nuget_fiap_app_producao_test.Service
{
    public class ProducaoServiceIT
    {
        private readonly ProducaoService _producaoService;
        private readonly RepositoryDB _repositoryDB;
        private readonly IProducaoRepository _producaoRepository;

        public ProducaoServiceIT()
        {
            _repositoryDB = new RepositoryDB();
            _producaoRepository = new ProducaoRepository(_repositoryDB);
            _producaoService = new ProducaoService(_producaoRepository);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task DevePermitirCriarProducao()
        {
            var novoProducao = new Producao
            {
                Pedido = new Pedido() {
                    Itens = new List<Item>
                    {
                        new() { Id = 1, Quantidade = 10, Descricao = "Batata Frita", Preco = 2.50m }
                    }
                },
                Status = StatusProducao.Recebido
            };

            var pedidoId = await _producaoService.AddProducao(novoProducao);

            pedidoId.Should().NotBeNullOrEmpty();

            var pedidoCriado = await _producaoService.GetProducaoById(pedidoId);

            pedidoCriado.Should().NotBeNull();
            pedidoCriado.Pedido.Should().NotBeNull();
            pedidoCriado.Status.Should().Be(StatusProducao.Recebido);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task DevePermitirAtualizarProducao()
        {
            var producao = new Producao
            {
                Pedido = new Pedido()
                {
                    Itens = new List<Item>
                    {
                        new() { Id = 1, Quantidade = 15, Descricao = "Batata Frita", Preco = 2.50m }
                    }
                },
                Status = StatusProducao.Recebido
            };

            var pedidoId = await _producaoService.AddProducao(producao);
            producao.Id = pedidoId; // Atualizar o ID para garantir consistência
            producao.Status = StatusProducao.EmPreparacao;

            var resultado = await _producaoService.UpdateProducao(producao, pedidoId);

            resultado.Should().BeTrue();

            var pedidoAtualizado = await _producaoService.GetProducaoById(pedidoId);

            pedidoAtualizado.Should().NotBeNull();
            pedidoAtualizado.Status.Should().Be(StatusProducao.Recebido);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task DevePermitirExcluirProducao()
        {
            var producao = new Producao
            {
                Pedido = new Pedido()
                {
                    Itens = new List<Item>
                    {
                        new() { Id = 1, Quantidade = 10, Descricao = "Item para exclusão", Preco = 5.50m }
                    }
                },
                Status = StatusProducao.Recebido
            };

            var pedidoId = await _producaoService.AddProducao(producao);

            var resultadoExclusao = await _producaoService.DeleteProducao(pedidoId);

            resultadoExclusao.Should().BeTrue();

            var pedidoExcluido = await _producaoService.GetProducaoById(pedidoId);

            pedidoExcluido.Should().BeNull();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task DeveRecuperarTodosProducaos()
        {
            await _producaoService.AddProducao(new Producao
            {
                Pedido = new Pedido()
                {
                    Itens = new List<Item>
                    {
                        new() { Id = 1, Quantidade = 10, Descricao = "Hambúrguer", Preco = 5.50m }
                    }
                },
                Status = StatusProducao.Recebido
            });

            await _producaoService.AddProducao(new Producao
            {
                Pedido = new Pedido()
                {
                    Itens = new List<Item>
                    {
                        new() { Id = 2, Quantidade = 20, Descricao = "Batata Frita", Preco = 15.00m }
                    }
                },
                Status = StatusProducao.EmPreparacao
            });

            var producoes = await _producaoService.GetAllProducoes();

            producoes.Should().NotBeNull();
            producoes.Should().HaveCountGreaterOrEqualTo(2);
        }
    }
}
