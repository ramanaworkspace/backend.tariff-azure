using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TariffCalculator.Api.Models;

namespace TariffCalculator.Api.Services;
public class PdfReportService
{
    public byte[] Generate(TariffInputDto input, TariffResultDto result)
    {
        var now = DateTime.UtcNow;
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(12));
                page.Content().Column(col =>
                {
                    col.Spacing(10);
                    col.Item().Text("Tariff Calculator Report").FontSize(22).Bold();
                    col.Item().Text($"Generated: {now:yyyy-MM-dd HH:mm} UTC");

                    col.Item().Text("Calculator Input").Bold().FontSize(16);
                    col.Item().Table(t =>
                    {
                        t.ColumnsDefinition(c =>
                        {
                            c.ConstantColumn(220);
                            c.RelativeColumn();
                        });
                        void row(string k, string v)
                        {
                            t.Cell().Element(CellKey); t.Cell().Element(CellVal);
                            IContainer CellKey(IContainer c)
                            {
                                c = c.Border(0.5f).Padding(5).Background(Colors.Grey.Lighten3);
                                c.Text(k); // or c.Text(text => text.Span(k));
                                return c;
                            }
                            IContainer CellVal(IContainer c)
                            {
                                c = c.Border(0.5f).Padding(5);
                                c.Text(k); // or c.Text(text => text.Span(k));
                                return c;
                            }
                        }
                        row("Country of Origin", input.CountryOfOrigin);
                        row("HTS Code", input.HtsCode);
                        row("Quantity", input.Quantity.ToString());
                        row("Product Cost / Unit", input.ProductCostPerUnit.ToString("C"));
                        row("Sale Price / Unit", input.SalePricePerUnit.ToString("C"));
                        row("Pricing Date", input.PricingDate.ToString("yyyy-MM-dd"));
                        row("Absorption Rate", $"{input.AbsorptionRate:P0}");
                    });

                    col.Item().Text("Calculation Results").Bold().FontSize(16);
                    col.Item().Table(t =>
                    {
                        t.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn();
                            c.RelativeColumn();
                        });
                        t.Header(h =>
                        {
                            h.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Per Unit");
                            h.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Totals");
                        });
                        t.Cell().Padding(5).Text($"""
                                                Product Cost: {result.ProductCost:C}
                                                Duty Amount: {result.DutyAmount:C}
                                                Total Cost with Duty: {result.ProductCost + result.DutyAmount:C}
                                                Sale Price: {result.SalePrice:C}
                                                Duty Cost Passed to Customer: {result.DutyCostPassedToCustomer:C}
                                                Total Revenue per Unit: {result.TotalRevenuePerUnit:C}
                                                Margin ($): {result.MarginAmount:C}
                                                Margin (%): {result.MarginPercent:N2}%
                                                """);
                                                                        t.Cell().Padding(5).Text($"""
                                                Total Product Cost: {result.TotalProductCost:C}
                                                Total Duty Amount: {result.TotalDutyAmount:C}
                                                Total Cost with Duty: {result.TotalCostWithDuty:C}
                                                Base Revenue: {result.BaseRevenue:C}
                                                Total Duty Passed to Customers: {result.TotalDutyPassedToCustomers:C}
                                                Total Revenue with Duty: {result.TotalRevenueWithDuty:C}
                                                Total Margin ($): {result.TotalMarginAmount:C}
                                                """);
                    });

                    col.Item().Text("Tariff Breakdown").Bold().FontSize(16);
                    col.Item().Table(t =>
                    {
                        t.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn();
                            c.RelativeColumn();
                            c.RelativeColumn();
                        });
                        t.Header(h =>
                        {
                            h.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Tariff Type");
                            h.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Rate");
                            h.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Amount");
                        });

                        t.Cell().Padding(5).Text(result.TariffType);
                        t.Cell().Padding(5).Text($"{result.TariffRate:N2}%");
                        t.Cell().Padding(5).Text(result.DutyAmount.ToString("C"));
                    });

                    col.Item().Text("Insights").Bold().FontSize(16);
                    col.Item().Text($"""
                                    - Margin per unit: {result.MarginAmount:C} ({result.MarginPercent:N2}%)
                                    - Duty absorbed by company per unit: {(result.DutyAmount - result.DutyCostPassedToCustomer):C}
                                    - Duty passed to customer per unit: {result.DutyCostPassedToCustomer:C}
                                    """);
                });
            });
        });
        return doc.GeneratePdf();
    }
}