using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Business.Repositories;
using BRICOMA.ECOMMERCE.Business.Services;
using BRICOMA.ECOMMERCE.Data.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<BRICOMAFIDELITEContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BRICOMAFIDELITEContext")));

// Repositories
builder.Services.AddScoped<ICarteRepository, CarteRepository>();

// Services
builder.Services.AddScoped<ICarteService, CarteService>();

var app = builder.Build();

// Swagger (disponible en dev et prod pour l'instant)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BRICOMA.ECOMMERCE API v1");
    c.RoutePrefix = "swagger";
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
