using BookStoreDK.BL.Interfaces;
using BookStoreDK.BL.Services;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.DL.Repositories.InMemoryRepositories;

namespace BookStoreDK
{
    public static class IoCContainer
    {
        public static WebApplication AddServicesAndBuild(this WebApplicationBuilder builder)
        {
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

           
            // add services
            builder.Services.AddSingleton<IPersonService, PersonService>();
            builder.Services.AddSingleton<IAuthorService, AuthorService>();
            builder.Services.AddSingleton<IBookService, BookService>();

            // add repositories
            builder.Services.AddSingleton<IPersonRepository, PersonInMemoryRepository>();
            builder.Services.AddSingleton<IAuthorRepository, AuthorInMemoryRepository>();
            builder.Services.AddSingleton<IBookRepository, BookInMemoryRepository>();


            return builder.Build();

        }
    }
}
