using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MySQLContext>(options => 
    options
    .UseMySql(connection, new MySqlServerVersion(new Version(8,0,28))));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapControllers();

app.MapPost("/api/v1/Product", async (ProductVO vo, IProductRepository _repository) =>
{
    if (vo == null) return Results.BadRequest();
    var product = await _repository.Create(vo);
    return Results.Ok(product);
})
.WithName("CreateProduct");

app.MapGet("/api/v1/Product/{id}", async (long id, IProductRepository _repository) =>
{
    var product = await _repository.GetById(id);
    if (product == null) return Results.NotFound();
    return Results.Ok(product);
})
.WithName("GetProductById");

app.MapGet("/api/v1/Product", async (IProductRepository _repository) =>
{
    var products = await _repository.GetAll();
    return Results.Ok(products);
})
.WithName("GetAllProduct");

app.MapPut("/api/v1/Product", async (ProductVO vo, IProductRepository _repository) =>
{
    if (vo == null) return Results.BadRequest();
    var product = await _repository.Update(vo);
    return Results.Ok(product);
})
.WithName("PutProduct");

app.MapDelete("/api/vi/product/{id}", async (long id, IProductRepository _repository) =>
{
    if (id <= 0) return Results.BadRequest();
    var result = await _repository.Delete(id);
    if (!result) return Results.BadRequest();
    return Results.NoContent();
})
.WithName("DeleteProduct");

app.Run();

