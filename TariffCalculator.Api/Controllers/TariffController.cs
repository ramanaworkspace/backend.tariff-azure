using Microsoft.AspNetCore.Mvc;
using TariffCalculator.Api.Models;
using TariffCalculator.Api.Services;

namespace TariffCalculator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TariffController : ControllerBase
{
    private readonly TariffCalculatorService _calc;
    private readonly PdfReportService _pdf;

    public TariffController(TariffCalculatorService calc, PdfReportService pdf)
    {
        _calc = calc;
        _pdf = pdf;
    }

    [HttpPost("calculate")]
    public ActionResult<TariffResultDto> Calculate([FromBody] TariffInputDto input)
    {
        var result = _calc.Calculate(input);
        return Ok(result);
    }

    [HttpPost("report")]
    public IActionResult Report([FromBody] TariffInputDto input)
    {
        var result = _calc.Calculate(input);
        var bytes = _pdf.Generate(input, result);
        return File(bytes, "application/pdf", "TariffReport.pdf");
    }
}