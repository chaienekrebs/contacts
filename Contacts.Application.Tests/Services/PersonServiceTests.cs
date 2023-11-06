using AutoFixture;
using Contacts.Application.Services;
using Contacts.Domain.DTO;
using Contacts.Domain.Entities;
using Contacts.Helpers;
using Contacts.Domain.Interfaces.Repositories;
using Moq;
using Shouldly;
using Xunit;
using System.Linq.Expressions;
using Contacts.Domain.DTO.Model;
using Contacts.Domain.Interfaces.Application;

namespace Contacts.Application.Tests.Services
{
    public class PersonServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ILogService> _logService;
        public PersonServiceTests()
        {

            _fixture = new Fixture();
            _logService = new Mock<ILogService>();
        }

        [Fact(DisplayName = "GetById returns a Person")]
        public void GetById_ReturnsContact()
        {
            // Arrange
            var person = _fixture.Create<Person>();
            var repositoryMock = new Mock<IRepositoryBase<Person>>();
            repositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Person, bool>>>()))
                         .Returns(new[] { person }.AsQueryable());
            var personService = new PersonService(repositoryMock.Object, _logService.Object);

            // Act
            var result = personService.GetById(person.Id);

            // Assert
            result.ShouldBe(person);
        }

        [Fact(DisplayName = "GetById throws an exception when the ID doesn't exist")]
        public void GetById_ThrowsException()
        {
            // Arrange
            var id = 1;
            var repositoryMock = new Mock<IRepositoryBase<Person>>();
            repositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Person, bool>>>()));

            var personService = new PersonService(repositoryMock.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() => personService.GetById(id));
            exception.Message.ShouldBe("Person not found!");
        }


        [Fact(DisplayName = "Delete a Person successfully")]
        public void Delete_Success()
        {
            // Arrange
            var person = _fixture.Create<Person>();
            var repositoryMock = new Mock<IRepositoryBase<Person>>();
            repositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Person, bool>>>()))
                         .Returns(new[] { person }.AsQueryable());
            var personService = new PersonService(repositoryMock.Object, _logService.Object);

            // Act 
            Action deleteAction = () => personService.Delete(person.Id);

            // Assert
            deleteAction.ShouldNotThrow();
            repositoryMock.Verify(r => r.Delete(It.IsAny<Person>()));
            repositoryMock.Verify(r => r.SaveChanges());
        }


        [Fact(DisplayName = "Delete throws an exception when the ID is not found")]
        public void Delete_ThrowsException()
        {
            // Arrange
            // Arrange
            var id = 1; // ID que não existe
            var repositoryMock = new Mock<IRepositoryBase<Person>>();
            repositoryMock.Setup(r => r.Query(It.IsAny < Expression<Func<Person, bool>>>()))
                         .Returns(new List<Person>().AsQueryable());
            var personService = new PersonService(repositoryMock.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<Contacts.Helpers.NotFoundException>(() => personService.Delete(id));
            exception.Message.ShouldBe("Person not found!");
        }


        [Theory(DisplayName = "List returns a list of Person")]
        [InlineData(0, 10, "John", "12345", "name", "desc")]
        [InlineData(10, 20, "Jane", "54321")]
        public void ListaPaginada_RetornaListaPaginada(int start, int length, string nome, string cpf, string sort = "", string direction = "")
        {
            // Arrange
            var repositoryMock = new Mock<IRepositoryBase<Person>>();
            var personService = new PersonService(repositoryMock.Object, _logService.Object);

            // Act
            var result = personService.List(start, length, nome, cpf, sort, direction);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<PaginatedList<Person>>();
        }

        [Fact(DisplayName = "Save a Person successfully")]
        public void Save_SavesPersonSuccessfully()
        {
            // Arrange
            var personModel = _fixture.Create<PersonModel>();
            var repositoryMock = new Mock<IRepositoryBase<Person>>();
            var personService = new PersonService(repositoryMock.Object, _logService.Object);

            // Act
            Action saveAction = () => personService.Save(personModel);

            // Assert
            saveAction.ShouldNotThrow();
            repositoryMock.Verify(r => r.Save(It.IsAny<Person>()));
            repositoryMock.Verify(r => r.SaveChanges());
        }

        [Fact(DisplayName = "Save throws an exception when the object is null")]
        public void Save_ThrowsExceptionIfObjectIsNull()
        {
            // Arrange
            PersonModel obj = null;
            var repositoryMock = new Mock<IRepositoryBase<Person>>();
            var personService = new PersonService(repositoryMock.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => personService.Save(obj));
            exception.Message.ShouldBe("The object cannot be null!");
        }


        [Fact(DisplayName = "Update a Person successfully")]
        public void Update_UpdatesContactSuccessfully()
        {
            // Arrange
            var id = 1;
            var personModel = _fixture.Create<PersonModel>();
            var repositoryMock = new Mock<IRepositoryBase<Person>>();
            var personService = new PersonService(repositoryMock.Object, _logService.Object);

            var existingContact = _fixture.Create<Person>();
            repositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Person, bool>>>()))
                         .Returns(new[] { existingContact }.AsQueryable());

            // Act
            Action updateAction = () => personService.Update(id, personModel);

            // Assert
            updateAction.ShouldNotThrow();
            repositoryMock.Verify(r => r.Update(existingContact));
            repositoryMock.Verify(r => r.SaveChanges());
        }

        [Fact(DisplayName = "Update throws an exception when the object is null")]
        public void Update_ThrowsExceptionIfObjectIsNull()
        {
            // Arrange
            var repositoryMock = new Mock<IRepositoryBase<Person>>();
            var personService = new PersonService(repositoryMock.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => personService.Update(1, null));
            exception.Message.ShouldBe("The object cannot be null!");
        }

        [Fact(DisplayName = "Update throws an exception when id is 0")]
        public void Update_ThrowsExceptionIfIdIsZero()
        {
            // Arrange
            var personModel = _fixture.Create<PersonModel>();
            var repositoryMock = new Mock<IRepositoryBase<Person>>();
            var personService = new PersonService(repositoryMock.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => personService.Update(0, personModel));
            exception.Message.ShouldBe("Provide the record's ID!");
        }

    }
}