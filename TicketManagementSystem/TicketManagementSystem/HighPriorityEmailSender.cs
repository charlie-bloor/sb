using EmailService;

namespace TicketManagementSystem
{
    public interface IHighPriorityEmailSender
    {
        void SendHighPriorityEmail(CreateTicketArgs createTicketArgs);
    }

    public class HighPriorityEmailSender : IHighPriorityEmailSender
    {
        private readonly IEmailService _emailService;

        public HighPriorityEmailSender(IEmailService emailService)
        {
            _emailService = emailService;
        }
        
        public void SendHighPriorityEmail(CreateTicketArgs createTicketArgs)
        {
            if (createTicketArgs.Priority == Priority.High)
            {
                _emailService.SendEmailToAdministrator(createTicketArgs.Title, createTicketArgs.AssignedToUsername);
            }
        }
    }
}