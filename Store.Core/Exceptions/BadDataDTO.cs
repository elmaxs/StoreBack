namespace Store.Core.Exceptions
{
    public class BadDataDTO : Exception
    {
        public BadDataDTO()
        {
        }

        public BadDataDTO(string? message) : base(message)
        {
        }
    }
}
