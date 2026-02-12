using Microsoft.AspNetCore.Components.Web;
using Serena.Profiles;
using Serena.Service;
using SerenaBlazor;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);



builder.WebHost.UseUrls("https://localhost:7171", "http://localhost:5159");


builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();



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


//Adiciona suporte a depuração/execução do Blazor
builder.Services.AddRazorComponents();

var app = builder.Build();


// Pipeline HTTP

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles(); // Arquivos do engine Blazor
app.UseStaticFiles(); //Arquivos estáticos (CSS, JS, imagens, etc)



app.UseRouting();

// Necessário para componentes interativos no .NET 8
app.UseAntiforgery();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies();

app.Run();
