using Store.DataAccess;
using Microsoft.EntityFrameworkCore;
using Store.Core.Abstractions.Repository;
using Store.DataAccess.Repositories;
using Store.Application.Abstractions.Admin;
using Store.Application.Services.Admin;
using Store.Application.Abstractions.User;
using Store.Application.Services.User;

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

//Repo
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();

//Admin services
builder.Services.AddScoped<IAdminCategoryService, AdminCategoryService>();
builder.Services.AddScoped<IAdminOrderService, AdminOrderService>();
builder.Services.AddScoped<IAdminOrderItemService, AdminOrderItemService>();
builder.Services.AddScoped<IAdminProductService, AdminProductService>();
builder.Services.AddScoped<IAdminBrandService, AdminBrandService>();

//User services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

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

app.UseCors("AllowReactApp");
//app.UseCors("AllowAll");//ReactApp

app.UseAuthorization();

app.MapControllers();

//for front
//app.UseCors(x =>
//{
//    x.WithHeaders().AllowAnyHeader();
//    x.WithOrigins("http//local");//puth from front
//    x.WithMethods().AllowAnyMethod();
//});
//app.UseCors("AllowAll");//ReactApp

app.Run();
