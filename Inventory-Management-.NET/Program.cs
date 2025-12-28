using CloudinaryDotNet;
using Inventory_Management_.NET.Auth;
using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Services;
using Inventory_Management_.NET.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    "Server=.;Database=InventoryManagement;Trusted_Connection=True;TrustServerCertificate=True")
);
builder.Services.AddScoped<DashBoardService>();
builder.Services.AddScoped<ProductServices>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<CloudinaryService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddScoped<OrderServices>();



builder.Services.Configure<CloudinarySetting>(
    builder.Configuration.GetSection("CloudinarySettings")
);

builder.Services.AddSingleton<Cloudinary>(sp =>
{
    var config = sp.GetRequiredService<IOptions<CloudinarySetting>>().Value;

    var account = new Account(
        config.CloudName,
        config.ApiKey,
        config.ApiSecret
        );

    return new Cloudinary(account);
});

builder.Services.AddJwtAuth(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
