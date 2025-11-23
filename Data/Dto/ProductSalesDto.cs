namespace Sharp_231.Data.Models
{
    internal class ProductSalesModel
    {
        public string ProductName { get; set; } = null!;
        public int SalesCount { get; set; }

        public override string ToString() => $"{ProductName} -- {SalesCount}";
    }
}