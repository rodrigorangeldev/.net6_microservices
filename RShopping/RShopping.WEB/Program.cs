using Microsoft.AspNetCore.Authentication;
using RShopping.WEB.Services;
using RShopping.WEB.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(a =>
{
    a.DefaultScheme = "Cookies";
    a.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
.AddOpenIdConnect("oidc", idc =>
{
    idc.Authority = builder.Configuration["ServiceUrls:IdentityServer"];
    idc.GetClaimsFromUserInfoEndpoint = true;
    idc.ClientId = "RShopping";
    idc.ClientSecret = "8usaduf88a998$asd";
    idc.ResponseType = "code";
    idc.ClaimActions.MapJsonKey("role", "role", "role");
    idc.ClaimActions.MapJsonKey("sub", "sub", "sub");
    idc.TokenValidationParameters.NameClaimType = "name";
    idc.TokenValidationParameters.RoleClaimType = "role";
    idc.Scope.Add("RShopping");
    idc.SaveTokens = true;
    idc.RequireHttpsMetadata = false;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IProductService, ProductService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
