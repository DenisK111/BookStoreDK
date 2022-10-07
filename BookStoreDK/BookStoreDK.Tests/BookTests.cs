using AutoMapper;
using BookStoreDK.AutoMapper;
using BookStoreDK.BL.Services;
using BookStoreDK.Controllers;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookStoreDK.Tests
{
    public class BookTests
    {
        private IList<Author> _authors = new List<Author>()
        {
            new Author() { Name = "Pesho",Id=1,Age=16,DateOfBirth=new DateTime(1990,12,14),NickName="Pepi"},
            new Author() { Name = "Gosho",Id=3,Age=20,DateOfBirth=new DateTime(1980,5,1),NickName="Gogo"},
            new Author() { Name = "Ivan",Id=2,Age=22,DateOfBirth=new DateTime(1945,9,22),NickName="Vanka"},
        };

        private IList<Book> _books = new List<Book>()
        {
            new Book()
            {
                Id = 13,
                Title = "Patilansko Carstvo",
                AuthorId = 2,
                Price = 14,
                Quantity = 100,
                LastUpdated = DateTime.Now,
            }
            ,
             new Book()
            {
                Id = 1,
                Title = "Malechko",
                AuthorId = 1,
                 Price = 14,
                Quantity = 100,
                LastUpdated = new DateTime(1990,10,4),
            }
             ,
              new Book()
            {
                Id = 43,
                Title = "Bai Ganio",
                AuthorId = 3,
                 Price = 14,
                Quantity = 100,
                LastUpdated = DateTime.Now,
            }
        };

        private readonly IMapper _mapper;
        private Mock<ILogger<BookController>> _loggerMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        public BookTests()
        {
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMappings());
            });

            _mapper = mockMapperConfig.CreateMapper();
            _loggerMock = new Mock<ILogger<BookController>>();

            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
        }

        [Fact]
        public async Task Book_GetAll_Count_Check()
        {
            //Setup

            var expectedCount = 3;
            _bookRepositoryMock.Setup(x => x.GetAll())
                .ReturnsAsync(_books);

            //Inject

            var controller = GetBookController();

            //Act

            var controllerResult = await controller.Get();

            //Assert

            var okObjectResult = controllerResult as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var response = okObjectResult!.Value as BookCollectionResponse;

            Assert.NotNull(response);
            var books = response!.Model;
            Assert.NotNull(books);
            Assert.NotEmpty(books);
            Assert.Equal(expectedCount, books!.Count());

        }

        [Fact]

        public async Task Book_GetById_Ok()
        {
            //setup
            var bookId = 1;
            var expectedBook = _books.First(x => x.Id == bookId);

            _bookRepositoryMock.Setup(x => x.GetById(bookId))
                .ReturnsAsync(expectedBook);

            //inject
            var controller = GetBookController();

            //act
            var result = await controller.GetById(bookId);
            //Assert
            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var bookResult = okObjectResult!.Value as BookResponse;

            Assert.NotNull(bookResult);

            var book = bookResult!.Model;

            Assert.NotNull(book);
            Assert.Equal(bookId, book!.Id);
        }

        [Fact]

        public async Task Book_GetById_NotFound()
        {
            //setup
            var bookId = 900;
            var expectedBook = _books.FirstOrDefault(x => x.Id == bookId);
            var expectedMessage = "Id does not exist";

            _bookRepositoryMock.Setup(x => x.GetById(bookId))
                .ReturnsAsync(expectedBook);

            //inject
            var controller = GetBookController();

            //act
            var result = await controller.GetById(bookId);
            //Assert
            var notFoundObjectResult = result as NotFoundObjectResult;

            Assert.NotNull(notFoundObjectResult);

            var bookResult = notFoundObjectResult!.Value as BookResponse;

            Assert.NotNull(bookResult);

            var message = bookResult!.Message;
            var book = bookResult!.Model;

            Assert.Null(book);
            Assert.Equal(expectedBook, book);
            Assert.Equal(expectedMessage, message);
        }

        [Fact]

        public async Task Book_Add_Ok()
        {
            //setup
            var request = new AddBookRequest()
            {
                Title = "Test Book",
                AuthorId = 1,
                Quantity = 1,
                Price = 1
            };

            var bookToAdd = new Book()
            {
                Title = "Test Book",
                AuthorId = 1,
                Quantity = 1,
                Price = 1
            };

            _bookRepositoryMock
            .Setup(x => x.Add(It.IsAny<Book>()))
            .Callback(() =>
            {
                _books.Add(bookToAdd);

            }).ReturnsAsync(() => _books.FirstOrDefault(x => x.Title == bookToAdd.Title));

            _authorRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(_authors.FirstOrDefault(x => x.Id == bookToAdd.AuthorId));

            // inject 

            var controller = GetBookController();

            //act

            var result = await controller.Add(request);

            //assert

            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult!.Value as BookResponse;
            Assert.NotNull(resultValue);

            var resultBook = resultValue!.Model;
            Assert.NotNull(resultBook);

            Assert.Equal(bookToAdd, resultBook);

            Assert.Equal(4, _books.Count());
        }

        [Fact]
        public async Task Book_Add_WhenExists_BadRequest()
        {
            //setup

            var expectedErrorMessage = "Book already exists";
            var request = new AddBookRequest()
            {
                Title = "Bai Ganio",
                AuthorId = 1,
                Quantity = 1,
                Price = 1
            };

            var bookToAdd = new Book()
            {
                Title = "Bai Ganio",
                AuthorId = 1,
                Quantity = 1,
                Price = 1
            };

            _bookRepositoryMock
                .Setup(x => x.GetBookByTitle(request.Title))
                .ReturnsAsync(_books.FirstOrDefault(x => x.Title == request.Title));

            // inject 

            var controller = GetBookController();

            //act

            var result = await controller.Add(request);

            //assert

            var badRequestObjectResult = result as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);

            var resultValue = badRequestObjectResult!.Value as ErrorResponse;

            Assert.NotNull(resultValue);
            Assert.Equal(expectedErrorMessage, resultValue!.Error);
            Assert.Equal(3, _authors.Count());
        }
        [Fact]
        public async Task Book_Add_WhenAuthorDoesNotExist_BadRequest()
        {
            //setup

            var expectedErrorMessage = "Author Id Does not Exist";
            var request = new AddBookRequest()
            {
                Title = "Bai Ganio Not Exists",
                AuthorId = 68,
                Quantity = 1,
                Price = 1
            };

            var bookToAdd = new Book()
            {
                Title = "Bai Ganio Not Exists",
                AuthorId = 68,
                Quantity = 1,
                Price = 1
            };

            _bookRepositoryMock
                .Setup(x => x.GetBookByTitle(request.Title))
                .ReturnsAsync(_books.FirstOrDefault(x => x.Title == request.Title));

            _authorRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(_authors.FirstOrDefault(x => x.Id == bookToAdd.AuthorId));

            // inject 

            var controller = GetBookController();

            //act

            var result = await controller.Add(request);

            //assert

            var badRequestObjectResult = result as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);

            var resultValue = badRequestObjectResult!.Value as ErrorResponse;

            Assert.NotNull(resultValue);
            Assert.Equal(expectedErrorMessage, resultValue!.Error);
            Assert.Equal(3, _authors.Count());
        }

        [Fact]
        public async Task Book_Update_Ok()
        {
            // setup
            //setup
            var request = new UpdateBookRequest()
            {
                Id = 1,
                Title = "Test Book",
                AuthorId = 1,
                Quantity = 1,
                Price = 1
            };

            var updatedBook = new Book()
            {
                Id = 1,
                Title = "Test Book",
                AuthorId = 1,
                Quantity = 1,
                Price = 1,
                LastUpdated = new DateTime(1990, 10, 4)
            };

            var bookToupdate = _books.FirstOrDefault(x => x.Id == request.Id);

            _bookRepositoryMock
            .Setup(x => x.Update(It.IsAny<Book>()))
            .Callback(() =>
            {
                _books.Remove(_books.FirstOrDefault(x => x.Id == request.Id)!);
                _books.Add(updatedBook);

            }).ReturnsAsync(() => _books.FirstOrDefault(x => x.Id == bookToupdate!.Id));

            _bookRepositoryMock
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(_books.FirstOrDefault(x => x.Id == request.Id));

            // inject 

            var controller = GetBookController();

            //act

            var result = await controller.Update(request);

            //assert

            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult!.Value as BookResponse;
            Assert.NotNull(resultValue);

            var resultBook = resultValue!.Model;
            Assert.NotNull(resultBook);

            Assert.Equal(updatedBook, resultBook);

            Assert.Equal(3, _books.Count());
        }

        [Fact]
        public async Task Book_Update_WhenNotExists()
        {
            var bookId = 150;
            var request = new UpdateBookRequest()
            {
                Id = bookId,
                Title = "Test Book",
                AuthorId = 1,
                Quantity = 1,
                Price = 1
            };
            var expectedErrorMessage = "Book does not exist";
            var bookToupdate = _books.FirstOrDefault(x => x.Id == bookId);

            _bookRepositoryMock
            .Setup(x => x.Update(It.IsAny<Book>()))
            .ReturnsAsync(() => _books.FirstOrDefault(x => x.Id == bookId));

            _bookRepositoryMock
               .Setup(x => x.GetById(It.IsAny<int>()))
               .ReturnsAsync(_books.FirstOrDefault(x => x.Id == bookId));

            // inject 

            var controller = GetBookController();

            //act

            var result = await controller.Update(request);

            //assert

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);

            var resultValue = badRequestObjectResult!.Value as ErrorResponse;
            Assert.NotNull(resultValue);

            var errorMessage = resultValue!.Error;

            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        [Fact]
        // TODO Make Method
        public async Task Book_Delete_Ok()
        {
            //setup
            var bookId = 1;
            var expectedBook = _books.First(x => x.Id == bookId);

            _bookRepositoryMock.Setup(x => x.Delete(bookId))
                .Callback(() =>
                {
                    _books.Remove(_books.First(x => x.Id == bookId));
                })
                .ReturnsAsync(() => expectedBook);

            //inject
            var controller = GetBookController();

            //act
            var result = await controller.Delete(bookId);
            //Assert
            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var bookResult = okObjectResult!.Value as BookResponse;

            Assert.NotNull(bookResult);

            var book = bookResult!.Model;

            Assert.NotNull(book);
            Assert.Equal(bookId, book!.Id);
        }

        [Fact]
        public async Task Book_Delete_NotFound()
        {
            //setup
            var bookId = 190;
            var expectedBook = _books.FirstOrDefault(x => x.Id == bookId);
            var expectedErrorMessage = "Id does not exist";

            _bookRepositoryMock.Setup(x => x.Delete(bookId))
                .ReturnsAsync(_books.FirstOrDefault(x => x.Id == bookId));

            //inject
            var controller = GetBookController();

            //act
            var result = await controller.Delete(bookId);
            //Assert
            var notFoundObjectResult = result as NotFoundObjectResult;

            Assert.NotNull(notFoundObjectResult);

            var bookResult = notFoundObjectResult!.Value as BookResponse;

            Assert.NotNull(bookResult);

            var book = bookResult!.Model;
            var errorMessage = bookResult!.Message;
            Assert.Null(book);
            Assert.Equal(expectedErrorMessage, errorMessage);
            Assert.Equal(3, _books.Count);
        }

        private BookController GetBookController()
        {
            var service = new BookService(_bookRepositoryMock.Object, _mapper, _authorRepositoryMock.Object);
           var controller = new BookController(null!);
            return controller;
        }
    }
}
