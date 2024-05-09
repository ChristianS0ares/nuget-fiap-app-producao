using Moq;
using nuget_fiap_app_producao_common.Interfaces.Repository;
using nuget_fiap_app_producao_common.Models;
using nuget_fiap_app_producao_common.Models.Enum;

namespace nuget_fiap_app_producao_test.Repository
{
    public class ProducaoRepositoryTest
    {
        private readonly Mock<IProducaoRepository> _mockProducaoRepository;

        public ProducaoRepositoryTest()
        {
            _mockProducaoRepository = new Mock<IProducaoRepository>();
        }


        [Fact]
        [Trait("Category", "Unit")]
        public async Task AdicionarProducao_DeveRetornarId()
        {
            var novoProducao = new Producao
            {
                Pedido = new Pedido
                {
                    Itens = new List<Item>
                    {
                        new() { Id = 1, Descricao = "Item 1", Preco = 100.00m, Quantidade = 2 }
                    }
                },
                Status = StatusProducao.Recebido
            };

            _mockProducaoRepository.Setup(repo => repo.AddProducao(It.IsAny<Producao>()))
                                 .ReturnsAsync(Guid.NewGuid().ToString());

            var idResultado = await _mockProducaoRepository.Object.AddProducao(novoProducao);

            Assert.NotNull(idResultado);
            Assert.NotEmpty(idResultado);
            _mockProducaoRepository.Verify(repo => repo.AddProducao(It.IsAny<Producao>()), Times.Once);

            Assert.Equal(StatusProducao.Recebido, novoProducao.Status);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task DeletarProducao_DeveRetornarVerdadeiro()
        {
            string idProducao = Guid.NewGuid().ToString();
            _mockProducaoRepository.Setup(repo => repo.DeleteProducao(It.IsAny<string>()))
                                 .ReturnsAsync(true);

            var resultado = await _mockProducaoRepository.Object.DeleteProducao(idProducao);

            Assert.True(resultado);
            _mockProducaoRepository.Verify(repo => repo.DeleteProducao(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ObterTodosProducoes_DeveRetornarListaDeProducoes()
        {
            var listaProducoes = new List<Producao>
            {
                new() { Pedido = new Pedido() { Itens = new List<Item> { new() { Id = 1, Descricao = "Item 1", Preco = 50.00m, Quantidade = 2 } } }, Status = StatusProducao.Pronto },
                new() { Pedido = new Pedido() { Itens = new List<Item> { new() { Id = 2, Descricao = "Item 2", Preco = 75.00m, Quantidade = 2 } } }, Status = StatusProducao.EmPreparacao }
            };
            _mockProducaoRepository.Setup(repo => repo.GetAllProducoes())
                                 .ReturnsAsync(listaProducoes);

            var resultado = await _mockProducaoRepository.Object.GetAllProducoes();

            Assert.Equal(2, resultado.Count);
            _mockProducaoRepository.Verify(repo => repo.GetAllProducoes(), Times.Once);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task ObterProducaoPorId_DeveRetornarProducao()
        {
            var producao = new Producao
            {
                Id = Guid.NewGuid().ToString(),
                Pedido = new Pedido()
                {
                    Id = Guid.NewGuid().ToString(),
                    Itens = new List<Item> { new Item { Id = 1, Descricao = "Item Test", Preco = 100.00m, Quantidade = 1 } }
                },
                Status = StatusProducao.Recebido
            };
            _mockProducaoRepository.Setup(repo => repo.GetProducaoById(It.IsAny<string>()))
                                 .ReturnsAsync(producao);

            var resultado = await _mockProducaoRepository.Object.GetProducaoById(producao.Id);

            Assert.Equal(producao.Id, resultado.Id);
            _mockProducaoRepository.Verify(repo => repo.GetProducaoById(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task AtualizarProducao_DeveRetornarVerdadeiro()
        {
            var producao = new Producao
            {
                Id = Guid.NewGuid().ToString(),
                Pedido = new Pedido()
                {
                    Id = Guid.NewGuid().ToString(),
                    Itens = new List<Item> { new Item { Id = 1, Descricao = "Item Atualizado", Preco = 300.00m, Quantidade = 1 } }
                },
                Status = StatusProducao.EmPreparacao
            };
            _mockProducaoRepository.Setup(repo => repo.UpdateProducao(It.IsAny<Producao>()))
                                 .ReturnsAsync(true);

            var resultado = await _mockProducaoRepository.Object.UpdateProducao(producao);

            Assert.True(resultado);
            _mockProducaoRepository.Verify(repo => repo.UpdateProducao(It.IsAny<Producao>()), Times.Once);
        }
    }
}
