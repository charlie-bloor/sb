using System;
using System;
using EmailService;

namespace TicketManagementSystem
{
    public class TicketService
    {
        private readonly IUserRepository _userRepository;

        public TicketService(IUserRepository userRepository = null)
        {
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
            var user = GetUser(assignedUsername);

            var priorityRaised = false;
            if (dateAndTime < DateTime.UtcNow - TimeSpan.FromHours(1))
            {
                if (priority == Priority.Low)
                {
                    priority = Priority.Medium;
                    priorityRaised = true;
                }
                else if (priority == Priority.Medium)
                {
                    priority = Priority.High;
                    priorityRaised = true;
                }
            }

            if ((title.Contains("Crash") || title.Contains("Important") || title.Contains("Failure")) && !priorityRaised)
            {
                if (priority == Priority.Low)
                {
                    priority = Priority.Medium;
                }
                else if (priority == Priority.Medium)
                {
                    priority = Priority.High;
                }
            }

            if (priority == Priority.High)
            {
                var emailService = new EmailServiceProxy();
                emailService.SendEmailToAdministrator(title, assignedUsername);
            }

            double price = 0;
            User accountManager = null;
            if (isPayingCustomer)
            {
                // Only paid customers have an account manager.
                accountManager = new UserRepository().GetAccountManager();
                if (priority == Priority.High)
                {
                    price = 100;
                }
                else
                {
                    price = 50;
                }
            }

            var ticket = new Ticket()
            {
                Title = title,
                AssignedUser = user,
                Priority = priority,
                Description = description,
                Created = dateAndTime,
                PriceDollars = price,
                AccountManager = accountManager
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
    }

    public enum Priority
    {
        High,
        Medium,
        Low
    }
}
