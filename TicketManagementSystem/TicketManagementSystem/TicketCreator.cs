using TicketManagementSystem.Validators;

namespace TicketManagementSystem
{
    public interface ITicketCreator
    {
        int CreateTicket(CreateTicketArgs createTicketArgs);
    }

    public class TicketCreator : ITicketCreator
    {
        private readonly IAccountManagerGetter _accountManagerGetter;
        private readonly IHighPriorityEmailSender _highPriorityEmailSender;
        private readonly IInjectableTicketRepository _injectableTicketRepository;
        private readonly ITicketDescriptionValidator _ticketDescriptionValidator;
        private readonly ITicketPriceCalculator _ticketPriceCalculator;
        private readonly ITicketPriorityCalculator _ticketPriorityCalculator;
        private readonly ITicketTitleValidator _ticketTitleValidator;
        private readonly IUserRepository _userRepository;

        public TicketCreator(IAccountManagerGetter accountManagerGetter,
                             IHighPriorityEmailSender highPriorityEmailSender,
                             IInjectableTicketRepository injectableTicketRepository,
                             ITicketDescriptionValidator ticketDescriptionValidator,
                             ITicketPriceCalculator ticketPriceCalculator,
                             ITicketPriorityCalculator ticketPriorityCalculator,
                             ITicketTitleValidator ticketTitleValidator,
                             IUserRepository userRepository)
        {
            _accountManagerGetter = accountManagerGetter;
            _highPriorityEmailSender = highPriorityEmailSender;
            _injectableTicketRepository = injectableTicketRepository;
            _ticketDescriptionValidator = ticketDescriptionValidator;
            _ticketPriceCalculator = ticketPriceCalculator;
            _ticketPriorityCalculator = ticketPriorityCalculator;
            _ticketTitleValidator = ticketTitleValidator;
            _userRepository = userRepository;
        }
        
        public int CreateTicket(CreateTicketArgs createTicketArgs)
        {
            _ticketTitleValidator.ValidateOrThrowInvalidTicketException(createTicketArgs.Title);
            _ticketDescriptionValidator.ValidateOrThrowInvalidTicketException(createTicketArgs.Description);
            _ticketPriorityCalculator.CalculatePriority(createTicketArgs);
            _highPriorityEmailSender.SendHighPriorityEmail(createTicketArgs);
            
            var ticket = new Ticket
            {
                Title = createTicketArgs.Title,
                AssignedUser = _userRepository.GetUserOrThrowNotFoundException(createTicketArgs.AssignedToUsername),
                Priority = createTicketArgs.Priority,
                Description = createTicketArgs.Description,
                Created = createTicketArgs.DateAndTime,
                PriceDollars = _ticketPriceCalculator.CalculatePrice(createTicketArgs),
                AccountManager = _accountManagerGetter.GetAccountManager(createTicketArgs.IsPayingCustomer)
            };

            var ticketId = _injectableTicketRepository.CreateTicket(ticket);
            return ticketId;
        }
    }
}