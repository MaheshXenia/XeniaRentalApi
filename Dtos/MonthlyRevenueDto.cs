namespace XeniaRentalApi.Dtos
{
    public class MonthlyRevenueDto
    {
        public string Month { get; set; } = string.Empty;
        public decimal TotalRent { get; set; }
    }
}
