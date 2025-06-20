namespace Store.Core.Exceptions
{
    public class NotFound : Exception
    {
        public NotFound()
        {
        }

        public NotFound(string? message) : base(message)
        {
        }
    }
}
