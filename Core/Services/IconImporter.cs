using System.Drawing.Imaging;
using System.Diagnostics;
using MonLauncherJeux.Core.Constants;

namespace MonLauncherJeux.Core.Services;

/// <summary>
/// Service d'extraction et de sauvegarde des icones de jeux
/// </summary>
public static class IconImporter
{
    /// <summary>
    /// Importe l'icone d'un fichier executable ou raccourci
    /// </summary>
    public static string ImportIcon(string sourcePath, string gameName)
    {
        try
        {
            Directory.CreateDirectory(Paths.IconFolder);

            // Nettoie le nom du fichier
            var cleanName = string.Join("_", gameName.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
            
            // Genere un nom unique base sur le hash du chemin source
            var hash = Math.Abs(sourcePath.ToLowerInvariant().GetHashCode());
            var output = Path.Combine(Paths.IconFolder, $"{cleanName}_{hash}.png");

            // Si l'icone existe deja, la retourne
            if (File.Exists(output))
                return output;

            // Extrait l'icone associee au fichier
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
            Debug.WriteLine($"Erreur lors de l'importation de l'icone : {ex.Message}");
            return string.Empty;
        }
    }
}