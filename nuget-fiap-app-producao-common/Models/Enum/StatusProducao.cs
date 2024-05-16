namespace nuget_fiap_app_producao_common.Models.Enum
{
    public enum StatusProducao : ushort
    {
        Recebido = 0,
        EmPreparacao = 1,
        Pronto = 2,
        Finalizado = 3
    }

    public class StatusProducaoUtil
    {
        public static StatusProducao StatusProducaoStringToEnum(string statusProducao)
        {
            return statusProducao switch
            {
                "Em Preparação" => StatusProducao.EmPreparacao,
                "Pronto" => StatusProducao.Pronto,
                "Finalizado" => StatusProducao.Finalizado,
                _ => StatusProducao.Recebido,
            };
        }
    }
}
