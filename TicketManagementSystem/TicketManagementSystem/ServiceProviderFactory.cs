using EmailService;
using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Validators;

namespace TicketManagementSystem
{
    public class ServiceProviderFactory
    {
        public ServiceProvider Create()
        {
            var services = new ServiceCollection();
            
            // Main services
            services.AddTransient<TicketAssigner, TicketAssigner>();
            services.AddTransient<TicketCreator, TicketCreator>();

            // Repositories
            services.AddSingleton<IUserRepository, UserRepository>();            
            services.AddSingleton<IInjectableTicketRepository, InjectableTicketRepository>();

            // Other services
            services.AddTransient<IAccountManagerGetter, AccountManagerGetter>();    
            services.AddTransient<IEmailService, EmailServiceProxy>();
            services.AddTransient<IHighPriorityEmailSender, HighPriorityEmailSender>();
            services.AddTransient<ITicketDescriptionValidator, TicketDescriptionValidator>();            
            services.AddTransient<ITicketPriceCalculator, TicketPriceCalculator>();            
            services.AddTransient<ITicketPriorityCalculator, TicketPriorityCalculator>();            
            services.AddTransient<ITicketTitleValidator, TicketTitleValidator>();            
            return services.BuildServiceProvider();
        }
    }
}