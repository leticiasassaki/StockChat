using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockChat.Domain.Interfaces.Infra.Queue;
using StockChat.Domain.Interfaces.Infra.Repository;
using StockChat.Domain.Interfaces.Service;
using StockChat.Infrastructure.Data;
using StockChat.Infrastructure.Queue;
using StockChat.Infrastructure.Repositories;
using StockChat.Service.Chat;

namespace StockChat.IoC.Initializers
{
    public class InitializeServices
    {
        private readonly IServiceCollection _services;

        public InitializeServices(IServiceCollection services)
        {
            _services = services;
        }

        public InitializeServices Initialize(IConfiguration configuration)
        {
            _services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            _services.AddDatabaseDeveloperPageExceptionFilter();

            _services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            _services.AddSingleton<IChatUserService, ChatUserService>();
            _services.AddScoped<IChatService, ChatService>();

            _services.AddScoped<IMessageRepository, MessageRepository>();
            _services.AddScoped<IQueue, Queue>();
            _services.AddSingleton<IChatConfigurationService, ChatConfigurationService>();

            _services.AddSignalR();

            _services.AddHttpClient("StockAPI", client =>
            {
                client.BaseAddress = new Uri(configuration["Urls:StockAPI"]);
            });

            return this;
        }

        public InitializeServices StartConsumer<T>() where T : BackgroundService
        {
            _services.AddHostedService<T>();
            return this;
        }
    }
}
