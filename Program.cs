using Hyka.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/*
    vs-code
--------------------------------------------------------
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet-ef migrations add InitialCreate
dotnet-ef database update
--------------------------------------------------------
    vs-2022
--------------------------------------------------------
PM> add-migrations [name]
PM> update-database 
*/

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>
    (
        options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultSQLS")
        )
    );

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
