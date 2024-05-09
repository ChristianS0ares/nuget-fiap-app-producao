using nuget_fiap_app_producao_common.Interfaces.Repository;
using nuget_fiap_app_producao_common.Interfaces.Services;
using nuget_fiap_app_producao_common.Models;
using nuget_fiap_app_producao_common.Models.Enum;

namespace nuget_fiap_app_producao.Services
{
    public class ProducaoService : IProducaoService
    {
        private readonly IProducaoRepository _producaoRepository;

        public ProducaoService(IProducaoRepository producaoRepository)
        {
            _producaoRepository = producaoRepository;
        }

        private async Task<StatusProducao> ValidateStatusProducao(StatusProducao statusProducao)
        {
            if (Enum.IsDefined(typeof(StatusProducao), statusProducao))
                return await Task.Run(() => statusProducao);

            throw new InvalidOperationException($"Status de produção com o ID {(ushort) statusProducao} não existe.");
        }

        public async Task<string> AddProducao(Producao producao)
        {
            producao.Status = await ValidateStatusProducao(StatusProducao.Recebido);
            string producaoId = await _producaoRepository.AddProducao(producao);
            return producaoId;
        }

        public async Task<bool> UpdateProducao(Producao producao, string id)
        {
            var existingProducao = await _producaoRepository.GetProducaoById(id);
            if (existingProducao == null)
            {
                return false;
            }

            existingProducao.Pedido = producao.Pedido; // Pedido e Itens já validados na API de Pedido
            existingProducao.Status = await ValidateStatusProducao((StatusProducao)producao.Status);

            return await _producaoRepository.UpdateProducao(existingProducao);
        }

        public async Task<bool> DeleteProducao(string id)
        {
            return await _producaoRepository.DeleteProducao(id);
        }

        public async Task<List<Producao>> GetAllProducoes()
        {
            return await _producaoRepository.GetAllProducoes();
        }

        public async Task<Producao> GetProducaoById(string id)
        {
            return await _producaoRepository.GetProducaoById(id);
        }
    }
}