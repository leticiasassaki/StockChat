using StockChat.MessageConsumer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<InsertMessageConsumer>();
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.Run();