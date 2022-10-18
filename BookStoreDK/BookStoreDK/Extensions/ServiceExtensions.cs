using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Repositories.MongoRepositories;
using BookStoreDK.BL.Services;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.DL.Repositories.MsSql;
using BookStoreDK.KafkaCache;
using BookStoreDK.Models.Configurations;
using BookStoreDK.Models.Models;

namespace BookStoreDK.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services
               .AddSingleton<IPersonRepository, PersonRepository>()
               .AddSingleton<IAuthorRepository, AuthorRepository>()
               .AddSingleton<IBookRepository, BookRepository>()
               .AddScoped<IUserInfoStore, UserInfoStore>()
               .AddSingleton<IEmployeeRepository, EmployeeRepository>()
               .AddSingleton<IPurchaseRepository, PurchaseRepository>()
               .AddScoped<IShoppingCartRepository,ShoppingCartRepository>();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services
                .AddScoped<IEmployeeService, EmployeeService>()
                .AddScoped<IUserInfoService, EmployeeService>()
                .AddScoped<IPurchaseService, PurchaseService>()
                .AddScoped<IBookService, BookService>()
                .AddTransient<IIDentityService, IdentityService>()
                .AddScoped<IShoppingCartService, ShoppingCartService>();

            return services;
        }

        public static IServiceCollection RegisterKafkaConsumers(this IServiceCollection services)
        {
            services
                .AddSingleton<KafkaConsumer<int, Book, KafkaBookConsumerSettings>>();

            return services;
        }
    }
}
