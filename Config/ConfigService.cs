using System;
using System.IO;
using System.Text.Json;
using TAS_Test.ViewModels;

namespace TAS_Test.Config;

public class AppConfig
{
    public PdfSettings Pdf { get; set; } = new();
    public DatabaseSettings Database { get; set; } = new();
}

public class PdfSettings
{
    public string PrincePath { get; set; } = string.Empty;
    public string outputPath { get; set; } = string.Empty;
}

public class DatabaseSettings
{
    public string dbPath { get; set; } = string.Empty;
}
    
public static class ConfigService
{
    private static readonly string configPath = Path.Combine(AppContext.BaseDirectory, "/Users/niklas/RiderProjects/TAS_Test/Config/config.json");

    public static AppConfig LoadConfig()
    {
        if (!File.Exists(configPath))
            throw new FileNotFoundException($"Config-Datei nicht gefunden: {configPath}");

        string json = File.ReadAllText(configPath);
        var config = JsonSerializer.Deserialize<AppConfig>(json);

        if (config == null)
            throw new Exception("Fehler beim Einlesen der Config.");

        return config;
    }
    public static void SaveConfig(AppConfig config)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(config, options);
        File.WriteAllText(configPath, json);
    }
}
