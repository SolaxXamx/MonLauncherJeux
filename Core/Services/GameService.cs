using System.Diagnostics;
using MonLauncherJeux.Core.Models;

namespace MonLauncherJeux.Core.Services;

/// <summary>
/// Service de gestion des jeux (ajout, suppression, lancement)
/// </summary>
public sealed class GameService : IDisposable
{
    private readonly DataService _dataService;
    private readonly List<GameEntry> _games = new();

    public event EventHandler<GamesChangedEventArgs>? GamesChanged;

    public GameService()
    {
        _dataService = new DataService();
        LoadGames();
    }

    /// <summary>
    /// Retourne la liste des jeux
    /// </summary>
    public IReadOnlyList<GameEntry> GetGames() => _games.AsReadOnly();

    /// <summary>
    /// Retourne les jeux favoris
    /// </summary>
    public IReadOnlyList<GameEntry> GetFavorites() => _games.Where(g => g.IsFavorite).ToList().AsReadOnly();

    /// <summary>
    /// Charge les jeux depuis le fichier JSON
    /// </summary>
    private void LoadGames()
    {
        _games.Clear();
        _games.AddRange(_dataService.LoadGames());
    }

    /// <summary>
    /// Ajoute un nouveau jeu à la bibliothèque
    /// </summary>
    public bool AddGame(string filePath, bool showErrors = true)
    {
        // Validation basique
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return false;

        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        if (extension is not ".exe" and not ".lnk")
        {
            if (showErrors)
                MessageBox.Show("Veuillez sélectionner un fichier .exe ou .lnk", "Format non supporté", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        // Résout la cible si c'est un raccourci
        var launchPath = filePath;
        var iconSource = ShortcutResolver.TryResolveTarget(filePath) ?? filePath;

        // Vérifie si le jeu existe déjà
        if (_games.Any(g => string.Equals(g.Path, launchPath, StringComparison.OrdinalIgnoreCase)))
            return false;

        // Crée l'entrée du jeu
        var gameName = Path.GetFileNameWithoutExtension(iconSource);
        var iconPath = IconImporter.ImportIcon(iconSource, gameName);

        var game = new GameEntry
        {
            Name = gameName,
            Path = launchPath,
            IconPath = iconPath
        };

        _games.Add(game);
        SaveGames();
        GamesChanged?.Invoke(this, new GamesChangedEventArgs(GameChangeType.Added, game));
        return true;
    }

    /// <summary>
    /// Supprime un jeu de la bibliothèque
    /// </summary>
    public void RemoveGame(GameEntry game)
    {
        if (_games.Remove(game))
        {
            SaveGames();
            GamesChanged?.Invoke(this, new GamesChangedEventArgs(GameChangeType.Removed, game));
        }
    }

    /// <summary>
    /// Bascule le statut favori d'un jeu
    /// </summary>
    public void ToggleFavorite(GameEntry game)
    {
        game.IsFavorite = !game.IsFavorite;
        SaveGames();
        GamesChanged?.Invoke(this, new GamesChangedEventArgs(GameChangeType.Modified, game));
    }

    /// <summary>
    /// Lance un jeu
    /// </summary>
    public void LaunchGame(GameEntry game)
    {
        if (!File.Exists(game.Path))
        {
            MessageBox.Show("Ce jeu ou raccourci n'existe plus.", "Introuvable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            game.LastLaunch = DateTime.Now;
            game.LaunchCount++;

            Process.Start(new ProcessStartInfo(game.Path)
            {
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(game.Path) ?? Environment.CurrentDirectory
            });

            SaveGames();
            GamesChanged?.Invoke(this, new GamesChangedEventArgs(GameChangeType.Launched, game));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors du lancement du jeu : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Sauvegarde les jeux dans le fichier JSON
    /// </summary>
    private void SaveGames()
    {
        _dataService.SaveGames(_games);
    }

    /// <summary>
    /// Importe plusieurs jeux à la fois
    /// </summary>
    public int AddGames(IEnumerable<string> filePaths, bool showErrors = true)
    {
        int added = 0;
        foreach (var filePath in filePaths)
        {
            if (AddGame(filePath, showErrors))
                added++;
        }
        return added;
    }

    public void Dispose()
    {
        _games.Clear();
    }
}

public enum GameChangeType
{
    Added,
    Removed,
    Modified,
    Launched
}

public sealed class GamesChangedEventArgs : EventArgs
{
    public GameChangeType ChangeType { get; }
    public GameEntry Game { get; }

    public GamesChangedEventArgs(GameChangeType changeType, GameEntry game)
    {
        ChangeType = changeType;
        Game = game;
    }
}