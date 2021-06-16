using System;

namespace TicketManagementSystem.TicketCreation
{
    public class CreateTicketArgs
    {
        public string Title { get; set; }
        public Priority Priority { get; set; }
        public string AssignedToUsername { get; set; }
        public string Description { get; set; }
        public DateTime DateAndTime { get; set; }
        public bool IsPayingCustomer { get; set; }
    }
}