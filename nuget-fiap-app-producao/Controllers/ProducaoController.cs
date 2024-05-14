using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nuget_fiap_app_producao_common.Interfaces.Services;
using nuget_fiap_app_producao_common.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace nuget_fiap_app_producao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProducaoController : ControllerBase
    {
        private readonly IProducaoService _producaoService;

        public ProducaoController(IProducaoService producaoService)
        {
            _producaoService = producaoService;
        }

        [HttpGet(Name = "GetAllProducoes")]
        [SwaggerOperation(Summary = "Listagem de todas as produções ativas", Description = "Recupera uma lista de todas as produções.")]
        [SwaggerResponse(StatusCodes.Status200OK, "A lista de produções foi recuperada com sucesso.", typeof(IEnumerable<Producao>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nenhuma produção encontrada.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor.")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var producoes = await _producaoService.GetAllProducoes();
                if (producoes == null || producoes.Count == 0)
                    return NotFound("Nenhuma produção encontrada.");

                return Ok(producoes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetProducaoById")]
        [SwaggerOperation(Summary = "Obtenção de produção por ID", Description = "Obtém uma produção com base no ID especificado.")]
        [SwaggerResponse(StatusCodes.Status200OK, "A produção foi recuperada com sucesso.", typeof(Producao))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Produção não encontrada.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor.")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var producao = await _producaoService.GetProducaoById(id);
                if (producao == null)
                    return NotFound($"Produção com ID {id} não encontrada.");

                return Ok(producao);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost(Name = "CreateProducao")]
        [SwaggerOperation(Summary = "Criação de uma nova produção de pedido", Description = "Cria uma nova produção com base nos dados fornecidos.")]
        [SwaggerResponse(StatusCodes.Status201Created, "A produção foi criada com sucesso.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor.")]
        public async Task<IActionResult> Post([FromBody] Producao producao)
        {
            try
            {
                var producaoId = await _producaoService.AddProducao(producao);
                return CreatedAtRoute("GetProducaoById", new { id = producaoId }, producao);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}", Name = "UpdateProducao")]
        [SwaggerOperation(Summary = "Atualização de uma produção por ID", Description = "Atualiza uma produção com base no ID especificado.")]
        [SwaggerResponse(StatusCodes.Status200OK, "A produção foi atualizada com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Produção não encontrada.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor.")]
        public async Task<IActionResult> Put(string id, [FromBody] Producao producao)
        {
            try
            {
                bool updated = await _producaoService.UpdateProducao(producao, id);
                if (!updated)
                    return NotFound($"Produção com ID {id} não encontrada.");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}", Name = "DeleteProducao")]
        [SwaggerOperation(Summary = "Exclusão de produção por ID", Description = "Exclui uma produção com base no ID especificado.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "A produção foi excluída com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Produção não encontrada.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor.")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                bool deleted = await _producaoService.DeleteProducao(id);
                if (!deleted)
                    return NotFound($"Produção com ID {id} não encontrada.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
