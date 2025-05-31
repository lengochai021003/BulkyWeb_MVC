using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Microsoft.EntityFrameworkCore.Internal;
using BulkyBook.DataAccess.DbInitialzer;
using BulkyBook.DataAccess.NewFolder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddRazorPages();
builder.Host.ConfigureAppConfiguration((context, config) =>
{
    if (context.HostingEnvironment.IsDevelopment())
    {
        config.AddUserSecrets<Program>();
    }
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";

});

builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "1360049208612063";
    option.AppSecret = "cb3cfd2296a77005bc224e3833a9a891";
});

builder.Services.AddScoped<IDbInitializer,DbInitializer>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromSeconds(100);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});


builder.Services.AddScoped<IEmailSender,EmailSender>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
app.MapRazorPages();
app.UseRouting();
app.UseAuthentication();
app.UseSession();
SeedDatabase();
app.UseAuthorization();
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();


void SeedDatabase()
{
    using(var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initializer();
    }
}