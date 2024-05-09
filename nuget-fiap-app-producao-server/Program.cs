using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using nuget_fiap_app_producao.Services;
using nuget_fiap_app_producao_common.Interfaces.Repository;
using nuget_fiap_app_producao_common.Interfaces.Services;
using nuget_fiap_app_producao_repository;
using nuget_fiap_app_producao_repository.DB;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Adicione configuração do MemoryCache
        builder.Services.AddMemoryCache();
        var configuration = builder.Configuration;

        // Configurando ProducaoAPIRepository com HttpClient e parâmetros necessários
        builder.Services.AddHttpClient("ProducaoAPI", client =>
        {
            client.BaseAddress = new Uri(configuration["ProducaoApi:BaseUrl"]);
        });

        // Registro de outros serviços e repositórios
        builder.Services.AddScoped<RepositoryDB>();
        builder.Services.AddScoped<IProducaoRepository, ProducaoRepository>();
        builder.Services.AddScoped<IProducaoService, ProducaoService>();
        builder.Services.AddMemoryCache();

        // Configuração do HealthCheck e Swagger
        builder.Services.AddHealthChecks();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "NuGET Burger",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "Miro",
                    Url = new Uri("https://miro.com/app/board/uXjVMqYSzbg=/?share_link_id=124875092732")
                }
            });
        });

        var app = builder.Build();

        // Configuração do pipeline de requisições HTTP
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "NuGET Burger API V1");
        });
        app.UseReDoc(c =>
        {
            c.DocumentTitle = "REDOC API Documentation";
            c.SpecUrl("/swagger/v1/swagger.json");
        });

        app.UseAuthorization();
        app.MapControllers();
        app.MapHealthChecks("/health", new HealthCheckOptions()
        {
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
            },
        });

        app.Run();
    }
}
