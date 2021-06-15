using System;

namespace TicketManagementSystem
{
    public interface IPriorityCalculator
    {
        Priority Calculate(Priority priority, string title, DateTime dateAndTime);
    }

    public class PriorityCalculator : IPriorityCalculator
    {
        private readonly IClock _clock;

        public PriorityCalculator(IClock clock = null)
        {
            _clock = clock ?? new Clock();
        }
        
        public Priority Calculate(Priority priority, string title, DateTime dateAndTime)
        {
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

            return priority;
        }
    }
}