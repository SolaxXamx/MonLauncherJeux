namespace MonLauncherJeux.Core.Models;

/// <summary>
/// Représente une entrée de jeu dans la bibliothèque
/// </summary>
public sealed class GameEntry
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string IconPath { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }
    public DateTime LastLaunch { get; set; } = DateTime.MinValue;
    public int LaunchCount { get; set; }
    public long PlaytimeSeconds { get; set; }
    public DateTime? LastPlaySession { get; set; }
}