using System.Reflection;
using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Common.Logging;
using Common.Logging.Correlation;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace Catalog.API;

public class Startup
{
    public IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //Todo: 2.22.1 Setup Api Versioning
        services.AddApiVersioning();
        // services.AddCors(options =>
        // {
        //     options.AddPolicy("CorsPolicy", policy =>
        //     {
        //         policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        //     });
        // });
        //Todo: 2.22.2 Setup HealthCheck
        //Phương thức này được sử dụng để thêm dịch vụ kiểm tra sức khỏe vào container dịch vụ.
        //Kiểm tra sức khỏe là một cách để kiểm tra nhanh tình trạng của các dịch vụ và hệ thống mà ứng dụng của bạn phụ thuộc vào.
        services.AddHealthChecks()
            //Phương thức này thêm một kiểm tra sức khỏe cho MongoDB vào hệ thống kiểm tra sức khỏe.
            //Nó sẽ kiểm tra xem ứng dụng có thể kết nối đến cơ sở dữ liệu MongoDB hay không.
            // + Configuration["DatabaseSettings:ConnectionString"] là chuỗi kết nối đến cơ sở dữ liệu MongoDB.
            // + "Catalog  Mongo Db Health Check" là tên của kiểm tra sức khỏe này, bạn có thể sử dụng nó để nhận biết kết quả kiểm tra sức khỏe cụ thể nào.
            // + HealthStatus.Degraded là trạng thái sẽ được báo cáo nếu kiểm tra sức khỏe thất bại.
            //      Trong trường hợp này, nếu ứng dụng không thể kết nối đến MongoDB, trạng thái sức khỏe sẽ được đánh dấu là Degraded.
            .AddMongoDb(Configuration["DatabaseSettings:ConnectionString"],
                "Catalog  Mongo Db Health Check",
                HealthStatus.Degraded);
        services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Catalog.API", Version = "v1"}); });
        
        //DI
        //Todo: 2.22.3 Setup Dependency Inject
        services.AddAutoMapper(typeof(Startup));
        services.AddMediatR(typeof(CreateProductHandler).GetTypeInfo().Assembly);
        services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
        services.AddScoped<ICatalogContext, CatalogContext>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBrandRepository, ProductRepository>();
        services.AddScoped<ITypesRepository, ProductRepository>();

        services.AddControllers();
        //Identity Server changes
        // var userPolicy = new AuthorizationPolicyBuilder()
        //     .RequireAuthenticatedUser()
        //     .Build();
        //
        // services.AddControllers(config =>
        // {
        //     config.Filters.Add(new AuthorizeFilter(userPolicy));
        // });
        //
        //
        // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //         .AddJwtBearer(options =>
        //         {
        //             options.Authority = "https://localhost:9009";
        //             options.Audience = "Catalog";
        //         });
        // services.AddAuthorization(options =>
        // {
        //     options.AddPolicy("CanRead", policy=>policy.RequireClaim("scope", "catalogapi.read"));
        // });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();  
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        // app.UseCors("CorsPolicy");d
        app.UseAuthentication();
        app.UseStaticFiles();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            //Todo: 2.23.1 Setup HealthCheck route api
            //Phương thức này định tuyến yêu cầu HTTP đến đường dẫn “/health” để thực hiện tất cả các kiểm tra sức khỏe đã được đăng ký.
            endpoints.MapHealthChecks("/health", new HealthCheckOptions()
            {
                //Điều này chỉ định rằng tất cả các kiểm tra sức khỏe sẽ được thực hiện khi có yêu cầu đến đường dẫn “/health”.
                //Điều này có thể được tùy chỉnh để chỉ thực hiện một số kiểm tra sức khỏe nhất định dựa trên điều kiện.
                Predicate = _ => true,
                // Đây là phương thức được sử dụng để viết phản hồi HTTP khi có yêu cầu đến đường dẫn “/health”.
                // UIResponseWriter.WriteHealthCheckUIResponse là một phương thức được cung cấp sẵn để viết phản hồi theo định dạng phù hợp với giao diện người dùng HealthChecks UI.
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
    
    /// <summary>
    /// //Todo: 2.23.2 Example for health check with many route api
    /// </summary>
    /// <param name="services"></param>
    // public void HealthCheckConfigureServices(IServiceCollection services)
    // {
    //     services.AddHealthChecks()
    //         .AddCheck("Database", new SqlConnectionHealthCheck(Configuration.GetConnectionString("DefaultConnection")), tags: new[] { "basic" })
    //         .AddCheck("ExternalService", new ExternalServiceHealthCheck(), tags: new[] { "full" });
    // }

    /// <summary>
    /// //Todo: 2.23.3 Example for health check with many route api
    /// </summary>
    /// <param name="services"></param>
    // public void HealthCheckConfigure(IApplicationBuilder app, IWebHostEnvironment env)
    // {
    //     app.UseEndpoints(endpoints =>
    //     {
    //         endpoints.MapHealthChecks("/health/basic", new HealthCheckOptions
    //         {
    //             Predicate = check => check.Tags.Contains("basic"),
    //             ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    //         });
    //
    //         endpoints.MapHealthChecks("/health/full", new HealthCheckOptions
    //         {
    //             Predicate = check => check.Tags.Contains("full"),
    //             ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    //         });
    //     });
    // }
}