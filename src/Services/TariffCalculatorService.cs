using TariffCalculator.Api.Data;
using TariffCalculator.Api.Entities;
using TariffCalculator.Api.Models;

namespace TariffCalculator.Api.Services;

public class TariffCalculatorService
{
    private readonly RulesEngineService _rules;
    private readonly AppDbContext _db;

    public TariffCalculatorService(RulesEngineService rules, AppDbContext db)
    {
        _rules = rules;
        _db = db;
    }

    public TariffResultDto Calculate(TariffInputDto input)
    {
        var rule = _rules.FindRule(input.CountryOfOrigin, input.HtsCode);
        var ratePct = rule?.Rate ?? 0m;
        var tariffType = rule?.TariffType ?? "NO_TARIFF";

        decimal dutyPerUnit = Math.Round(input.ProductCostPerUnit * (ratePct / 100m), 2);
        decimal absorbedPerUnit = Math.Round(dutyPerUnit * input.AbsorptionRate, 2);
        decimal passedPerUnit = Math.Round(dutyPerUnit - absorbedPerUnit, 2);

        decimal revenuePerUnit = input.SalePricePerUnit + passedPerUnit;
        decimal totalCostPerUnit = input.ProductCostPerUnit + absorbedPerUnit;
        decimal marginPerUnit = Math.Round(revenuePerUnit - totalCostPerUnit, 2);
        decimal marginPct = revenuePerUnit == 0 ? 0 : Math.Round((marginPerUnit / revenuePerUnit) * 100m, 2);

        var totalProductCost = input.ProductCostPerUnit * input.Quantity;
        var totalDuty = dutyPerUnit * input.Quantity;
        var totalCostWithDuty = totalProductCost + totalDuty;
        var baseRevenue = input.SalePricePerUnit * input.Quantity;
        var totalDutyPassed = passedPerUnit * input.Quantity;
        var totalRevenueWithDuty = baseRevenue + totalDutyPassed;
        var totalMargin = marginPerUnit * input.Quantity;

        // Save record
        _db.Calculations.Add(new CalculationRecord
        {
            CountryOfOrigin = input.CountryOfOrigin,
            HtsCode = input.HtsCode,
            Quantity = input.Quantity,
            ProductCostPerUnit = input.ProductCostPerUnit,
            SalePricePerUnit = input.SalePricePerUnit,
            AbsorptionRate = input.AbsorptionRate,
            TariffRate = ratePct,
            TariffType = tariffType,
            DutyAmountPerUnit = dutyPerUnit,
            MarginPercentPerUnit = marginPct
        });
        _db.SaveChanges();

        return new TariffResultDto
        {
            ProductCost = Math.Round(input.ProductCostPerUnit,2),
            DutyAmount = dutyPerUnit,
            TotalCostWithDuty = Math.Round(input.ProductCostPerUnit + dutyPerUnit,2),
            SalePrice = Math.Round(input.SalePricePerUnit,2),
            DutyCostPassedToCustomer = passedPerUnit,
            TotalRevenuePerUnit = Math.Round(revenuePerUnit,2),
            MarginAmount = marginPerUnit,
            MarginPercent = marginPct,

            Quantity = input.Quantity,
            TotalProductCost = Math.Round(totalProductCost,2),
            TotalDutyAmount = Math.Round(totalDuty,2),
            BaseRevenue = Math.Round(baseRevenue,2),
            TotalDutyPassedToCustomers = Math.Round(totalDutyPassed,2),
            TotalRevenueWithDuty = Math.Round(totalRevenueWithDuty,2),
            TotalMarginAmount = Math.Round(totalMargin,2),

            TariffType = tariffType,
            TariffRate = ratePct,
            ProductDescription = ""
        };
    }
}