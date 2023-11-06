using AutoFixture;
using Contacts.Application.Services;
using Contacts.Domain.DTO;
using Contacts.Domain.Entities;
using Contacts.Helpers;
using Contacts.Domain.Interfaces.Repositories;
using Moq;
using Shouldly;
using Xunit;
using Contacts.Domain.Interfaces.Application;
using System.Linq.Expressions;
using Contacts.Domain.DTO.Model;

namespace Contacts.Application.Tests.Services
{
    public class ContactServiceTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IContactTypeService> _contactTypeService;
        private readonly Mock<IPersonService> _personService;
        private readonly Mock<ILogService> _logService;

        public ContactServiceTests()
        {
            _fixture = new Fixture();
            _contactTypeService = new Mock<IContactTypeService>();
            _personService = new Mock<IPersonService>();
            _logService= new Mock<ILogService>();
        }

        [Fact(DisplayName = "GetById returns a Contact")]
        public void GetById_ReturnsContact()
        {
            // Arrange
            var contact = _fixture.Create<Contact>();
            var repositoryMock = new Mock<IRepositoryBase<Contact>>();
            repositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Contact, bool>>>()))
                         .Returns(new[] { contact }.AsQueryable());
            var contactService = new ContactService(repositoryMock.Object, _contactTypeService.Object, _personService.Object, _logService.Object);

            // Act
            var result = contactService.GetById(contact.Id);

            // Assert
            result.ShouldBe(contact);
        }

        [Fact(DisplayName = "GetById throws an exception when the ID doesn't exist")]
        public void GetById_ThrowsException()
        {
            // Arrange
            var id = 1;
            var repositoryMock = new Mock<IRepositoryBase<Contact>>();
            repositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Contact, bool>>>()));

            var contactService = new ContactService(repositoryMock.Object, _contactTypeService.Object, _personService.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() => contactService.GetById(id));
            exception.Message.ShouldBe("Contact not found!");
        }

        [Fact(DisplayName = "Delete a Contact successfully")]
        public void Delete_Success()
        {
            // Arrange
            var contact = _fixture.Create<Contact>();
            var repositoryMock = new Mock<IRepositoryBase<Contact>>();
            repositoryMock.Setup(r => r.Query(It.IsAny < Expression<Func<Contact, bool>>>()))
                         .Returns(new[] { contact }.AsQueryable());
            var contactService = new ContactService(repositoryMock.Object, _contactTypeService.Object, _personService.Object, _logService.Object);

            // Act 
            Should.NotThrow(() => contactService.Delete(contact.Id));

            // Assert
            repositoryMock.Verify(r => r.Delete(It.IsAny<Contact>()));
            repositoryMock.Verify(r => r.SaveChanges());
        }

        [Fact(DisplayName = "Delete throws an exception when the ID is not found")]
        public void Delete_ThrowsException()
        {
            // Arrange
            var id = 1; // ID que não existe
            var repositoryMock = new Mock<IRepositoryBase<Contact>>();
            repositoryMock.Setup(r => r.Query(It.IsAny < Expression<Func<Contact, bool>>>()))
                         .Returns(new List<Contact>().AsQueryable());
            var contactService = new ContactService(repositoryMock.Object, _contactTypeService.Object, _personService.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() => contactService.Delete(id));
            exception.Message.ShouldBe("Contact not found!");
        }

        [Theory(DisplayName = "List returns a list of Contact")]
        [InlineData(0, 10, 1, 2, "John", "Value", "asc")]
        [InlineData(10, 20, 2, 1, "Jane")]
        public void List_ReturnsList(int start, int limit, int personId, int contactTypeId, string value, string sort = "", string direction = "")
        {
            // Arrange
            var repositoryMock = new Mock<IRepositoryBase<Contact>>();
            var contactService = new ContactService(repositoryMock.Object, _contactTypeService.Object, _personService.Object, _logService.Object);

            // Act
            var result = contactService.List(start, limit, personId, contactTypeId, value, sort, direction);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<PaginatedList<Contact>>();
        }

        [Fact(DisplayName = "Save a Contact successfully")]
        public void Save_SavesContactSuccessfully()
        {
            // Arrange
            var contactModel = _fixture.Create<ContactModel>();
            var repositoryMock = new Mock<IRepositoryBase<Contact>>();
            var contactService = new ContactService(repositoryMock.Object, _contactTypeService.Object, _personService.Object, _logService.Object);

            _contactTypeService.Setup(c => c.GetById(contactModel.ContactTypeId, "")).Returns(_fixture.Create<ContactType>());
            _personService.Setup(p => p.GetById(contactModel.PersonId,"")).Returns(_fixture.Create<Person>());

            // Act
            Action saveAction = () => contactService.Save(contactModel);

            // Assert
            saveAction.ShouldNotThrow();
            repositoryMock.Verify(r => r.Save(It.IsAny<Contact>()));
            repositoryMock.Verify(r => r.SaveChanges());
        }

        [Fact(DisplayName = "Save throws an exception when the object is null")]
        public void Save_ThrowsExceptionIfObjectIsNull()
        {
            // Arrange
            ContactModel obj = null; // Objeto nulo
            var repositoryMock = new Mock<IRepositoryBase<Contact>>();
            var contactService = new ContactService(repositoryMock.Object, _contactTypeService.Object, _personService.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => contactService.Save(obj));
            exception.Message.ShouldBe("The object cannot be null!");
        }

        [Fact(DisplayName = "Update a Contact successfully")]
        public void Update_UpdatesContactSuccessfully()
        {
            // Arrange
            var id = 1;
            var contactModel = _fixture.Create<ContactModel>();
            var repositoryMock = new Mock<IRepositoryBase<Contact>>();
            var contactService = new ContactService(repositoryMock.Object, _contactTypeService.Object, _personService.Object, _logService.Object);

            _contactTypeService.Setup(c => c.GetById(contactModel.ContactTypeId,"")).Returns(_fixture.Create<ContactType>());
            _personService.Setup(p => p.GetById(contactModel.PersonId, "")).Returns(_fixture.Create<Person>());

            var existingContact = _fixture.Create<Contact>();
            repositoryMock.Setup(r => r.Query(It.IsAny<Expression<Func<Contact, bool>>>()))
                         .Returns(new[] { existingContact }.AsQueryable());

            // Act
            Action updateAction = () => contactService.Update(id, contactModel);

            // Assert
            updateAction.ShouldNotThrow(); 
            repositoryMock.Verify(r => r.Update(existingContact));
            repositoryMock.Verify(r => r.SaveChanges());
        }

        [Fact(DisplayName = "Update throws an exception when the object is null")]
        public void Update_ThrowsExceptionIfObjectIsNull()
        {
            // Arrange
            var repositoryMock = new Mock<IRepositoryBase<Contact>>();
            var contactService = new ContactService(repositoryMock.Object, _contactTypeService.Object, _personService.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => contactService.Update(1, null));
            exception.Message.ShouldBe("The object cannot be null!");
        }

        [Fact(DisplayName = "Update throws an exception when id is 0")]
        public void Update_ThrowsExceptionIfIdIsZero()
        {
            // Arrange
            var contactModel = _fixture.Create<ContactModel>();
            var repositoryMock = new Mock<IRepositoryBase<Contact>>();
            var contactService = new ContactService(repositoryMock.Object, _contactTypeService.Object, _personService.Object, _logService.Object);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => contactService.Update(0, contactModel));
            exception.Message.ShouldBe("Provide the record's ID!");
        }

    }
}