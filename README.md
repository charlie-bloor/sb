# Refactoring test

1. We would ideally start from a regression suite of tests. This would provide confidence that the new solution behaves exactly like the old. Challenges around existing code, in particular the `UserRepository`, and time, meant this wasn't really feasible.
   
   To see a unit test or 2 (including bonus mocking framework!) please see `TicketServiceTests` in the `TicketManagementSystem.Tests` project, which rely on a simple base class introduced in the `TestUtilities` project.

1. To make the code more manageable, much of the existing logic has been broken down into smaller components, and dependency injection is introduced.

   We can't change `Program.cs`. However, the "bootstrapping" process of initializing a service container has to start somewhere. The following class is therefore instantiated directly by the new version of `TicketService`:

    |Class|Description|
    |-|-|
    |`ServiceProviderFactory`|Creates a service provider that the new version of `TicketService` can use to resolve the new, high-level `TicketCreator` and `TicketAssigner` services|

    These services are located explicitly, but their own dependencies are injected automatically, and they represent the end of the bootstrapping process:

   |Class|Description|
   |-|-|
   |`TicketCreator`|Performs the logic of the `CreateTicket` method in the previous version of TicketService, and may still  have too many responsibilties|
   |`TicketAssigner`|Performs most of the logic of the `AssignTicket` method in the previous version of TicketService|

   The automatically-injected new services include:

   |Class|Description|
   |-|-|
   |`AccountManagerGetter`|Gets the account manager if the customer is paying|
   |`Clock`|Allows `DateTime.UtcNow` to be mocked in unit tests|
   |`CreateTicketArgs`|Avoids having to pass too many arguments when creating a ticket|
   |`HighPriorityEmailSender`|Encapsulates the small amount of logic high-priority ticket  notifications|
   |`TicketCreator`|Replaces most of the logic in the original `TicketService` using injected services|
   |`TicketPriceCalculator`|Encapsulates ticket price calculation|
   |`TicketPriorityCalculator`|Encapsulates ticket priority calculation|


1. 