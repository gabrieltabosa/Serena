using Microsoft.AspNetCore.Authentication.Cookies;
using Serena.Profiles;
using Serena.Service;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });
// registra profiles do assembly onde DenunciaProfile está
builder.Services.AddAutoMapper(typeof(DenunciaProfile));
// registra profiles do assembly onde DenunciaProfile está
builder.Services.AddAutoMapper(typeof(UserProfile));

var apiBase = builder.Configuration["ApiGateway:BaseUrl"];
if (string.IsNullOrWhiteSpace(apiBase)) throw new InvalidOperationException("ApiGateway:BaseUrl não configurado.");

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient("ApiGateway", client =>
{
    client.BaseAddress = new Uri(apiBase);
    client.Timeout = TimeSpan.FromSeconds(100);
});

builder.Services.AddScoped<IUserApiClient, UserApiClient>();
builder.Services.AddScoped<IDenunciaService, DenunciaService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User";
        options.AccessDeniedPath = "/User";

        // Garante os 10 minutos de expiração globalmente
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        options.SlidingExpiration = true;
    });

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
