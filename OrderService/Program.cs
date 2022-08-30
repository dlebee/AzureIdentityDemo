using OrderService.Integration;
using OrderService.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

builder.Services.AddMemoryCache();
builder.Services.AddTransient<AuthenticateViaAzureManagedIdentity>();
builder.Services
    .AddHttpClient("CustomerSupport", configureClient =>
    {
        configureClient.BaseAddress = new Uri(builder.Configuration["CustomerService:Endpoint"]);
    })
    .AddHttpMessageHandler<AuthenticateViaAzureManagedIdentity>();
builder.Services.AddTransient<ICustomerSupportIntegrationService, CustomerSupportIntegrationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
