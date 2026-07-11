using MonLauncherJeux.Core.Models;
using MonLauncherJeux.UI.Themes;

namespace MonLauncherJeux.UI.Controls;

/// <summary>
/// Contrôle de carte de jeu avec image, titre et boutons d'action
/// </summary>
public sealed class GameCardControl : Control
{
    private readonly GameEntry _game;
    private readonly ThemePalette _theme;
    private readonly PictureBox _iconBox;
    private readonly Label _nameLabel;
    private readonly ModernButton _playButton;
    private readonly ModernButton _favoriteButton;
    private readonly Label _statsLabel;
    private bool _isHovered;
    private int _borderRadius = 12;

    public event EventHandler<GameCardEventArgs>? PlayClicked;
    public event EventHandler<GameCardEventArgs>? FavoriteClicked;

    public GameCardControl(GameEntry game, ThemePalette theme)
    {
        _game = game;
        _theme = theme;

        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = theme.Background;
        ForeColor = theme.Text;
        Size = new Size(220, 280);
        Margin = new Padding(10);

        _iconBox = new PictureBox { Width = 120, Height = 120, SizeMode = PictureBoxSizeMode.Zoom, BackColor = theme.Card };
        LoadGameIcon();

        _nameLabel = new Label { Text = game.Name, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = theme.Text, BackColor = theme.Background, AutoSize = false, Height = 40 };

        _playButton = new ModernButton { Text = "▶ Jouer", NormalColor = theme.Accent, HoverColor = theme.Success, Width = 100, Height = 35 };
        _playButton.Click += (_, _) => PlayClicked?.Invoke(this, new GameCardEventArgs(_game));

        _favoriteButton = new ModernButton { Text = _game.IsFavorite ? "★ Favori" : "☆ Épingler", NormalColor = theme.Panel, HoverColor = theme.Warning, Width = 100, Height = 35 };
        _favoriteButton.Click += (_, _) => FavoriteClicked?.Invoke(this, new GameCardEventArgs(_game));

        var lastLaunch = _game.LastLaunch != DateTime.MinValue ? _game.LastLaunch.ToString("dd/MM/yyyy") : "Jamais";
        _statsLabel = new Label { Text = $"Lancements: {_game.LaunchCount}\nDernier: {lastLaunch}", Font = new Font("Segoe UI", 9), ForeColor = theme.TextSecondary, BackColor = theme.Background, AutoSize = false, Height = 30 };

        Controls.Add(_iconBox);
        Controls.Add(_nameLabel);
        Controls.Add(_playButton);
        Controls.Add(_favoriteButton);
        Controls.Add(_statsLabel);

        PerformLayout();
    }

    private void LoadGameIcon()
    {
        try
        {
            if (File.Exists(_game.IconPath))
            {
                _iconBox.Image = Image.FromFile(_game.IconPath);
            }
            else
            {
                _iconBox.Paint += (_, e) =>
                {
                    TextRenderer.DrawText(e.Graphics, "🎮", new Font("Segoe UI Emoji", 40), _iconBox.ClientRectangle, _theme.Accent, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                };
            }
        }
        catch { }
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        _isHovered = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _isHovered = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(BackColor);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        var cardColor = _isHovered ? Color.FromArgb(35, 45, 70) : _theme.Card;
        using var cardBrush = new SolidBrush(cardColor);
        using var cardPath = CreateRoundedRectangle(new Rectangle(0, 0, Width - 1, Height - 1), _borderRadius);
        e.Graphics.FillPath(cardBrush, cardPath);

        var borderColor = _isHovered ? _theme.Accent : _theme.Border;
        using var borderPen = new Pen(borderColor, _isHovered ? 2f : 1f);
        e.Graphics.DrawPath(borderPen, cardPath);

        if (_isHovered)
        {
            using var shadowBrush = new SolidBrush(Color.FromArgb(60, 0, 0, 0));
            e.Graphics.FillPath(shadowBrush, cardPath);
        }

        base.OnPaint(e);
    }

    protected override void OnLayout(LayoutEventArgs e)
    {
        base.OnLayout(e);

        int padding = 10;
        _iconBox.Location = new Point(padding, padding);
        _iconBox.Size = new Size(Width - padding * 2, 100);

        _nameLabel.Location = new Point(padding, _iconBox.Bottom + 5);
        _nameLabel.Size = new Size(Width - padding * 2, 35);

        _playButton.Location = new Point(padding, _nameLabel.Bottom + 5);
        _playButton.Size = new Size((Width - padding * 3) / 2, 35);

        _favoriteButton.Location = new Point(_playButton.Right + 5, _nameLabel.Bottom + 5);
        _favoriteButton.Size = new Size((Width - padding * 3) / 2, 35);

        _statsLabel.Location = new Point(padding, _playButton.Bottom + 5);
        _statsLabel.Size = new Size(Width - padding * 2, Height - _statsLabel.Top - padding);
    }

    private System.Drawing.Drawing2D.GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
    {
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        var d = radius * 2;

        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.X + rect.Width - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.X + rect.Width - d, rect.Y + rect.Height - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Y + rect.Height - d, d, d, 90, 90);
        path.CloseFigure();

        return path;
    }
}

public sealed class GameCardEventArgs : EventArgs
{
    public GameEntry Game { get; }

    public GameCardEventArgs(GameEntry game)
    {
        Game = game;
    }
}