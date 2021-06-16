namespace TicketManagementSystem
{
    public interface IAccountManagerGetter
    {
        User GetAccountManager(bool isPayingCustomer);
    }

    public class AccountManagerGetter : IAccountManagerGetter
    {
        private readonly IUserRepository _userRepository;

        public AccountManagerGetter(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public User GetAccountManager(bool isPayingCustomer)
        {
            if (isPayingCustomer)
            {
                return _userRepository.GetAccountManager();
            }

            return null;
        }    
    }
}