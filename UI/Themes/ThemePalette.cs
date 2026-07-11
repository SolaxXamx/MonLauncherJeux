namespace MonLauncherJeux.UI.Themes;

/// <summary>
/// Représente une palette de couleurs pour un thème
/// </summary>
public sealed class ThemePalette
{
    public string Name { get; }
    public Color Background { get; }
    public Color Panel { get; }
    public Color Card { get; }
    public Color Text { get; }
    public Color TextSecondary { get; }
    public Color Accent { get; }
    public Color Border { get; }
    public Color Hover { get; }
    public Color Success { get; }
    public Color Warning { get; }
    public Color Error { get; }

    public ThemePalette(
        string name,
        string backgroundHex,
        string panelHex,
        string cardHex,
        string textHex,
        string textSecondaryHex,
        string accentHex,
        string borderHex,
        string hoverHex,
        string successHex,
        string warningHex,
        string errorHex
    )
    {
        Name = name;
        Background = ColorTranslator.FromHtml(backgroundHex);
        Panel = ColorTranslator.FromHtml(panelHex);
        Card = ColorTranslator.FromHtml(cardHex);
        Text = ColorTranslator.FromHtml(textHex);
        TextSecondary = ColorTranslator.FromHtml(textSecondaryHex);
        Accent = ColorTranslator.FromHtml(accentHex);
        Border = ColorTranslator.FromHtml(borderHex);
        Hover = ColorTranslator.FromHtml(hoverHex);
        Success = ColorTranslator.FromHtml(successHex);
        Warning = ColorTranslator.FromHtml(warningHex);
        Error = ColorTranslator.FromHtml(errorHex);
    }
}