namespace MonLauncherJeux.Core.Constants;

/// <summary>
/// Constantes pour les chemins de fichiers
/// </summary>
public static class Paths
{
    public static readonly string AppFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
        "MonLauncherJeux"
    );

    public static readonly string IconFolder = Path.Combine(AppFolder, "icons");
    public static readonly string ProfilesFolder = Path.Combine(AppFolder, "profiles");
    public static readonly string BackgroundsFolder = Path.Combine(AppFolder, "backgrounds");
    
    public static readonly string GamesDataFile = Path.Combine(AppFolder, "games.json");
    public static readonly string SettingsFile = Path.Combine(AppFolder, "settings.json");
    public static readonly string ProfileFile = Path.Combine(AppFolder, "profile.json");

    public static void EnsureDirectories()
    {
        Directory.CreateDirectory(AppFolder);
        Directory.CreateDirectory(IconFolder);
        Directory.CreateDirectory(ProfilesFolder);
        Directory.CreateDirectory(BackgroundsFolder);
    }
}