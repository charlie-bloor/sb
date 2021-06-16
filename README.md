# Refactoring test: explanation and thoughts

1. We'd ideally start from a regression suite of tests, ideally end to end. This would provide confidence that the new solution behaves exactly like the old one. Challenges around existing partially-implemented code, in particular the `UserRepository`, and time, meant this wasn't really feasible.
   
1. To make the code more manageable, much of the existing logic has been broken down into smaller components, and dependency injection is introduced.

   We can't change `Program.cs`. However, the "bootstrapping" process of initializing a service container has to start somewhere. The following class is therefore instantiated directly by the new version of `TicketService`:

    |Class|Purpose|
    |-|-|
    |`ServiceProviderFactory`|Creates a service provider that the new version of `TicketService` can use to resolve the new, high-level `TicketCreator` and `TicketAssigner` services|

    Although the following services are located explicitly, their own dependencies are injected automatically, and they represent the end of the bootstrapping process:

   |Class|Purpose|
   |-|-|
   |`TicketCreator`|Performs the logic of the `CreateTicket` method in the previous version of TicketService, but may still  have too many responsibilties|
   |`TicketAssigner`|Performs most of the logic of the `AssignTicket` method in the previous version of TicketService|

   The automatically-injected new services include:

   |Class|Purpose|
   |-|-|
   |`AccountManagerGetter`|Gets the account manager if the customer is paying|
   |`Clock`|Allows `DateTime.UtcNow` to be mocked in unit tests|
   |`CreateTicketArgs`|Avoids having to pass too many arguments when creating a ticket|
   |`HighPriorityEmailSender`|Encapsulates the small amount of logic for high-priority ticket  notifications|
   |`InjectableTicketRepository`|Wraps calls to `TicketRepository`, which must remain static|
   |`TicketPriceCalculator`|Encapsulates ticket price calculation|
   |`TicketPriorityCalculator`|Encapsulates ticket priority calculation|
   |`UserRepository`|An interface has been extracted allowing the class to be injected as a service|

1. To view a unit test or two, please see the `TicketServiceTests` fixture in the `TicketManagementSystem.Tests` project, which relies on a simple base class introduced in the `TestUtilities` project. It includes auto-mocking and use of the *FluentAssertions* package. There is one smaller test `TicketTitleValidatorTests`, and one larger one `TicketCreatorTests`. The larger test may be another indicator we need to further decompose `TicketCreator`!

1. We'd typically locate domain types such as `User` and `Ticket` in their own `.Domain` project but doing so would require a change to `Program.cs`. The repositories could be moved to their own `.Data` project. Validation could be achieved by introducing the *FluentValidation* package and data access would be via an object-relational model approach such as *Entity Framework*.