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
        [TestCase(" ")]
        public void CreateTicket_TitleIsEmpty_ThrowsInvalidTicketException(string testInputTitle)
        {
            // Arrange
            
            // Act
            Action act = () => Subject.CreateTicket(testInputTitle, default, default, default, default, default);

            // Assert
            act.Should().Throw<InvalidTicketException>();
        }        
        
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void CreateTicket_DescriptionIsEmpty_ThrowsInvalidTicketException(string testInputDescription)
        {
            // Arrange
            
            // Act
            Action act = () => Subject.CreateTicket(default, default, default, testInputDescription, default, default);

            // Assert
            act.Should().Throw<InvalidTicketException>();
        }
    }
}