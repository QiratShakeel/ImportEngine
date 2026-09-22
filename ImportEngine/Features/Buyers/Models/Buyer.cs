namespace ImportEngine.Features.Buyers.Models
{
    public class Buyer
    {
        public int Id { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public string TargetTableName { get; set; } = string.Empty;
    }
}