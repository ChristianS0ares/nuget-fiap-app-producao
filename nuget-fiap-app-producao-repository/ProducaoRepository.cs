using MongoDB.Driver;
using nuget_fiap_app_producao_common.Interfaces.Repository;
using nuget_fiap_app_producao_common.Models;
using nuget_fiap_app_producao_repository.DB;

namespace nuget_fiap_app_producao_repository
{
    public class ProducaoRepository : IProducaoRepository
    {
        private RepositoryDB _session;
        private IMongoCollection<Producao> _producoes;
        public ProducaoRepository(RepositoryDB session)
        {
            _session = session;
            _producoes = _session.Database.GetCollection<Producao>("producoes");
        }
        public async Task<string> AddProducao(Producao producao)
        {
            await _producoes.InsertOneAsync(producao);

            return producao.Id;
        }

        public async Task<bool> DeleteProducao(string id)
        {
            var result = await _producoes.DeleteOneAsync(producao => producao.Id == id);

            return result.DeletedCount > 0;
        }

        public async Task<List<Producao>> GetAllProducoes()
        {
            return await _producoes.Find(_ => true).ToListAsync();
        }

        public async Task<Producao> GetProducaoById(string id)
        {
            return await _producoes.Find(producao => producao.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProducao(Producao producao)
        {
            var filter = Builders<Producao>.Filter.Eq(p => p.Id, producao.Id);
            var result = await _producoes.ReplaceOneAsync(filter, producao, new ReplaceOptions { IsUpsert = false });

            return result.ModifiedCount > 0;
        }
    }
}
