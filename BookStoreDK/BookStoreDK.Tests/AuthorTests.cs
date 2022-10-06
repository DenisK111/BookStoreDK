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
    public class AuthorTests
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
                LastUpdated = DateTime.Now,
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
        private Mock<ILogger<AuthorService>> _loggerMock;
        private Mock<ILogger<AuthorController>> _loggerAuthorControllerMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        public AuthorTests()
        {
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMappings());
            });

            _mapper = mockMapperConfig.CreateMapper();
            _loggerMock = new Mock<ILogger<AuthorService>>();

            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _loggerAuthorControllerMock = new Mock<ILogger<AuthorController>>();
        }

        [Fact]
        public async Task Author_GetAll_Count_Check()
        {
            //Setup

            var expectedCount = 3;
            _authorRepositoryMock.Setup(x => x.GetAll())
                .ReturnsAsync(_authors);

            //Inject

            var service = new AuthorService(_authorRepositoryMock.Object, _mapper, _bookRepositoryMock.Object, _loggerMock.Object);
            var controller = new AuthorController(_loggerAuthorControllerMock.Object, service);

            //Act

            var controllerResult = await controller.Get();

            //Assert

            var okObjectResult = controllerResult as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var response = okObjectResult.Value as AuthorsCollectionResponse;

            Assert.NotNull(response);
            var authors = response.Authors;
            Assert.NotNull(authors);
            Assert.NotEmpty(authors);
            Assert.Equal(expectedCount, authors.Count());

        }

        [Fact]

        public async Task Author_GetById_Ok()
        {
            //setup
            var authorId = 1;
            var expectedAuthor = _authors.First(x => x.Id == authorId);

            _authorRepositoryMock.Setup(x => x.GetById(authorId))
                .ReturnsAsync(expectedAuthor);

            //inject
            var controller = GetAuthorController();

            //act
            var result = await controller.GetById(authorId);
            //Assert
            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var authorResult = okObjectResult.Value as AuthorResponse;

            Assert.NotNull(authorResult);

            var author = authorResult.Author;

            Assert.NotNull(author);
            Assert.Equal(authorId, author.Id);

        }

        [Fact]

        public async Task Author_GetById_NotFound()
        {
            //setup
            var authorId = 4;
            var expectedAuthor = _authors.FirstOrDefault(x => x.Id == authorId);
            var expectedMessage = "Id does not exist";

            _authorRepositoryMock.Setup(x => x.GetById(authorId))
                .ReturnsAsync(expectedAuthor);

            //inject
            var controller = GetAuthorController();

            //act
            var result = await controller.GetById(authorId);
            //Assert
            var notFoundObjectResult = result as NotFoundObjectResult;

            Assert.NotNull(notFoundObjectResult);

            var authorResult = notFoundObjectResult.Value as AuthorResponse;

            Assert.NotNull(authorResult);
            Assert.Equal(expectedMessage, authorResult.Message);

            var author = authorResult.Author;

            Assert.Null(author);
            Assert.Equal(expectedAuthor, author);

        }

        [Fact]

        public async Task Author_Add_Ok()
        {
            //setup
            var request = new AddAuthorRequest()
            {
                Age = 14,
                Name = "Vanyo",
                NickName = "Pesho",
                DateOfBirth = new DateTime(1999, 10, 14),
            };

            var author = new Author()
            {
                Id = 14,
                Name = request.Name,
                Age = request.Age,
                DateOfBirth = request.DateOfBirth,
                NickName = request.NickName,

            };

            _authorRepositoryMock
            .Setup(x => x.Add(It.IsAny<Author>()))
            .Callback(() =>
        {
            _authors.Add(author);

        }).ReturnsAsync(() => _authors.FirstOrDefault(x => x.Id == author.Id));

            // inject 

            var controller = GetAuthorController();

            //act

            var result = await controller.Add(request);

            //assert

            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult!.Value as AuthorResponse;
            Assert.NotNull(resultValue);

            var resultAuthor = resultValue!.Author;
            Assert.NotNull(resultAuthor);

            Assert.Equal(author, resultAuthor);

            Assert.Equal(4, _authors.Count());
        }

        [Fact]
        public async Task Author_Add_WhenExists()
        {
            //setup

            var expectedErrorMessage = "Author already exists";
            var request = new AddAuthorRequest()
            {
                Age = 14,
                Name = "Pesho",
                NickName = "Vanka",
                DateOfBirth = new DateTime(1999, 10, 14),
            };

            var author = new Author()
            {
                Id = 14,
                Name = request.Name,
                Age = request.Age,
                DateOfBirth = request.DateOfBirth,
                NickName = request.NickName,

            };

            var authorMockObject = It.IsAny<Author>();

            _authorRepositoryMock
                .Setup(x => x.Add(authorMockObject))
                .Callback(() =>
                {
                    _authors.Add(author);

                }).ReturnsAsync(() => _authors.FirstOrDefault(x => x.Id == author.Id));

            _authorRepositoryMock
                .Setup(x => x.GetAuthorByName(request.Name))
                .ReturnsAsync(_authors.FirstOrDefault(x => x.Name == request.Name));

            // inject 

            var controller = GetAuthorController();

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
        public async Task Author_Update_Ok()
        {
            // setup

            var request = new UpdateAuthorRequest()
            {
                Id = 1,
                Age = 14,
                Name = "Vanyo",
                NickName = "Pesho",
                DateOfBirth = new DateTime(1999, 10, 14),
            };

            var author = new Author()
            {
                Id = request.Id,
                Name = request.Name,
                Age = request.Age,
                DateOfBirth = request.DateOfBirth,
                NickName = request.NickName,

            };

            _authorRepositoryMock
          .Setup(x => x.Update(It.IsAny<Author>()))
          .Callback(() =>
          {
              _authors.Remove(_authors.First(x => x.Id == author.Id));
              _authors.Add(author);

          }).ReturnsAsync(() => _authors.FirstOrDefault(x => x.Id == author.Id));

            _authorRepositoryMock
                .Setup(x => x.GetById(request.Id))
                .ReturnsAsync(_authors.FirstOrDefault(x => x.Id == request.Id));

            // inject 

            var controller = GetAuthorController();

            //act

            var result = await controller.Update(request);

            //assert

            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult.Value as AuthorResponse;
            Assert.NotNull(resultValue);

            var resultAuthor = resultValue.Author;

            Assert.NotNull(resultAuthor);
            Assert.Equal(author, resultAuthor);
            Assert.Equal(3, _authors.Count());
        }

        [Fact]
        public async Task Author_Update_WhenNotExists()
        {
            // setup

            var expectedErrorMessage = "Author does not exist";
            var request = new UpdateAuthorRequest()
            {
                Id = 16,
                Age = 14,
                Name = "Vanyo",
                NickName = "Pesho",
                DateOfBirth = new DateTime(1999, 10, 14),
            };

            var author = new Author()
            {
                Id = request.Id,
                Name = request.Name,
                Age = request.Age,
                DateOfBirth = request.DateOfBirth,
                NickName = request.NickName,

            };

            _authorRepositoryMock
          .Setup(x => x.Update(author))
          .ReturnsAsync(_authors.FirstOrDefault(x => x.Id == author.Id));

            // inject 

            var controller = GetAuthorController();

            //act

            var result = await controller.Update(request);

            //assert

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);

            var resultValue = badRequestObjectResult!.Value as ErrorResponse;
            Assert.NotNull(resultValue);


            Assert.Equal(expectedErrorMessage, resultValue!.Error);
            Assert.Equal(3, _authors.Count());
        }

        [Fact]
        // TODO Make Method
        public async Task Author_Delete_Ok()
        {
            //setup
            var authorId = 3;
            var expectedAuthor = _authors.FirstOrDefault(x => x.Id == authorId);

            _authorRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).Callback(() =>
            {
                _authors.Remove(_authors.FirstOrDefault(x => x.Id == authorId)!);
            })
                .ReturnsAsync(expectedAuthor);

            //inject
            var controller = GetAuthorController();

            //act
            var result = await controller.Delete(authorId);
            //Assert
            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var authorResult = okObjectResult!.Value as AuthorResponse;

            Assert.NotNull(authorResult);
            Assert.Equal(2, _authors.Count);

            var author = authorResult!.Author;

            Assert.NotNull(author);
            Assert.Equal(expectedAuthor, author);
        }

        [Fact]
        // TODO Make Method
        public async Task Author_Delete_NotFound()
        {
            //setup
            var authorId = 4;
            var expectedAuthor = _authors.FirstOrDefault(x => x.Id == authorId);
            var expectedMessage = "Id does not exist";

            _authorRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(expectedAuthor);

            //inject
            var controller = GetAuthorController();

            //act
            var result = await controller.Delete(authorId);
            //Assert

            var notFoundObjectResult = result as NotFoundObjectResult;

            Assert.NotNull(notFoundObjectResult);

            var authorResult = notFoundObjectResult!.Value as AuthorResponse;

            Assert.NotNull(authorResult);
            Assert.Equal(expectedMessage, authorResult!.Message);

            var author = authorResult.Author;

            Assert.Null(author);
            Assert.Equal(expectedAuthor, author);

        }

        [Fact]
        public async Task Author_Delete_AuthorHasBooks_BadRequest()
        {

            var authorId = 3;
            var expectedAuthor = _authors.FirstOrDefault(x => x.Id == authorId);
            var booksCount = 1;
            var expectedErrorMessage = "Cannot Delete Author With Books";

            _authorRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).Callback(() =>
            {
                _authors.Remove(_authors.FirstOrDefault(x => x.Id == authorId)!);
            })
                .ReturnsAsync(expectedAuthor);

            _bookRepositoryMock
                .Setup(x => x.GetBooksCountByAuthorId(It.IsAny<int>()))
                .ReturnsAsync(booksCount);

            //inject
            var controller = GetAuthorController();

            //act
            var result = await controller.Delete(authorId);
            //Assert
            var BadRequestObjectResult = result as BadRequestObjectResult;

            Assert.NotNull(BadRequestObjectResult);

            var authorResult = BadRequestObjectResult!.Value as ErrorResponse;

            Assert.NotNull(authorResult);
            Assert.Equal(3, _authors.Count);

            var errorMessage = authorResult!.Error;

            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        [Fact]

        public async Task Author_AddRange_Ok()
        {
            var authorsToAddRequest = new List<AddAuthorRequest>()
            {
                new AddAuthorRequest()
                {
                    Age = 14,
                    Name = "Vanka",
                    NickName = "Pesho",
                    DateOfBirth = new DateTime(1999, 10, 14),
                },
                 new AddAuthorRequest()
                {
                    Age = 15,
                    Name = "Vanyo",
                    NickName = "Pesho",
                    DateOfBirth = new DateTime(1999, 10, 14),
                },
            };

            var authorsToAdd = new List<Author>()
            {
                new Author()
                {

                    Age = 14,
                    Name = "Vanka",
                    NickName = "Pesho",
                    DateOfBirth = new DateTime(1999, 10, 14),
                },
                 new Author()
                {
                    Age = 15,
                    Name = "Vanyo",
                    NickName = "Pesho",
                    DateOfBirth = new DateTime(1999, 10, 14),
                },
            };

            var addMultipleAuthorsRequest = new AddMultipleAuthorsRequest()
            {
                AuthorRequests = authorsToAddRequest,
                Reason = "None"
            };



            _authorRepositoryMock
                .Setup(x => x.AddMultipleAuthors(It.IsAny<IEnumerable<Author>>()))
                .Callback(() =>
                {
                    foreach (var author in authorsToAdd)
                    {
                        _authors.Add(author);
                    }
                })
                .ReturnsAsync(() => true);


            //inject
            var controller = GetAuthorController();

            //act
            var result = await controller.AddAuthorRange(addMultipleAuthorsRequest);
            //Assert
            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var authorsResult = okObjectResult!.Value as AuthorsCollectionResponse;

            Assert.NotNull(authorsResult);
            Assert.Equal(5, _authors.Count);

            var authors = authorsResult!.Authors;

            Assert.NotNull(authors);

            Assert.Equal(authorsToAdd, authors);

        }

        [Fact]

        public async Task Author_AddRange_BadRequest()
        {
            var authorsToAddRequest = new List<AddAuthorRequest>()
            {
                new AddAuthorRequest()
                {
                    Age = 14,
                    Name = "Vanka",
                    NickName = "Pesho",
                    DateOfBirth = new DateTime(1999, 10, 14),
                },
                 new AddAuthorRequest()
                {
                    Age = 15,
                    Name = "Vanyo",
                    NickName = "Pesho",
                    DateOfBirth = new DateTime(1999, 10, 14),
                },
            };

            var authorsToAdd = new List<Author>()
            {
                new Author()
                {

                    Age = 14,
                    Name = "Vanka",
                    NickName = "Pesho",
                    DateOfBirth = new DateTime(1999, 10, 14),
                },
                 new Author()
                {
                    Age = 15,
                    Name = "Vanyo",
                    NickName = "Pesho",
                    DateOfBirth = new DateTime(1999, 10, 14),
                },
            };

            var addMultipleAuthorsRequest = new AddMultipleAuthorsRequest()
            {
                AuthorRequests = authorsToAddRequest,
                Reason = "None"
            };

            var expectedErrorMessage = "Failed to add authors";

            _authorRepositoryMock
                .Setup(x => x.AddMultipleAuthors(It.IsAny<IEnumerable<Author>>()))
                                .ReturnsAsync(false);

            //inject
            var controller = GetAuthorController();

            //act
            var result = await controller.AddAuthorRange(addMultipleAuthorsRequest);
            //Assert
            var badRequestObjectResult = result as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);

            var authorsResult = badRequestObjectResult!.Value as ErrorResponse;

            Assert.NotNull(authorsResult);
            Assert.Equal(3, _authors.Count);

            var errorMessage = authorsResult!.Error;

            Assert.Equal(expectedErrorMessage, errorMessage);

        }

        private AuthorController GetAuthorController()
        {
            var service = new AuthorService(_authorRepositoryMock.Object, _mapper, _bookRepositoryMock.Object, _loggerMock.Object);
            var controller = new AuthorController(_loggerAuthorControllerMock.Object, service);
            return controller;
        }
    }
}