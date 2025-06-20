namespace Store.Core.Exceptions
{
    public class ErrorDuringCreation : Exception
    {
        public ErrorDuringCreation()
        {
        }

        public ErrorDuringCreation(string? message) : base(message)
        {
        }
    }
}
