using DapperCRUDAPI.Models;
using DapperCRUDAPI.Repositories;
using DapperCRUDAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IDbConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    // Replace environment variable placeholders with actual values
    connectionString = connectionString?
        .Replace("${SQL_USER}", Environment.GetEnvironmentVariable("SQL_USER") ?? "defaultUser")
        .Replace("${SQL_PASSWORD}", Environment.GetEnvironmentVariable("SQL_PASSWORD") ?? "defaultPassword");

    return new SqlConnection(connectionString);
});


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/products", async (IProductService productService) =>
{
    var products = await productService.GetAllProductAsync();
    return Results.Ok(products);
});

app.MapGet("/products/{id:int}", async (int id, IProductService productService) =>
{
    try
    {
        var product = await productService.GetProductByIdAsync(id);
        return Results.Ok(product);
    }
    catch (NullReferenceException)
    {
        return Results.NotFound();
    }
});

app.MapPost("/products", async (Product product, IProductService productService) =>
{
    await productService.AddProductAsync(product);
    return Results.Created($"/products/{product.ProductID}", product);
});

app.MapPut("/products", async (int id, Product product, IProductService productService) =>
{
    await productService.UpdateProductAsync(product);
    return Results.NoContent();
});

app.MapDelete("/products/{id:int}", async (int id, IProductService productService) =>
{
    await productService.DeleteProductAsync(id);
    return Results.NoContent();
});

app.Run();
