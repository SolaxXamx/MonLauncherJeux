using System.Drawing.Imaging;
using MonLauncherJeux.Core.Constants;

namespace MonLauncherJeux.Core.Services;

/// <summary>
/// Service d'extraction et de sauvegarde des icônes de jeux
/// </summary>
public static class IconImporter
{
    /// <summary>
    /// Importe l'icône d'un fichier exécutable ou raccourci
    /// </summary>
    public static string ImportIcon(string sourcePath, string gameName)
    {
        try
        {
            Directory.CreateDirectory(Paths.IconFolder);

            // Nettoie le nom du fichier
            var cleanName = string.Join("_", gameName.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
            
            // Génère un nom unique basé sur le hash du chemin source
            var hash = Math.Abs(sourcePath.ToLowerInvariant().GetHashCode());
            var output = Path.Combine(Paths.IconFolder, $"{cleanName}_{hash}.png");

            // Si l'icône existe déjà, la retourne
            if (File.Exists(output))
                return output;

            // Extrait l'icône associée au fichier
            using var icon = Icon.ExtractAssociatedIcon(sourcePath);
            if (icon is null)
                return string.Empty;

            // Convertit en bitmap et sauvegarde
            using var bitmap = icon.ToBitmap();
            bitmap.Save(output, ImageFormat.Png);
            
            return output;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur lors de l'importation de l'icône : {ex.Message}");
            return string.Empty;
        }
    }
}