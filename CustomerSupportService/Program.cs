using CustomerSupportService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                },
                Scheme = "oauth2",
                Name = "oauth2",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        In = ParameterLocation.Header,
        Flows = new OpenApiOAuthFlows()
        {
            AuthorizationCode = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{builder.Configuration["TenantId"]}/oauth2/v2.0/authorize"),
                TokenUrl = new Uri($"https://login.microsoftonline.com/{builder.Configuration["TenantId"]}/oauth2/v2.0/token"),
                Scopes = new Dictionary<string, string>
                {
                    { $"{builder.Configuration["Audience"]}/roles", "Access to roles of user" }
                }
            }
        }
    });;
});

builder.Services.AddAuthorization(a =>
{
    a.AddPolicy("CustomerSupport", p =>
    {
        p.RequireAssertion(context =>
        {
            if (false == context.User.Identity?.IsAuthenticated)
                return false;

            var isAllowed = context.User
                .Claims
                .Where(t => t.Type.Contains("role"))
                .Any(t => t.Value.Contains("CustomerSupport"));

            return isAllowed;
        });
    });
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = $"https://login.microsoftonline.com/{builder.Configuration["TenantId"]}/v2.0";
        options.Audience = builder.Configuration["Audience"];
        options.TokenValidationParameters.ValidIssuers = new List<string> {
            $"https://sts.windows.net/{builder.Configuration["TenantId"]}/"
        };
    });

builder.Services.AddSingleton<ITicketService, TicketService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.OAuthClientId(builder.Configuration["ClientId"]);
        c.OAuthUsePkce();
        c.OAuthScopes($"{builder.Configuration["Audience"]}/roles");
        c.OAuthRealm("oauth2");
        c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
