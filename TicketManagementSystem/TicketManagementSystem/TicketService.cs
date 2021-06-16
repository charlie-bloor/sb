using System;
using Microsoft.Extensions.DependencyInjection;

// TODO:
// The TicketRepository must remain static, so create a wrapper class
// Optionally, create an GetAccountManagerGetter: is this going too far?

namespace TicketManagementSystem
{
    public class TicketService
    {
        public int CreateTicket(string title,
                                Priority priority,
                                string assignedToUsername,
                                string description,
                                DateTime dateAndTime,
                                bool isPayingCustomer)
        {
            using var serviceProvider = new ServiceProviderFactory().Create(); 
            var ticketCreator = serviceProvider.GetRequiredService<TicketCreator>();
            
            var createTicketArgs = new CreateTicketArgs
            {
                Title = title,
                Description = description,
                Priority = priority,
                AssignedToUsername = assignedToUsername,
                DateAndTime = dateAndTime,
                IsPayingCustomer = isPayingCustomer
            };
            
            return ticketCreator.CreateTicket(createTicketArgs);
        }

        public void AssignTicket(int ticketId, string username)
        {
            using var serviceProvider = new ServiceProviderFactory().Create(); 
            var ticketAssigner = serviceProvider.GetRequiredService<TicketAssigner>();
            ticketAssigner.AssignTicket(ticketId, username);
        }
    }

    public enum Priority
    {
        High,
        Medium,
        Low
    }
}
