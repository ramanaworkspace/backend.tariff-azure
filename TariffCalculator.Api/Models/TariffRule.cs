namespace TariffCalculator.Api.Models;

public class TariffRule
{
    public string Country { get; set; } = "";
    public string HtsCode { get; set; } = "";
    public string TariffType { get; set; } = "STANDARD_TARIFF";
    public decimal Rate { get; set; } // percent, e.g. 26.0
    public string? Notes { get; set; }
}