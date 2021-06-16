using System;
using FluentAssertions;
using NUnit.Framework;
using TestUtilities;
using TicketManagementSystem.Exceptions;
using TicketManagementSystem.TicketCreation.Validators;

namespace TicketManagementSystem.Tests
{
    [TestFixture]
    public class TicketTitleValidatorTests : MockBase<TicketTitleValidator>
    {
        [TestCase(null)]
        [TestCase("")]
        public void ValidateOrThrowInvalidTicketException_TitleIsNullOrEmpty_ThrowsInvalidTicketException(string testInputTitle)
        {
            // Arrange
            
            // Act
            Action act = () => Subject.ValidateOrThrowInvalidTicketException(testInputTitle);

            // Assert
            act.Should().Throw<InvalidTicketException>().Where(e => e.Message == "Title or description were null");
        }        
        
        [TestCase("Valid name")]
        [TestCase("Other valid name...")]
        public void ValidateOrThrowInvalidTicketException_TitleIsValid_DoesNotThrow(string testInputTitle)
        {
            // Arrange
            
            // Act
            Action act = () => Subject.ValidateOrThrowInvalidTicketException(testInputTitle);

            // Assert
            act.Should().NotThrow();
        }
    }
}