using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Repositories.MongoRepositories;
using BookStoreDK.BL.Services;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.DL.Repositories.MsSql;
using BookStoreDK.KafkaCache;
using BookStoreDK.Models.Configurations;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Models.KafkaConsumerModels;
using BookStoreDK.Dataflow;
using BookStoreDK.BL.HttpClientProviders.Contracts;
using BookStoreDK.BL.HttpClientProviders.Implementations;

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
               .AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

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
            // DATAFLOW CONSUMERS
            services
                .AddSingleton<KafkaConsumer<Guid, PurchaseObject, KafkaPurchaseConsumerSettings>>()
                .AddSingleton<KafkaConsumer<int, BookDeliveryObject, KafkaBookDeliveryConsumerSettings>>();

            // CACHE CONSUMERS
            services
                .AddSingleton<KafkaConsumer<int, Book, KafkaBookConsumerSettings>>();

            return services;
        }

        public static IServiceCollection RegisterDataFlowHostedServices(this IServiceCollection services)
        {
            services
                .AddHostedService<DeliveryDataFlow>()
                .AddHostedService<PurchaseDataFlow>();

            return services;
        }

        public static IServiceCollection RegisterCaches(this IServiceCollection services)
        {
            services
                .AddSingleton<KafkaCache<int, Book, KafkaBookConsumerSettings>>();

            return services;
        }

        public static IServiceCollection RegisterHttpProviders(this IServiceCollection services)
        {
            services
                .AddSingleton<IAdditionalInfoProvider, AdditionalInfoProvider>();

            return services;
        }
    }
}
