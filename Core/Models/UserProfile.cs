namespace MonLauncherJeux.Core.Models;

/// <summary>
/// Représente le profil utilisateur
/// </summary>
public sealed class UserProfile
{
    public string Username { get; set; } = "Joueur";
    public string ProfileImagePath { get; set; } = string.Empty;
    public int Level { get; set; } = 1;
    public int ExperiencePoints { get; set; }
    public string Status { get; set; } = "En ligne";
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public long TotalPlaytimeSeconds { get; set; }
    public int TotalGamesOwned { get; set; }

    public string GetFormattedLevel() => $"Niveau {Level}";
    
    public string GetFormattedPlaytime()
    {
        var hours = TotalPlaytimeSeconds / 3600;
        var minutes = (TotalPlaytimeSeconds % 3600) / 60;
        return $"{hours}h {minutes}m";
    }
}