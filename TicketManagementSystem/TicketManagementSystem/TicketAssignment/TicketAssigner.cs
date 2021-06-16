using TicketManagementSystem.Repositories;

namespace TicketManagementSystem.TicketAssignment
{
    public interface ITicketAssigner
    {
        void AssignTicket(int ticketId, string username);
    }

    public class TicketAssigner : ITicketAssigner
    {
        private readonly IInjectableTicketRepository _injectableTicketRepository;
        private readonly IUserRepository _userRepository;

        public TicketAssigner(IInjectableTicketRepository injectableTicketRepository,
                              IUserRepository userRepository)
        {
            _injectableTicketRepository = injectableTicketRepository;
            _userRepository = userRepository;
        }
        
        public void AssignTicket(int ticketId, string username)
        {
            var ticket = _injectableTicketRepository.GetTicketOrThrowApplicationException(ticketId);
            ticket.AssignedUser = _userRepository.GetUserOrThrowNotFoundException(username);
            _injectableTicketRepository.UpdateTicket(ticket);
        }
    }
}