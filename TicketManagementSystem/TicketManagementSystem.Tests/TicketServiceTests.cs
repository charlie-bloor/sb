using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TestUtilities;

namespace TicketManagementSystem.Tests
{
    public class TicketServiceTests : MockBase<TicketService>
    {
        [TestCase(null)]
        [TestCase("")]
        public void CreateTicket_TitleIsNullOrEmpty_ThrowsInvalidTicketException(string testInputTitle)
        {
            // Arrange
            
            // Act
            Action act = () => Subject.CreateTicket(testInputTitle, default, default, default, default, default);

            // Assert
            act.Should().Throw<InvalidTicketException>().Where(e => e.Message == "Title or description were null");
        }        
        
        [TestCase(null)]
        [TestCase("")]
        public void CreateTicket_DescriptionIsNullOrEmpty_ThrowsInvalidTicketException(string testInputDescription)
        {
            // Arrange
            
            // Act
            Action act = () => Subject.CreateTicket(default, default, default, testInputDescription, default, default);

            // Assert
            act.Should().Throw<InvalidTicketException>().Where(e => e.Message == "Title or description were null");
        }
        
        [Test]
        public void CreateTicket_UserIsNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            string testInputAssignedUserName = "foo";
            var testInputTitle = "Valid title";
            var testInputDescription = "Valid description";

            GetMock<IUserRepository>()
                .Setup(x => x.GetUser(testInputAssignedUserName))
                .Returns(() => null);

            // Act
            Action act = () => Subject.CreateTicket(testInputTitle, default, testInputAssignedUserName, testInputDescription, default, default);

            // Assert
            act.Should().Throw<UserNotFoundException>();
            
            GetMock<IUserRepository>()
                .Verify(x => x.GetUser(testInputAssignedUserName), Times.Once);            
        }        
        
        [Test]
        public void CreateTicket_UserIsNull_DoesNotCallRepository()
        {
            // Arrange
            var testInputTitle = "Valid title";
            var testInputDescription = "Valid description";

            // Act
            Action act = () => Subject.CreateTicket(testInputTitle, default, null, testInputDescription, default, default);

            // Assert
            act.Should().Throw<UserNotFoundException>();
            
            GetMock<IUserRepository>()
                .Verify(x => x.GetUser(It.IsAny<string>()), Times.Never);            
        }
    }
}