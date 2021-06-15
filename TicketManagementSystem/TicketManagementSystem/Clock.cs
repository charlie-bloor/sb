using System;

namespace TicketManagementSystem
{
    public interface IClock
    {
        DateTime UtcNow();
    }
    
    public class  Clock : IClock
    {
        public DateTime UtcNow() => DateTime.Now;
    }
}