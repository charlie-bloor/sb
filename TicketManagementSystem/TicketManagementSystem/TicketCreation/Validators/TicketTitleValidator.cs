using TicketManagementSystem.Exceptions;

namespace TicketManagementSystem.TicketCreation.Validators
{
    public interface ITicketTitleValidator
    {
        void ValidateOrThrowInvalidTicketException(string title);
    }

    public class TicketTitleValidator : ITicketTitleValidator
    {
        public void ValidateOrThrowInvalidTicketException(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                // TODO: make message more specific
                throw new InvalidTicketException("Title or description were null");
            }
        }
    }
}