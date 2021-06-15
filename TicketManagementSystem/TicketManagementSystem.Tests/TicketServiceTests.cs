using System;
using FluentAssertions;
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
        
        [TestCase("foo")]
        [TestCase(null)]
        public void CreateTicket_UserIsNotFound_ThrowsUserNotFoundException(string testInputAssignedUserName)
        {
            // Arrange
            var testInputTitle = "Valid title";
            var testInputDescription = "Valid description";

            GetMock<IUserRepository>()
                .Setup(x => x.GetUser(testInputAssignedUserName))
                .Returns(() => null);
            
            // Act
            Action act = () => Subject.CreateTicket(testInputTitle, default, testInputAssignedUserName, testInputDescription, default, default);

            // Assert
            act.Should().Throw<UserNotFoundException>();
        }
    }
}