using TicketManagementSystem.Exceptions;

namespace TicketManagementSystem.TicketCreation.Validators
{
    public interface ITicketDescriptionValidator
    {
        void ValidateOrThrowInvalidTicketException(string description);
    }

    public class TicketDescriptionValidator : ITicketDescriptionValidator
    {
        public void ValidateOrThrowInvalidTicketException(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                // TODO: make message more specific
                throw new InvalidTicketException("Title or description were null");
            }
        }
    }
}