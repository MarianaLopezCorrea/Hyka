using Hyka.Data;
using Hyka.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hyka.Areas.Identity.PoliciesDefinition;
using System.Security.Claims;
using Hyka.Areas.Identity.RolesDefinition;
using Hyka.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Builder;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

/*
    vs-code
--------------------------------------------------------
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet-ef migrations add [name]
dotnet-ef database update
dotnet-ef migrations remove
--------------------------------------------------------
    vs-2022
--------------------------------------------------------
PM> add-migrations [name]
PM> update-database 
*/

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(option => { option.ResourcesPath = "Resources"; });
builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(
    option =>
    {
        var supportedLanguages = new List<CultureInfo>
        {
            new CultureInfo("es-CO"),
            new CultureInfo("en-US")
        };
        option.DefaultRequestCulture = new RequestCulture("es-CO");
        option.SupportedCultures = supportedLanguages;
        option.SupportedUICultures = supportedLanguages;
    }
);

var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection");
var isUserSignInKey = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Keys")["TokenSignIn"]);

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(connectionString)
    );

builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    options =>
        options.SignIn.RequireConfirmedAccount = false
    )
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
            var user = userManager.GetUserAsync(context.HttpContext.User);
            if (user == null) context.Fail("Unauthorized");
            return Task.CompletedTask;
        }
    };
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(isUserSignInKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
  {
      // this defines a CORS policy called "default"
      options.AddPolicy("default", policy =>
      {
          policy.WithOrigins("https://api.qrserver.com/v1/create-qr-code/")
              .AllowAnyHeader()
              .AllowAnyMethod();
      });
  });

// MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Roles.ADMIN, policy => policy.RequireClaim(ClaimTypes.Role, Roles.ADMIN));
        options.AddPolicy(Roles.BLOCKBUSTER, policy => policy.RequireClaim(ClaimTypes.Role, Roles.BLOCKBUSTER));

        options.AddPolicy(Policy.REQUIRE_ADMIN, policy => policy.RequireRole(Roles.ADMIN));
        options.AddPolicy(Policy.REQUIRE_BLOCKBUSTER, policy => policy.RequireRole(Roles.BLOCKBUSTER));
    }
);

// Dependency Injection
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ITerritoryService, TerritoryService>();
builder.Services.AddScoped<ITariffService, TariffService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<IIngressService, IngressService>();
builder.Services.AddScoped<IReportService, ReportService>();

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
app.UseRequestLocalization();

app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}"
);

app.MapRazorPages();
app.Run();
