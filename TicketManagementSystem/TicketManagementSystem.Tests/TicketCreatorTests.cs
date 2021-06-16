using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TestUtilities;
using TicketManagementSystem.Repositories;
using TicketManagementSystem.TicketCreation;
using TicketManagementSystem.TicketCreation.Validators;

namespace TicketManagementSystem.Tests
{
    [TestFixture]
    public class TicketCreatorTests : MockBase<TicketCreator>
    {
        [Test]
        public void CreateTicket_PathIsHappy_CallsExpectedServices()
        {
            // Arrange
            var testInputCreateTicketArgs = new CreateTicketArgs
            {
                Description = "Valid description",
                Priority = Priority.Medium,
                Title = "Title",
                AssignedToUsername = "APerson",
                DateAndTime = DateTime.Now,
                IsPayingCustomer = true
            };

            var testInputAssignedUser = new User { FirstName = "Test User" };

            GetMock<IUserRepository>()
                .Setup(x => x.GetUserOrThrowNotFoundException(testInputCreateTicketArgs.AssignedToUsername))
                .Returns(testInputAssignedUser);

            var testInputPriority = Priority.High;
            
            GetMock<ITicketPriorityCalculator>()
                .Setup(x => x.CalculatePriority(testInputCreateTicketArgs))
                .Callback(() => testInputCreateTicketArgs.Priority = testInputPriority);

            var testInputPrice = 10.00;

            GetMock<ITicketPriceCalculator>()
                .Setup(x => x.CalculatePrice(testInputCreateTicketArgs))
                .Returns(testInputPrice);

            var testInputAccountManagerUser = new User { FirstName = "Test Account Manager User" };
            
            GetMock<IAccountManagerGetter>()
                .Setup(x => x.GetAccountManager(testInputCreateTicketArgs.IsPayingCustomer))
                .Returns(testInputAccountManagerUser);

            var testInputTicketId = 123;
            Ticket testOutputCreatedTicket = null;
            
            GetMock<IInjectableTicketRepository>()
                .Setup(x => x.CreateTicket(It.IsAny<Ticket>()))
                .Callback<Ticket>(t => testOutputCreatedTicket = t)
                .Returns(testInputTicketId);

            // Act
            var result = Subject.CreateTicket(testInputCreateTicketArgs);
            
            // Assert
            result.Should().Be(testInputTicketId);

            testOutputCreatedTicket.Should().BeEquivalentTo(new Ticket
            {
                Title = testInputCreateTicketArgs.Title,
                AssignedUser = testInputAssignedUser,
                Priority = testInputPriority,
                Description = testInputCreateTicketArgs.Description,
                Created = testInputCreateTicketArgs.DateAndTime,
                PriceDollars = testInputPrice,
                AccountManager = testInputAccountManagerUser
            });

            GetMock<ITicketTitleValidator>()
                .Verify(x => x.ValidateOrThrowInvalidTicketException(testInputCreateTicketArgs.Title), Times.Once);
            
            GetMock<ITicketDescriptionValidator>()
                .Verify(x => x.ValidateOrThrowInvalidTicketException(testInputCreateTicketArgs.Description), Times.Once);

            GetMock<ITicketPriorityCalculator>()
                .Verify(x => x.CalculatePriority(testInputCreateTicketArgs), Times.Once);

            GetMock<IHighPriorityEmailSender>()
                .Verify(x => x.SendHighPriorityEmail(testInputCreateTicketArgs), Times.Once);

            GetMock<IUserRepository>()
                .Verify(x => x.GetUserOrThrowNotFoundException(testInputCreateTicketArgs.AssignedToUsername), Times.Once);

            GetMock<ITicketPriorityCalculator>()
                .Verify(x => x.CalculatePriority(testInputCreateTicketArgs), Times.Once);
                
            GetMock<ITicketPriceCalculator>()
                .Verify(x => x.CalculatePrice(testInputCreateTicketArgs), Times.Once);          

            GetMock<IAccountManagerGetter>()
                .Verify(x => x.GetAccountManager(testInputCreateTicketArgs.IsPayingCustomer), Times.Once);
                
            GetMock<IInjectableTicketRepository>()
                .Verify(x => x.CreateTicket(It.IsAny<Ticket>()), Times.Once);
        }
    }
}