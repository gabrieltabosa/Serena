using Serena.Profiles;
using Serena.Service;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


// MVC + JSON

builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

// AutoMapper

builder.Services.AddAutoMapper(typeof(DenunciaProfile));
builder.Services.AddAutoMapper(typeof(UserProfile));


// Configuração da API Gateway

var apiBase = builder.Configuration["ApiGateway:BaseUrl"];
if (string.IsNullOrWhiteSpace(apiBase))
    throw new InvalidOperationException("ApiGateway:BaseUrl não configurado.");

builder.Services.AddHttpClient("ApiGateway", client =>
{
    client.BaseAddress = new Uri(apiBase);
    client.Timeout = TimeSpan.FromSeconds(100);
});


// Cache em memória (Sessões)

builder.Services.AddMemoryCache();


// Services da aplicação

builder.Services.AddScoped<IUserApiClient, UserApiClient>();
builder.Services.AddScoped<IDenunciaService, DenunciaService>();

var app = builder.Build();

// Pipeline HTTP

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ❌ NÃO usar autenticação/autorização
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
