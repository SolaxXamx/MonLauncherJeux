namespace MonLauncherJeux.Core.Models;

/// <summary>
/// Représente les paramètres de l'application
/// </summary>
public sealed class AppSettings
{
    public string CurrentTheme { get; set; } = "Sombre violet";
    public bool EnableAnimations { get; set; } = true;
    public int CardSize { get; set; } = 200; // pixels
    public string FontFamily { get; set; } = "Segoe UI";
    public float FontSize { get; set; } = 11f;
    public string Language { get; set; } = "fr-FR";
    public bool EnableAutoUpdate { get; set; } = true;
    public string BackgroundImagePath { get; set; } = string.Empty;
    public bool DarkMode { get; set; } = true;
    public int WindowWidth { get; set; } = 1400;
    public int WindowHeight { get; set; } = 900;
    public bool RememberWindowSize { get; set; } = true;
}