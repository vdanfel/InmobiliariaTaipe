using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ILoginService,LoginService>();
builder.Services.AddScoped<ITablasService,TablasService>();
builder.Services.AddScoped<IPersonaService,PersonaService>();
builder.Services.AddScoped<IProgramaService,ProgramaService>();

builder.Services.AddScoped<SqlConnection>(c =>
{
    var connectionString = builder.Configuration.GetConnectionString("INMOBILIARIAConnection");
    return new SqlConnection(connectionString);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login/Ingresar";
                options.LogoutPath = "/Login/Logout";
                options.AccessDeniedPath = "/Home/Index";
            });

// Agrega el servicio de sesión
builder.Services.AddSession(options =>
{
    // Configura las opciones de la sesión según tus necesidades
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});

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

// Habilita el middleware de sesión
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
