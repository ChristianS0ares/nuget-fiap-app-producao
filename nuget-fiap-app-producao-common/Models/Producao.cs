using nuget_fiap_app_producao_common.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace nuget_fiap_app_producao_common.Models
{
    public class Producao
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public Pedido? Pedido { get; set; }

        public StatusProducao? Status { get; set; }
    }
}
