using BookStoreDK.BL.Interfaces;
using BookStoreDK.BL.Services;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.DL.Repositories.InMemoryRepositories;

namespace BookStoreDK.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services
                .AddSingleton<IPersonRepository, PersonInMemoryRepository>()
                .AddSingleton<IAuthorRepository, AuthorInMemoryRepository>()
                .AddSingleton<IBookRepository, BookInMemoryRepository>();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IPersonService, PersonService>()
                .AddSingleton<IAuthorService, AuthorService>()
                .AddSingleton<IBookService, BookService>();

            return services;
        }
    }
}
