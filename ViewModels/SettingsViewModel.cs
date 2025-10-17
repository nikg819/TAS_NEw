using System;
using System.Reactive;
using System.Threading;
using ReactiveUI;
using TAS_Test.Config;
using TAS_Test.Models;

namespace TAS_Test.ViewModels;

public class SettingsViewModel : ReactiveObject
{
    private string _subheader = "";
    public string Subheader
    {
        get => _subheader;
        set => this.RaiseAndSetIfChanged(ref _subheader, value);
    }
    public string SettingsPrince { get; set; }
    public string SettingsOutputPath { get; set; }
    public string SettingsDbPath { get; set; }
    public ReactiveCommand<Unit, Unit> ButtonSafeSettings { get; set; }

    public SettingsViewModel()
    {
        LoadConfig();
        ButtonSafeSettings = ReactiveCommand.Create(UpdateConfig);
    }

    private void UpdateConfig()
    {
        var config = ConfigService.LoadConfig();
        if (config.Pdf.PrincePath == SettingsPrince &&
            config.Pdf.outputPath == SettingsOutputPath &&
            config.Database.dbPath == SettingsDbPath)
        {
            Subheader = "Es wurde nichts geändert";
        }
        else
        {
            config.Pdf.PrincePath = SettingsPrince;
            config.Pdf.outputPath = SettingsOutputPath;
            config.Database.dbPath = SettingsDbPath;
            ConfigService.SaveConfig(config);
            Subheader = "✅ Einstellungen gespeichert";
        }
    }

    private void LoadConfig()
    {
        var config = ConfigService.LoadConfig();
        SettingsPrince = config.Pdf.PrincePath;
        SettingsOutputPath = config.Pdf.outputPath;
        SettingsDbPath = config.Database.dbPath;
    }
    
}