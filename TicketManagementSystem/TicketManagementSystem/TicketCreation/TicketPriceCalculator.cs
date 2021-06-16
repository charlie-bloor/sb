namespace TicketManagementSystem.TicketCreation
{
    public interface ITicketPriceCalculator
    {
        double CalculatePrice(CreateTicketArgs createTicketArgs);
    }

    public class TicketPriceCalculator : ITicketPriceCalculator
    {
        // TODO: price should be decimal because double is approximate
        public double CalculatePrice(CreateTicketArgs createTicketArgs)
        {
            if (!createTicketArgs.IsPayingCustomer)
            {
                return 0;
            }
            
            if (createTicketArgs.Priority == Priority.High)
            {
                return 100;
            }

            return 50;
        }
    }
}