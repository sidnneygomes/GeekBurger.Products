using AutoMapper;
using GeekBurger.Products.Extension;
using GeekBurger.Products.Repository;
using GeekBurger.Products.Repository.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDbContext<ProductsDbContext>(o => o.UseInMemoryDatabase("geekburger-products"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IProductChangedEventRepository, ProductChangedEventRepository>();

var app = builder.Build();
var scope = app.Services.CreateScope();
var productsDbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
productsDbContext.Seed();

var mvcCoreBuilder = builder.Services.AddMvcCore();

mvcCoreBuilder
    .AddFormatterMappings()
    //.AddJsonFormatters()
    .AddCors();

builder.Services.AddSwaggerGen();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
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
    pattern: "{controller=Home}/{action=Products}/{id?}");

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products");
});

app.Run();