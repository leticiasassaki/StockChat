using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Identity.Web;
using StockChat.UI.Hubs;
using Microsoft.EntityFrameworkCore;
using StockChat.IoC.Initializers;
using StockChat.UI.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
new InitializeServices(builder.Services).Initialize(builder.Configuration)
                                   .StartConsumer<StockConsumer>();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();

app.MapHub<ChatHub>("/chathub");
app.MapFallbackToPage("/_Host");

app.Run();
