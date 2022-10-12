using BookStoreDK.BL.Interfaces;
using BookStoreDK.BL.Kafka;
using BookStoreDK.BL.Services;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.DL.Repositories.MsSql;
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
               .AddSingleton<KafkaConsumer<int, Book>>()
               .AddSingleton<KafkaProducer<int, Book>>();
               
            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services
                .AddScoped<IEmployeeService, EmployeeService>()
                .AddScoped<IUserInfoService, EmployeeService>()
                .AddTransient<IIDentityService,IdentityService>();
           
            return services;
        }
    }
}
