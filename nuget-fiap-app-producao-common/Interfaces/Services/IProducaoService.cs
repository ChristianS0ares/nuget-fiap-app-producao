using nuget_fiap_app_producao_common.Models;

namespace nuget_fiap_app_producao_common.Interfaces.Services
{
    public interface IProducaoService
    {
        Task<List<Producao>> GetAllProducoes();
        Task<Producao> GetProducaoById(string id);
        Task<string> AddProducao(Producao producao);
        Task<bool> UpdateProducao(Producao producao, string id);
        Task<bool> DeleteProducao(string id);
    }
}
