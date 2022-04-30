using Hyka.Data;
using Hyka.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

builder.Services.AddDbContext<ApplicationDbContext>
    (
        options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("JWTSQLS")
        )
    );

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
// .AddDefaultTokenProviders();

// Adding Authentication
builder.Services.AddAuthentication(
// options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
// }
)
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    // Validate if the user actually is sign in
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

    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };


});

// API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<Territory>();
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
        options.AddPolicy(Roles.Admin, policy => policy.RequireClaim(ClaimTypes.Role, Roles.Admin));
        options.AddPolicy(Roles.Blockbuster, policy => policy.RequireClaim(ClaimTypes.Role, Roles.Blockbuster));

        options.AddPolicy(Roles.RequireAdmin, policy => policy.RequireRole(Roles.RequireAdmin));
        options.AddPolicy(Roles.RequireBlockbuster, policy => policy.RequireRole(Roles.RequireBlockbuster));
    }
);


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

app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();


