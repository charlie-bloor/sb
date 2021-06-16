using System;

namespace TicketManagementSystem.Repositories
{
    public interface IInjectableTicketRepository
    {
        int CreateTicket(Ticket ticket);
        void UpdateTicket(Ticket ticket);
        Ticket GetTicket(int id);
        Ticket GetTicketOrThrowApplicationException(int id);
    }

    public class InjectableTicketRepository : IInjectableTicketRepository
    {
        // TODO: locking if required
        
        public int CreateTicket(Ticket ticket)
        {
            return TicketRepository.CreateTicket(ticket);
        }

        public void UpdateTicket(Ticket ticket)
        {
            TicketRepository.UpdateTicket(ticket);
        }

        public Ticket GetTicket(int id)
        {
            return TicketRepository.GetTicket(id);
        }
        
        public Ticket GetTicketOrThrowApplicationException(int id)
        {
            return GetTicket(id) ?? throw new ApplicationException("No ticket found for id " + id);
        }
    }
}