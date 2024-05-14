using nuget_fiap_app_producao_common.Models;

namespace nuget_fiap_app_producao_common.Interfaces.Repository
{
    public interface IProdutoAPIRepository
    {
        public Task<IEnumerable<Item>> GetAllItens();
    }
}
