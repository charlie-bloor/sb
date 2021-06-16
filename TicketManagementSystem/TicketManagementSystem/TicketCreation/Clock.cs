using System;

namespace TicketManagementSystem.TicketCreation
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
    
    public class Clock : IClock
    {
        public DateTime UtcNow => DateTime.Now;
    }
}