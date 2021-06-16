using System;

namespace TicketManagementSystem
{
    public interface ITicketPriorityCalculator
    {
        void CalculatePriority(CreateTicketArgs createTicketArgs);
    }

    public class TicketPriorityCalculator : ITicketPriorityCalculator
    {
        private readonly IClock _clock;

        public TicketPriorityCalculator(IClock clock = null)
        {
            _clock = clock ?? new Clock();
        }
        
        public void CalculatePriority(CreateTicketArgs createTicketArgs)
        {
            var priorityRaised = false;
            
            if (IsTicketMoreThan1HourOld(createTicketArgs))
            {
                priorityRaised = RaisePriority(createTicketArgs);
            }

            if (!priorityRaised && TitleContainsKeyword(createTicketArgs))
            {
                RaisePriority(createTicketArgs);
            }
        }

        private bool IsTicketMoreThan1HourOld(CreateTicketArgs createTicketArgs)
        {
            return createTicketArgs.DateAndTime < _clock.UtcNow - TimeSpan.FromHours(1);
        }

        private bool RaisePriority(CreateTicketArgs createTicketArgs)
        {
            if (createTicketArgs.Priority == Priority.Low)
            {
                createTicketArgs.Priority = Priority.Medium;
                return true;
            }

            if (createTicketArgs.Priority == Priority.Medium)
            {
                createTicketArgs.Priority = Priority.High;
                return true;
            }

            return false;
        }

        private bool TitleContainsKeyword(CreateTicketArgs createTicketArgs)
        {
            if (createTicketArgs.Title.Contains("Crash"))
            {
                return true;
            }
            
            if (createTicketArgs.Title.Contains("Important"))
            {
                return true;
            }
            
            if (createTicketArgs.Title.Contains("Failure"))
            {
                return true;
            }

            return false;
        }
    }
}