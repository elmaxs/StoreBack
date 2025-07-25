namespace Store.Contracts.UserContracts.Request.ProductUserDTO
{
    public class ProductFilterParams
    {
        public Guid CategoryId { get; set; }
        public string Order { get; set; } = "asc";
        public string Sort { get; set; } = "name";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
