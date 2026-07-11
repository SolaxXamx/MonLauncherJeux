namespace MonLauncherJeux.UI.Themes;

/// <summary>
/// Gestionnaire des thèmes avec 7 thèmes prédéfinis modernes
/// </summary>
public sealed class ThemeManager
{
    private readonly Dictionary<string, ThemePalette> _themes = new();
    private ThemePalette _currentTheme;

    public event EventHandler<ThemeChangedEventArgs>? ThemeChanged;

    public ThemePalette CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (_currentTheme != value)
            {
                _currentTheme = value;
                ThemeChanged?.Invoke(this, new ThemeChangedEventArgs(value));
            }
        }
    }

    public IReadOnlyDictionary<string, ThemePalette> AvailableThemes => _themes;

    public ThemeManager()
    {
        InitializeThemes();
        _currentTheme = _themes["Sombre violet"];
    }

    private void InitializeThemes()
    {
        // 🌙 Sombre violet - Thème par défaut élégant
        _themes["Sombre violet"] = new ThemePalette(
            "Sombre violet",
            "#0B1017", "#141B24", "#1B2332", "#FFFFFF", "#A0A7B4", "#8B5CF6", "#2A3847", "#2A3F5F", "#22C55E", "#F59E0B", "#EF4444"
        );

        // 🎮 Steam - Inspiré de Steam
        _themes["Steam"] = new ThemePalette(
            "Steam",
            "#0B0E11", "#1B2838", "#2A475E", "#ECEEEF", "#8F98A0", "#1B9EFF", "#3F4F5F", "#2A3F5F", "#4CB343", "#FFA500", "#C23C2A"
        );

        // 🎯 Xbox - Inspiré de Xbox Series X
        _themes["Xbox"] = new ThemePalette(
            "Xbox",
            "#050505", "#0A0E27", "#1A1F3A", "#FFFFFF", "#909090", "#107C10", "#1F2937", "#2A3847", "#22C55E", "#FFB900", "#F44747"
        );

        // 🎪 PS5 - Inspiré de PlayStation 5
        _themes["PS5"] = new ThemePalette(
            "PS5",
            "#0F1419", "#191D26", "#262D38", "#F5F5F5", "#B0B5BA", "#0071E3", "#3A404A", "#2A3847", "#1DB954", "#FFA500", "#E74C3C"
        );

        // 💻 Cyber - Thème cyberpunk moderne
        _themes["Cyber"] = new ThemePalette(
            "Cyber",
            "#0A0E27", "#1A1F3A", "#242B48", "#00FF88", "#00D9FF", "#FF006E", "#3A404A", "#1F2937", "#22C55E", "#FFD700", "#FF4444"
        );

        // 🪟 Windows 11 - Inspiré de Windows 11
        _themes["Windows 11"] = new ThemePalette(
            "Windows 11",
            "#202124", "#2C2C2C", "#3A3A3A", "#FFFFFF", "#A0A0A0", "#0067C0", "#404040", "#4A4A4A", "#22C55E", "#FFB900", "#F44747"
        );

        // 📺 OLED - Optimisé pour écrans OLED
        _themes["OLED"] = new ThemePalette(
            "OLED",
            "#000000", "#0D0D0D", "#1A1A1A", "#FFFFFF", "#B0B0B0", "#00D9FF", "#2A2A2A", "#3A3A3A", "#22C55E", "#FFD700", "#FF4444"
        );
    }

    public ThemePalette? GetTheme(string name)
    {
        return _themes.TryGetValue(name, out var theme) ? theme : null;
    }

    public void SetTheme(string name)
    {
        if (_themes.TryGetValue(name, out var theme))
        {
            CurrentTheme = theme;
        }
    }
}

public sealed class ThemeChangedEventArgs : EventArgs
{
    public ThemePalette Theme { get; }

    public ThemeChangedEventArgs(ThemePalette theme)
    {
        Theme = theme;
    }
}