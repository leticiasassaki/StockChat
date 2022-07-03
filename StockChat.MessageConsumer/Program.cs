using Microsoft.EntityFrameworkCore;
using StockChat.Domain.Interfaces.Infra.Repository;
using StockChat.Infrastructure.Data;
using StockChat.Infrastructure.Repositories;
using StockChat.MessageConsumer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddHostedService<InsertMessageConsumer>();
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.Run();