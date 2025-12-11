using Serena.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var apiBase = builder.Configuration["ApiGateway:BaseUrl"];
if (string.IsNullOrWhiteSpace(apiBase)) throw new InvalidOperationException("ApiGateway:BaseUrl não configurado.");

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient("ApiGateway", client =>
{
    client.BaseAddress = new Uri(apiBase);
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddScoped<IUserApiClient, UserApiClient>();
builder.Services.AddScoped<IDenunciaService, DenunciaService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
