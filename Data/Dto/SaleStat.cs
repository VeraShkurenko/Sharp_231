using Sharp_231.Attributes;

namespace Sharp_231.Data.Dto
{
    [TableName("Sales")]
    public class SaleStat
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }        // для кількості штук або чеків
        public double TotalAmount { get; set; }  // для суми продажів
    }

}