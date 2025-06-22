using Store.DataAccess;
using Microsoft.EntityFrameworkCore;
using Store.Core.Abstractions.Repository;
using Store.DataAccess.Repositories;
using Store.Application.Abstractions.Admin;
using Store.Application.Services.Admin;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OnlineStoreDbContext>(
    options => options.UseNpgsql(connectionString + ";TrustServerCertificate=True"));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAdminCategoryService, AdminCategoryService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IAdminOrderService, AdminOrderService>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IAdminOrderItemService, AdminOrderItemService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAdminProductService, AdminProductService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//for front
//app.UseCors(x =>
//{
//    x.WithHeaders().AllowAnyHeader();
//    x.WithOrigins("http//local");//puth from front
//    x.WithMethods().AllowAnyMethod();
//});
app.UseCors("AllowReactApp");

app.Run();
