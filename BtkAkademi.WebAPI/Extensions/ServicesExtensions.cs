
using BtkAkademi.Repositories.Contracts;
using BtkAkademi.Repositories.EFCore;
using BtkAkademi.Services;
using BtkAkademi.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BtkAkademi.WebAPI.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration configuration) => services.AddDbContext<RepositoryContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BtkDatabase")));
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager,RepositoryManager>();
        public static void ConfigureServiceManager(this IServiceCollection services) => 
            services.AddScoped<IServiceManager,ServiceManager>();
        public static void ConfigureLoggerServicer(this IServiceCollection services) =>
            services.AddSingleton<ILoggerService, LoggerManager>();
    }
}
