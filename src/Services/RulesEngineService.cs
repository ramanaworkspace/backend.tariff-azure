using System.Collections.Concurrent;
using System.Text.Json;
using TariffCalculator.Api.Models;

namespace TariffCalculator.Api.Services;

public class RulesEngineService
{
    private readonly string _rulesFilePath;
    private readonly ILogger<RulesEngineService> _logger;
    private readonly ConcurrentDictionary<string, TariffRule> _rules = new();

    public RulesEngineService(IConfiguration config, IWebHostEnvironment env, ILogger<RulesEngineService> logger)
    {
        _logger = logger;
        var configured = config["Rules:FilePath"] ?? "Rules/tariff-rules.json";
        _rulesFilePath = Path.IsPathRooted(configured) ? configured : Path.Combine(env.ContentRootPath, configured);
        LoadRules();
    }

    public void LoadRules()
    {
        _rules.Clear();
        if (!File.Exists(_rulesFilePath))
        {
            _logger.LogWarning("Rules file not found at {Path}. Creating a default file.", _rulesFilePath);
            Directory.CreateDirectory(Path.GetDirectoryName(_rulesFilePath)!);
            File.WriteAllText(_rulesFilePath, "[]");
        }

        var json = File.ReadAllText(_rulesFilePath);
        var list = JsonSerializer.Deserialize<List<TariffRule>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new();

        foreach (var r in list)
        {
            var key = Key(r.Country, r.HtsCode);
            _rules[key] = r;
        }
        _logger.LogInformation("Loaded {Count} tariff rules", _rules.Count);
    }

    public void OverwriteRules(IEnumerable<TariffRule> rules)
    {
        var list = rules.ToList();
        var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        Directory.CreateDirectory(Path.GetDirectoryName(_rulesFilePath)!);
        File.WriteAllText(_rulesFilePath, json);
        LoadRules();
    }

    public TariffRule? FindRule(string country, string htsCode)
    {
        _rules.TryGetValue(Key(country, htsCode), out var rule);
        return rule;
    }

    private static string Key(string country, string hts) => $"{country.Trim().ToUpperInvariant()}|{hts.Trim()}";
}