namespace TariffCalculator.Api.Models;

public class TariffInputDto
{
    public string CountryOfOrigin { get; set; } = "";
    public string HtsCode { get; set; } = "";
    public int Quantity { get; set; } = 1;
    public decimal ProductCostPerUnit { get; set; }
    public decimal SalePricePerUnit { get; set; }
    public DateOnly PricingDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    /// <summary>0..1 fraction: percent of duty absorbed by company</summary>
    public decimal AbsorptionRate { get; set; } = 0.5m;
}