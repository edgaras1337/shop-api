namespace api.CustomExceptions
{
    public class RelatedDataException : Exception
    {
        public RelatedDataException()
        {
        }

        public RelatedDataException(string message)
            : base(message)
        {
        }

        public RelatedDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
