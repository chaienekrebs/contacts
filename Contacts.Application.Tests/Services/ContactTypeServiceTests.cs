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
    public class ContactTypeServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ILogService> _logService;

        public ContactTypeServiceTests()
        {
            _fixture = new Fixture();
            _logService = new Mock<ILogService>();
        }

        [Fact(DisplayName = "GetById returns a ContactType")]
        public void GetById_ReturnsContactType()
        {
            // Arrange
            var contactType = _fixture.Create<ContactType>();
            var repositoryMock = new Mock<IRepositoryBase<ContactType>>();
            repositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<ContactType, bool>>>()))
                         .Returns(new[] { contactType }.AsQueryable());
            var contactTypeService = new ContactTypeService(repositoryMock.Object, _logService.Object);

            // Act
            var result = contactTypeService.GetById(contactType.Id);

            // Assert
            result.ShouldBe(contactType);
        }

        [Fact(DisplayName = "GetById throws an exception when the ID doesn't exist")]
        public void GetById_ThrowsException()
        {
            // Arrange
            var id = 1;
            var repositoryMock = new Mock<IRepositoryBase<ContactType>>();
            repositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<ContactType, bool>>>()));

            var contactTypeService = new ContactTypeService(repositoryMock.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() => contactTypeService.GetById(id));
            exception.Message.ShouldBe("Type not found!");
        }


        [Fact(DisplayName = "Delete a ContactType successfully")]
        public void Delete_Success()
        {
            // Arrange
            var type = _fixture.Create<ContactType>();
            var repositoryMock = new Mock<IRepositoryBase<ContactType>>();
            repositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<ContactType, bool>>>()))
                         .Returns(new[] { type }.AsQueryable());
            var contactTypeService = new ContactTypeService(repositoryMock.Object, _logService.Object);

            // Act 
            Action deleteAction = () => contactTypeService.Delete(type.Id);

            // Assert
            deleteAction.ShouldNotThrow();
            repositoryMock.Verify(r => r.Delete(It.IsAny<ContactType>()));
            repositoryMock.Verify(r => r.SaveChanges());
        }

        [Fact(DisplayName = "Delete throws an exception when the ID is not found")]
        public void Delete_ThrowsException()
        {
            // Arrange
            var id = 1; // ID que não existe
            var repositoryMock = new Mock<IRepositoryBase<ContactType>>();
            repositoryMock.Setup(r => r.Query(It.IsAny < Expression<Func<ContactType, bool>>>()))
                         .Returns(new List<ContactType>().AsQueryable());
            var contactTypeService = new ContactTypeService(repositoryMock.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<Contacts.Helpers.NotFoundException>(() => contactTypeService.Delete(id));
            exception.Message.ShouldBe("Type not found!");
        }


        [Theory(DisplayName = "List returns a list of ContactType")]
        [InlineData(0, 10, "John", "12345", "name", "desc")]
        [InlineData(10, 20, "Jane", "54321")]
        public void ListaPaginada_RetornaListaPaginada(int start, int length, string nome, string cpf, string sort = "", string direction = "")
        {
            // Arrange
            var repositoryMock = new Mock<IRepositoryBase<ContactType>>();
            var contactTypeService = new ContactTypeService(repositoryMock.Object, _logService.Object);

            // Act
            var result = contactTypeService.List(start, length, nome, sort, direction);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<PaginatedList<ContactType>>();
        }

        [Fact(DisplayName = "Save a ContactType successfully")]
        public void Save_SavesContactTypeSuccessfully()
        {
            // Arrange
            var contactTypeModel = _fixture.Create<ContactTypeModel>();
            var repositoryMock = new Mock<IRepositoryBase<ContactType>>();
            var contactTypeService = new ContactTypeService(repositoryMock.Object, _logService.Object);

            // Act
            Action saveAction = () => contactTypeService.Save(contactTypeModel);

            // Assert
            saveAction.ShouldNotThrow();
            repositoryMock.Verify(r => r.Save(It.IsAny<ContactType>()));
            repositoryMock.Verify(r => r.SaveChanges());
        }



        [Fact(DisplayName = "Save throws an exception when the object is null")]
        public void Save_ThrowsExceptionIfObjectIsNull()
        {
            // Arrange
            ContactTypeModel obj = null;
            var repositoryMock = new Mock<IRepositoryBase<ContactType>>();
            var contactTypeService = new ContactTypeService(repositoryMock.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => contactTypeService.Save(obj));
            exception.Message.ShouldBe("The object cannot be null!");
        }

        [Fact(DisplayName = "Update a ContactType successfully")]
        public void Update_UpdatesContactSuccessfully()
        {
            // Arrange
            var id = 1;
            var contactTypeModel = _fixture.Create<ContactTypeModel>();
            var repositoryMock = new Mock<IRepositoryBase<ContactType>>();
            var ContactTypeService = new ContactTypeService(repositoryMock.Object, _logService.Object);

            var existingContact = _fixture.Create<ContactType>();
            repositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<ContactType, bool>>>()))
                         .Returns(new[] { existingContact }.AsQueryable());

            // Act
            Action updateAction = () => ContactTypeService.Update(id, contactTypeModel);

            // Assert
            updateAction.ShouldNotThrow();
            repositoryMock.Verify(r => r.Update(existingContact));
            repositoryMock.Verify(r => r.SaveChanges());
        }

        [Fact(DisplayName = "Update throws an exception when the object is null")]
        public void Update_ThrowsExceptionIfObjectIsNull()
        {
            // Arrange
            var repositoryMock = new Mock<IRepositoryBase<ContactType>>();
            var contactTypeService = new ContactTypeService(repositoryMock.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => contactTypeService.Update(1, null));
            exception.Message.ShouldBe("The object cannot be null!");
        }

        [Fact(DisplayName = "Update throws an exception when id is 0")]
        public void Update_ThrowsExceptionIfIdIsZero()
        {
            // Arrange
            var contactTypeModel = _fixture.Create<ContactTypeModel>();
            var repositoryMock = new Mock<IRepositoryBase<ContactType>>();
            var contactTypeService = new ContactTypeService(repositoryMock.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => contactTypeService.Update(0, contactTypeModel));
            exception.Message.ShouldBe("Provide the record's ID!");
        }

    }
}