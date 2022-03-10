namespace api.CustomExceptions
{
    public class UserRegistrationException : Exception
    {
        public UserRegistrationException()
        {
        }

        public UserRegistrationException(string message) 
            : base(message)
        {
        }

        public UserRegistrationException(string message, Exception inner) 
            : base(message, inner)
        {
        }
    }
}
