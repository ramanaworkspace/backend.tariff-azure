namespace TariffCalculator.Api.Models;

public class TariffResultDto
{
    // Per unit
    public decimal ProductCost { get; set; }
    public decimal DutyAmount { get; set; }
    public decimal TotalCostWithDuty { get; set; }
    public decimal SalePrice { get; set; }
    public decimal DutyCostPassedToCustomer { get; set; }
    public decimal TotalRevenuePerUnit { get; set; }
    public decimal MarginAmount { get; set; }
    public decimal MarginPercent { get; set; }

    // Totals
    public int Quantity { get; set; }
    public decimal TotalProductCost { get; set; }
    public decimal TotalDutyAmount { get; set; }
    public decimal BaseRevenue { get; set; }
    public decimal TotalDutyPassedToCustomers { get; set; }
    public decimal TotalRevenueWithDuty { get; set; }
    public decimal TotalMarginAmount { get; set; }

    // Meta
    public string TariffType { get; set; } = "";
    public decimal TariffRate { get; set; }
    public string ProductDescription { get; set; } = "";
}