using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using MonLauncherJeux.Core.Constants;
using MonLauncherJeux.Core.Models;

namespace MonLauncherJeux.Core.Services;

/// <summary>
/// Service de gestion des donnees JSON (jeux, profil, parametres)
/// </summary>
public sealed class DataService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Charge la liste des jeux depuis le fichier JSON
    /// </summary>
    public List<GameEntry> LoadGames()
    {
        try
        {
            if (!File.Exists(Paths.GamesDataFile))
                return new List<GameEntry>();

            var json = File.ReadAllText(Paths.GamesDataFile);
            return JsonSerializer.Deserialize<List<GameEntry>>(json, JsonOptions) ?? new List<GameEntry>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur lors du chargement des jeux : {ex.Message}");
            return new List<GameEntry>();
        }
    }

    /// <summary>
    /// Sauvegarde la liste des jeux dans le fichier JSON
    /// </summary>
    public void SaveGames(List<GameEntry> games)
    {
        try
        {
            Paths.EnsureDirectories();
            var json = JsonSerializer.Serialize(games, JsonOptions);
            File.WriteAllText(Paths.GamesDataFile, json);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur lors de la sauvegarde des jeux : {ex.Message}");
        }
    }

    /// <summary>
    /// Charge le profil utilisateur
    /// </summary>
    public UserProfile LoadProfile()
    {
        try
        {
            if (!File.Exists(Paths.ProfileFile))
                return new UserProfile();

            var json = File.ReadAllText(Paths.ProfileFile);
            return JsonSerializer.Deserialize<UserProfile>(json, JsonOptions) ?? new UserProfile();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur lors du chargement du profil : {ex.Message}");
            return new UserProfile();
        }
    }

    /// <summary>
    /// Sauvegarde le profil utilisateur
    /// </summary>
    public void SaveProfile(UserProfile profile)
    {
        try
        {
            Paths.EnsureDirectories();
            var json = JsonSerializer.Serialize(profile, JsonOptions);
            File.WriteAllText(Paths.ProfileFile, json);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur lors de la sauvegarde du profil : {ex.Message}");
        }
    }

    /// <summary>
    /// Charge les parametres de l'application
    /// </summary>
    public AppSettings LoadSettings()
    {
        try
        {
            if (!File.Exists(Paths.SettingsFile))
                return new AppSettings();

            var json = File.ReadAllText(Paths.SettingsFile);
            return JsonSerializer.Deserialize<AppSettings>(json, JsonOptions) ?? new AppSettings();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur lors du chargement des parametres : {ex.Message}");
            return new AppSettings();
        }
    }

    /// <summary>
    /// Sauvegarde les parametres de l'application
    /// </summary>
    public void SaveSettings(AppSettings settings)
    {
        try
        {
            Paths.EnsureDirectories();
            var json = JsonSerializer.Serialize(settings, JsonOptions);
            File.WriteAllText(Paths.SettingsFile, json);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur lors de la sauvegarde des parametres : {ex.Message}");
        }
    }
}