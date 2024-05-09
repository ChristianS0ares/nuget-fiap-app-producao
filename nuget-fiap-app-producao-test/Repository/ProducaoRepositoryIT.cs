using nuget_fiap_app_producao_common.Models;
using nuget_fiap_app_producao_common.Models.Enum;
using nuget_fiap_app_producao_repository;
using nuget_fiap_app_producao_repository.DB;

namespace nuget_fiap_app_producao_test.Repository
{
    public class ProducaoRepositoryIT
    {
        private readonly ProducaoRepository _repository;
        private readonly RepositoryDB _repositoryDB;

        public ProducaoRepositoryIT()
        {
            _repositoryDB = new RepositoryDB();
            _repository = new ProducaoRepository(_repositoryDB);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task AdicionarProducao_DeveRetornarIdValido()
        {
            var producao = new Producao
            {
                Pedido = new Pedido()
                {
                    Itens = new List<Item> {
                        new() { Preco = 150.00m, Quantidade = 2 } // Total = 300.00m
                    }
                },
                Status = StatusProducao.Recebido
            };

            var id = await _repository.AddProducao(producao);
            Assert.False(string.IsNullOrWhiteSpace(id));

            // Limpa após o teste
            await _repository.DeleteProducao(id);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task AtualizarProducao_DeveRetornarVerdadeiro()
        {
            var producao = new Producao
            {
                Pedido = new Pedido()
                {
                     Itens = new List<Item> {
                        new() { Preco = 200.00m, Quantidade = 1 } // Total = 200.00m
                    }
                },
                Status = StatusProducao.Recebido
            };

            var id = await _repository.AddProducao(producao);
            producao.Id = id;
            producao.Status = StatusProducao.EmPreparacao;

            var updateResult = await _repository.UpdateProducao(producao);
            Assert.True(updateResult);

            // Limpa após o teste
            await _repository.DeleteProducao(id);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task DeletarProducao_DeveRetornarVerdadeiro()
        {
            var producao = new Producao
            {
                Pedido = new Pedido()
                {
                    Itens = new List<Item> {
                        new() { Preco = 150.00m, Quantidade = 1 } // Total = 150.00m
                    }
                },
                Status = StatusProducao.Recebido
            };

            var id = await _repository.AddProducao(producao);

            var deleteResult = await _repository.DeleteProducao(id);
            Assert.True(deleteResult);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ObterTodosProducoes_DeveRetornarLista()
        {
            var producao = new Producao
            {
                Pedido = new Pedido()
                {
                    Itens = new List<Item> {
                        new() { Preco = 100.00m, Quantidade = 1 } // Total = 100.00m
                    }
                },
                Status = StatusProducao.Recebido
            };

            var id = await _repository.AddProducao(producao);

            var producaos = await _repository.GetAllProducoes();
            Assert.True(producaos.Any());

            // Limpa após o teste
            await _repository.DeleteProducao(id);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ObterProducaoPorId_DeveRetornarProducao()
        {
            var producao = new Producao
            {
                Pedido = new Pedido()
                {
                    Itens = new List<Item> {
                        new() { Preco = 100.00m, Quantidade = 1 } // Total = 100.00m
                    }
                },
                Status = StatusProducao.Recebido
            };

            var id = await _repository.AddProducao(producao);
            var retrievedProducao = await _repository.GetProducaoById(id);

            Assert.Equal(id, retrievedProducao.Id);
            Assert.Equal(StatusProducao.Recebido, retrievedProducao.Status);

            // Limpa após o teste
            await _repository.DeleteProducao(id);
        }
    }
}
