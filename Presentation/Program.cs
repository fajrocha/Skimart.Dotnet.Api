using Application.DependencyInjection;
using Infrastructure.DependencyInjection;
using Infrastructure.Logging;
using Presentation.DependencyInjection;
using Presentation.Extensions;
using Presentation.Extensions.Auth;
using Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppControllers();
builder.Services.AddSwaggerDocumentation();

builder.Services.AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddPresentationServices();

builder.AddAppAuthentication()
    .AddAppAuthorization();

builder.Host.BootstrapLogger();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();