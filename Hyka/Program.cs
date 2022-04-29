using Hyka.Data;
using Hyka.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Hyka.Areas.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection");
var isUserSignInKey = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Keys")["TokenSignIn"]);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

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
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
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

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
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
    pattern: "{controller=Home}/{action=Index}"
);

app.MapRazorPages();
app.Run();
