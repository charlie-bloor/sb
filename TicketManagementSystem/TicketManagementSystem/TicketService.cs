using System;
using EmailService;

// TODO:
// TicketService becomes a wrapper that builds its own host
// Create TicketCreator
// Create validator classes and "inject"
// Create PriceCalculator
// Create TicketAssigner
// The TicketRepository must remain static, so create a wrapper class
// Optionally, create an GetAccountManagerGetter: is this going too far?

namespace TicketManagementSystem
{
    public class TicketService
    {
        private readonly IEmailService _emailService;
        private readonly IPriorityCalculator _priorityCalculator;
        private readonly IUserRepository _userRepository;
        
        public TicketService(IEmailService emailService = null,
                             IPriorityCalculator priorityCalculator = null,
                             IUserRepository userRepository = null)
        {
            // The following is "poor person's DI" and should be replaced with proper IoC.
            // But proper IoC would require changing Program.cs.
            _emailService = emailService ?? new EmailServiceProxy();
            _priorityCalculator = priorityCalculator ?? new PriorityCalculator();
            _userRepository = userRepository ?? new UserRepository();
        }
        
        public int CreateTicket(string title,
                                Priority priority,
                                string assignedUsername,
                                string description,
                                DateTime dateAndTime,
                                bool isPayingCustomer)
        {
            ValidateTitleOrThrowInvalidTicketException(description);
            ValidateDescriptionOrThrowInvalidTicketException(description);
            priority = _priorityCalculator.Calculate(priority, title, dateAndTime);

            if (priority == Priority.High)
            {
                _emailService.SendEmailToAdministrator(title, assignedUsername);
            }
            
            var ticket = new Ticket
            {
                Title = title,
                AssignedUser = GetUser(assignedUsername),
                Priority = priority,
                Description = description,
                Created = dateAndTime,
                PriceDollars = GetPrice(priority, isPayingCustomer),
                AccountManager = GetAccountManager(isPayingCustomer)
            };

            var ticketId = TicketRepository.CreateTicket(ticket);
            return ticketId;
        }

        private User GetUser(string assignedUsername)
        {
            User user = null;
            if (assignedUsername != null)
            {
                user = _userRepository.GetUser(assignedUsername);
            }

            if (user == null)
            {
                throw new UserNotFoundException(assignedUsername);
            }

            return user;
        }

        public void AssignTicket(int ticketId, string username)
        {
            User user = null;
            if (username != null)
            {
                user = _userRepository.GetUser(username);
            }

            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            var ticket = TicketRepository.GetTicket(ticketId);

            if (ticket == null)
            {
                throw new ApplicationException("No ticket found for id " + ticketId);
            }

            ticket.AssignedUser = user;

            TicketRepository.UpdateTicket(ticket);
        }

        private void ValidateTitleOrThrowInvalidTicketException(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new InvalidTicketException("Title or description were null");
            }
        }
        private void ValidateDescriptionOrThrowInvalidTicketException(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new InvalidTicketException("Title or description were null");
            }
        }

        private double GetPrice(Priority priority, bool isPayingCustomer)
        {
            if (!isPayingCustomer)
            {
                return 0;
            }
            
            if (priority == Priority.High)
            {
                return 100;
            }

            return 50;
        }

        private User GetAccountManager(bool isPayingCustomer)
        {
            if (isPayingCustomer)
            {
                // Only paid customers have an account manager.
                return _userRepository.GetAccountManager();
            }

            return null;
        }        
    }

    public enum Priority
    {
        High,
        Medium,
        Low
    }
}
