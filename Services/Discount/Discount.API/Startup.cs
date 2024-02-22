using System.Reflection;
using Common.Logging;
using Common.Logging.Correlation;
using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Repositories;
using MediatR;

namespace Discount.API;

public class Startup
{
    /// <summary>
    /// Todo: 4. Grpc
    /// Sử dụng Protobuf thay thế cho JSON / XML
    /// Build dựa trên HTTP 2 thay vì HTTP 1
    /// Nhanh hơn 7 - 10 khi transmit message
    /// Can be used in Polyglot environments
    ///     Viết được trên nhiều loại ngôn ngữ sau khi biên dịch protobuf file
    /// Độ trễ thấp
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMediatR(typeof(CreateDiscountCommandHandler).GetTypeInfo().Assembly);
        services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddAutoMapper(typeof(Startup));
        services.AddGrpc();
    }

    /// <summary>
    /// Todo: 4.12.1 Endpoint Mapping for GRPC
    /// Mapping Service ~ Mapping Controller trong WebApi
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<DiscountService>();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync(
                    "Communication with gRPC endpoints must be made through a gRPC client.");
            });
        });
    }
}