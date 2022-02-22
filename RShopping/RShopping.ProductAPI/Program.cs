using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RShopping.ProductAPI.Config;
using RShopping.ProductAPI.Data.ValueObjects;
using RShopping.ProductAPI.Models.Context;
using RShopping.ProductAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

//Connection String
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

//Mapper
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

//builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "RShopping.ProductAPI",
        Version = "v1",
    });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer' [space] and your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header
        },
        new List<string>()
      }
    });
});

builder.Services.AddDbContext<MySQLContext>(options => 
    options
    .UseMySql(connection, new MySqlServerVersion(new Version(8,0,28))));


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(x =>
    {
        x.Authority = "https://localhost:5210/";
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "RShopping");
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();

app.MapPost("/api/v1/Product", async (ProductVO vo, IProductRepository _repository) =>
{
    if (vo == null) return Results.BadRequest();
    var product = await _repository.Create(vo);
    return Results.Ok(product);
})
.WithName("CreateProduct")
.RequireAuthorization();

app.MapGet("/api/v1/Product/{id}", async (long id, IProductRepository _repository) =>
{
    var product = await _repository.GetById(id);
    if (product == null) return Results.NotFound();
    return Results.Ok(product);
})
.WithName("GetProductById")
.RequireAuthorization();

app.MapGet("/api/v1/Product", async (IProductRepository _repository) =>
{
    var products = await _repository.GetAll();
    return Results.Ok(products);
})
.WithName("GetAllProduct")
.RequireAuthorization();

app.MapPut("/api/v1/Product", async (ProductVO vo, IProductRepository _repository) =>
{
    if (vo == null) return Results.BadRequest();
    var product = await _repository.Update(vo);
    return Results.Ok(product);
})
.WithName("PutProduct")
.RequireAuthorization();

app.MapDelete("/api/v1/product/{id}", async (long id, IProductRepository _repository) =>
{
    if (id <= 0) return Results.BadRequest();
    var result = await _repository.Delete(id);
    if (!result) return Results.BadRequest();
    return Results.Ok(result);
})
.WithName("DeleteProduct")
.RequireAuthorization();

app.Run();

