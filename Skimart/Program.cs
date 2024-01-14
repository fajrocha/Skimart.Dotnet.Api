using Skimart.Application.DependencyInjection;
using Skimart.DependencyInjection;
using Skimart.Extensions;
using Skimart.Extensions.Auth;
using Skimart.Extensions.Cors;
using Skimart.Extensions.Migrations;
using Skimart.Infrastructure.DependencyInjection;
using Skimart.Infrastructure.Logging;
using Skimart.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppControllers();
builder.Services.AddSwaggerDocumentation();

builder.Services.AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddPresentationServices();

builder.AddAppAuthentication()
    .AddAppAuthorization();

builder.AddCorsPolicies();
builder.Host.BootstrapLogger();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
await app.Services.MigrateDbs();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.AddCorsPolicies();

app.MapControllers();

app.Run();