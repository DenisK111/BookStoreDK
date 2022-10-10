using BookStoreDK.BL.Interfaces;
using BookStoreDK.BL.Services;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.DL.Repositories.MsSql;

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
               .AddSingleton<IEmployeeRepository, EmployeeRepository>();
               
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
