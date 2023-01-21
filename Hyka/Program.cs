using Hyka.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using System.Security.Claims;
using Hyka.Areas.Identity.RolesDefinition;
using Hyka.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Identity.UI.Services;

/*
    vs-code
--------------------------------------------------------
dotnet tool update --global dotnet-ef
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

# region Globalization
builder.Services.AddLocalization(option => { option.ResourcesPath = "Resources"; });
builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedLanguages = new List<CultureInfo>
        {
            new CultureInfo("es-CO"),
            new CultureInfo("en-US")
        };
        options.DefaultRequestCulture = new RequestCulture("es-CO", "es-CO");
        options.SupportedCultures = supportedLanguages;
        options.SupportedUICultures = supportedLanguages;
    }
);
# endregion

# region DbConfigration
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection");
builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.UseSqlServer(connectionString);
        // !Enable database update 
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    );
# endregion

# region Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    options =>
    {
        options.User.RequireUniqueEmail = true;
        // !Convert to UTC-05:00 and Lockut 5 minutes
        options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 5, 0);
    }
    )
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
# endregion

# region JWT
var isUserSignInKey = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Keys")["TokenSignIn"]);
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
# endregion

# region API's
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
  {
      // this defines a CORS policy called "default"
      options.AddPolicy("default", policy =>
      {
          policy.WithOrigins("https://apimocha.com/districtz/districts")
             .AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
      });
  });
# endregion

# region Session & Authentication 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Roles.ADMIN, policy => policy.RequireClaim(ClaimTypes.Role, Roles.ADMIN));
        options.AddPolicy(Roles.BLOCKBUSTER, policy => policy.RequireClaim(ClaimTypes.Role, Roles.BLOCKBUSTER));
    }
);
# endregion

# region Services
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ITerritoryService, TerritoryService>();
builder.Services.AddScoped<ITariffService, TariffService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<IIngressService, IngressService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
# endregion

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
app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestLocalization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}"
);

app.MapRazorPages();
app.Run();
