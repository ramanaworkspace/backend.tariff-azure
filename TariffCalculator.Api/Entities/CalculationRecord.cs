using System.ComponentModel.DataAnnotations;

namespace TariffCalculator.Api.Entities;

public class CalculationRecord
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public string CountryOfOrigin { get; set; } = "";
    public string HtsCode { get; set; } = "";
    public int Quantity { get; set; }
    public decimal ProductCostPerUnit { get; set; }
    public decimal SalePricePerUnit { get; set; }
    public decimal AbsorptionRate { get; set; }
    public decimal TariffRate { get; set; }
    public string TariffType { get; set; } = "";
    public decimal DutyAmountPerUnit { get; set; }
    public decimal MarginPercentPerUnit { get; set; }
}