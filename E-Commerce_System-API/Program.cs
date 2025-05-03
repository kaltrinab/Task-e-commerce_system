using System.Text;
using E_Commerce_System_API.Application.Interfaces;
using E_Commerce_System_API.Application.Services;
using E_Commerce_System_API.Infrastructure.Context;
using E_Commerce_System_API.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IDapperContext, DapperContext>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<JWTService>();

builder.Services.AddHostedService<OrderStatusService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();


//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//to initialize the database from .sqlfile
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<IDapperContext>();
    DatabaseInitializer.RunSqlScript(context, "omsdb.sql");
}

app.Run();
