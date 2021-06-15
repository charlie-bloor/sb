using System;
namespace TicketManagementSystem
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string username) : base("User " + username + " not found")
        {
        }
    }
}
