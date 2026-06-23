using BRICOMA.ECOMMERCE.Business.Interfaces;
using BRICOMA.ECOMMERCE.Business.Repositories;
using BRICOMA.ECOMMERCE.Business.Services;
using BRICOMA.ECOMMERCE.Data.ApplicationUser;
using BRICOMA.ECOMMERCE.Data.Contexts;
using BRICOMA.ECOMMERCE.Models.Settings;
using BRICOMA.ECOMMERCE.Web.Authorization;
using BRICOMA.ECOMMERCE.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BRICOMAFIDELITEContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BRICOMAFIDELITEContext"),
        sql => sql.UseCompatibilityLevel(120)));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BRICOMAFIDELITEContext"),
        sql => sql.UseCompatibilityLevel(120)));

builder.Services.AddDbContext<BRICOMAMARKETContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BRICOMAMARKETContext"),
        sql => sql.UseCompatibilityLevel(120)));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Re-vérifie périodiquement la validité de la session (utilisé pour éjecter
// immédiatement un compte suspendu déjà connecté). N'affecte pas le login.
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(1);
});

builder.Services.AddScoped<IClaimsTransformation, PermissionClaimsTransformation>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

// Règle globale : toute page exige une session connectée par défaut.
// Les pages marquées [AllowAnonymous] (ex. Login) restent accessibles.
// Sinon, redirection automatique vers /Account/Login.
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

//pour les services injecté déja 
builder.Services.AddScoped<IClienteBORepository, ClienteBORepository>();
builder.Services.AddScoped<IClienteBOService, ClienteBOService>();
builder.Services.AddScoped<IPermissionBORepository, PermissionBORepository>();
builder.Services.AddScoped<IPermissionBOService, PermissionBOService>();
builder.Services.AddScoped<IMarketBORepository, MarketBORepository>();
builder.Services.AddScoped<IOtpRepository, OtpRepository>();
builder.Services.AddScoped<IOtpService, OtpService>();

builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection(TwilioSettings.SectionName));
builder.Services.AddScoped<IWhatsAppService, WhatsAppService>();

builder.Services.Configure<MediaSettings>(builder.Configuration.GetSection(MediaSettings.SectionName));
builder.Services.AddScoped<ICardImageService, CardImageService>();

var app = builder.Build();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
