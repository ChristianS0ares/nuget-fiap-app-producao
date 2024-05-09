using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using nuget_fiap_app_producao.Controllers;
using nuget_fiap_app_producao_common.Interfaces.Services;
using nuget_fiap_app_producao_common.Models;
using nuget_fiap_app_producao_common.Models.Enum;

namespace nuget_fiap_app_producao_test.Controller
{
    public class ProducaoControllerTest
    {
        private readonly Mock<IProducaoService> _producaoServiceMock;
        private readonly ProducaoController _controller;

        public ProducaoControllerTest()
        {
            _producaoServiceMock = new Mock<IProducaoService>();
            _controller = new ProducaoController(_producaoServiceMock.Object);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task GetAll_ShouldReturn200OK_WhenProducoesExist()
        {
            var pedidos = new List<Producao>
            {
                new() { Id = "1", Pedido = new Pedido() { Itens = new List<Item> { new() { Id = 1, Preco = 100, Quantidade = 1 } } }, Status = StatusProducao.Recebido },
                new() { Id = "2", Pedido = new Pedido() { Itens = new List<Item> { new() { Id = 2, Preco = 50, Quantidade = 2 } } }, Status = StatusProducao.Pronto }
            };
            _producaoServiceMock.Setup(x => x.GetAllProducoes()).ReturnsAsync(pedidos);

            var result = await _controller.GetAll();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(pedidos);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task GetAll_ShouldReturn404NotFound_WhenNoProducoesExist()
        {
            _producaoServiceMock.Setup(x => x.GetAllProducoes()).ReturnsAsync(new List<Producao>());

            var result = await _controller.GetAll();

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task GetById_ShouldReturn200OK_WhenPedidoExists()
        {
            var pedido = new Producao { Id = "1" };
            _producaoServiceMock.Setup(x => x.GetProducaoById("1")).ReturnsAsync(pedido);

            var result = await _controller.GetById("1");

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(pedido);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task GetById_ShouldReturn404NotFound_WhenPedidoDoesNotExist()
        {
            _producaoServiceMock.Setup(x => x.GetProducaoById("1")).ReturnsAsync((Producao)null);

            var result = await _controller.GetById("1");

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Post_ShouldReturn201Created_WhenPedidoIsCreated()
        {
            var pedido = new Producao();
            _producaoServiceMock.Setup(x => x.AddProducao(It.IsAny<Producao>())).ReturnsAsync("1");

            var result = await _controller.Post(pedido);

            result.Should().BeOfType<CreatedAtRouteResult>();
            var createdResult = result as CreatedAtRouteResult;
            createdResult.RouteName.Should().Be("GetPedidoById");
            createdResult.RouteValues["id"].Should().Be("1");
            createdResult.Value.Should().BeEquivalentTo(pedido);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Put_ShouldReturn200OK_WhenPedidoIsUpdated()
        {
            _producaoServiceMock.Setup(x => x.UpdateProducao(It.IsAny<Producao>(), "1")).ReturnsAsync(true);

            var result = await _controller.Put("1", new Producao());

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Put_ShouldReturn404NotFound_WhenPedidoDoesNotExist()
        {
            _producaoServiceMock.Setup(x => x.UpdateProducao(It.IsAny<Producao>(), "1")).ReturnsAsync(false);

            var result = await _controller.Put("1", new Producao());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Delete_ShouldReturn204NoContent_WhenPedidoIsDeleted()
        {
            _producaoServiceMock.Setup(x => x.DeleteProducao("1")).ReturnsAsync(true);

            var result = await _controller.Delete("1");

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Delete_ShouldReturn404NotFound_WhenPedidoDoesNotExist()
        {
            _producaoServiceMock.Setup(x => x.DeleteProducao("1")).ReturnsAsync(false);

            var result = await _controller.Delete("1");

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task GetAll_ShouldReturn500InternalServerError_WhenExceptionIsThrown()
        {
            _producaoServiceMock.Setup(x => x.GetAllProducoes()).ThrowsAsync(new Exception("Internal Server Error"));

            var result = await _controller.GetAll();

            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.StatusCode.Should().Be(500);
        }
    }
}
