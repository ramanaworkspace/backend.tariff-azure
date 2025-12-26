using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TariffCalculator.Api.Models;
using TariffCalculator.Api.Services;

namespace TariffCalculator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly RulesEngineService _rules;

    public AdminController(RulesEngineService rules)
    {
        _rules = rules;
    }

    [HttpPost("rules/upload")]
    [Consumes("multipart/form-data")]
    public IActionResult UploadRules([FromForm] UploadModel file)
    {
        if (file == null || file.File.Length == 0) return BadRequest("File is required");
        using var ms = new MemoryStream();
        file.File.CopyTo(ms);
        var json = System.Text.Encoding.UTF8.GetString(ms.ToArray());
        var rules = JsonSerializer.Deserialize<List<TariffRule>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (rules == null) return BadRequest("Invalid rules JSON");
        _rules.OverwriteRules(rules);
        return Ok(new { message = "Rules uploaded & reloaded", count = rules.Count });
    }

    [HttpPost("rules/reload")]
    [Consumes("multipart/form-data")]
    public IActionResult Reload()
    {
        _rules.LoadRules();
        return Ok(new { message = "Rules reloaded" });
    }
}