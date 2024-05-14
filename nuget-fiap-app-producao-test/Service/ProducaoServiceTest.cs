using Moq;
using nuget_fiap_app_producao.Services;
using nuget_fiap_app_producao_common.Interfaces.Repository;
using nuget_fiap_app_producao_common.Models;
using nuget_fiap_app_producao_common.Models.Enum;

namespace nuget_fiap_app_producao_test.Service
{
    public class ProducaoServiceTest
    {
        private readonly Mock<IProducaoRepository> _mockProducaoRepository = new Mock<IProducaoRepository>();
        private readonly ProducaoService _producaoService;

        public ProducaoServiceTest()
        {
            _producaoService = new ProducaoService(_mockProducaoRepository.Object);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task AddProducao_ReturnsProducaoId_WhenValidItems()
        {
            // Arrange
            var producao = new Producao { Pedido = new Pedido() { Itens = new List<Item> { new() { Id = 1, Quantidade = 2 } } }, Status = StatusProducao.Recebido };

            _mockProducaoRepository.Setup(repo => repo.AddProducao(It.IsAny<Producao>())).ReturnsAsync("123");

            // Act
            var result = await _producaoService.AddProducao(producao);

            // Assert
            Assert.Equal("123", result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task UpdateProducao_ReturnsFalse_WhenProducaoDoesNotExist()
        {
            // Arrange
            var producao = new Producao { Pedido = new Pedido() { Itens = new List<Item> { new() { Id = 1, Quantidade = 2 } } }, Status = StatusProducao.Recebido };
            var id = "non-existing-id";

            _mockProducaoRepository.Setup(repo => repo.GetProducaoById(id)).ReturnsAsync((Producao)null);

            // Act
            var result = await _producaoService.UpdateProducao(producao, id);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task UpdateProducao_ReturnsTrue_WhenProducaoExists()
        {
            // Arrange
            var producao = new Producao { Pedido = new Pedido() { Itens = new List<Item> { new() { Id = 1, Quantidade = 2 } } }, Status = StatusProducao.Recebido };
            var existingProducao = new Producao { Id = "123", Pedido = new Pedido() { Itens = new List<Item> { new() { Id = 1, Quantidade = 1 } } }, Status = StatusProducao.Recebido };

            _mockProducaoRepository.Setup(repo => repo.GetProducaoById("123")).ReturnsAsync(existingProducao);
            _mockProducaoRepository.Setup(repo => repo.UpdateProducao(It.IsAny<Producao>())).ReturnsAsync(true);

            // Act
            var result = await _producaoService.UpdateProducao(producao, "123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task DeleteProducao_ReturnsTrue_WhenProducaoDeleted()
        {
            // Arrange
            _mockProducaoRepository.Setup(repo => repo.DeleteProducao("123")).ReturnsAsync(true);

            // Act
            var result = await _producaoService.DeleteProducao("123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task GetAllProducaos_ReturnsProducaos()
        {
            // Arrange
            var producoes = new List<Producao> { new() { Id = "123" } };
            _mockProducaoRepository.Setup(repo => repo.GetAllProducoes()).ReturnsAsync(producoes);

            // Act
            var result = await _producaoService.GetAllProducoes();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task GetProducaoById_ReturnsProducao()
        {
            // Arrange
            var producao = new Producao { Id = "123" };
            _mockProducaoRepository.Setup(repo => repo.GetProducaoById("123")).ReturnsAsync(producao);

            // Act
            var result = await _producaoService.GetProducaoById("123");

            // Assert
            Assert.Equal("123", result.Id);
        }
    }
}
