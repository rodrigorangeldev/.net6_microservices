using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RShopping.IdentityServer.Config;
using RShopping.IdentityServer.Data;
using RShopping.IdentityServer.Data.Context;
using RShopping.IdentityServer.Initializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Connection String
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MySqlContext>(options =>
    options
    .UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 28))));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MySqlContext>()
    .AddDefaultTokenProviders();

var identityServerBuilder = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
})
.AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
.AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
.AddInMemoryClients(IdentityConfiguration.Clients)
.AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

identityServerBuilder.AddDeveloperSigningCredential();


var app = builder.Build();

var dbInitializeService = app.Services.CreateScope().ServiceProvider.GetService<IDbInitializer>();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

dbInitializeService.Initialize();

app.Run();
