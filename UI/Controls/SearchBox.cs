using MonLauncherJeux.UI.Themes;

namespace MonLauncherJeux.UI.Controls;

/// <summary>
/// Boîte de recherche moderne avec placeholder
/// </summary>
public sealed class SearchBox : Control
{
    private string _placeholder = "Rechercher un jeu...";
    private string _searchText = string.Empty;
    private bool _isHovered;
    private bool _isFocused;
    private ThemePalette? _theme;
    private int _borderRadius = 6;

    public event EventHandler<SearchEventArgs>? SearchChanged;

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                SearchChanged?.Invoke(this, new SearchEventArgs(value));
                Invalidate();
            }
        }
    }

    public SearchBox()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
        BackColor = Color.Transparent;
        ForeColor = Color.White;
        Font = new Font("Segoe UI", 11);
        Height = 40;
        Width = 300;
    }

    public void SetTheme(ThemePalette theme)
    {
        _theme = theme;
        Invalidate();
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

    protected override void OnMouseClick(MouseEventArgs e)
    {
        Focus();
        _isFocused = true;
        Invalidate();
        base.OnMouseClick(e);
    }

    protected override void OnLostFocus(EventArgs e)
    {
        _isFocused = false;
        Invalidate();
        base.OnLostFocus(e);
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            return;
        }

        if (e.KeyChar == (char)Keys.Back && _searchText.Length > 0)
        {
            _searchText = _searchText.Substring(0, _searchText.Length - 1);
            SearchChanged?.Invoke(this, new SearchEventArgs(_searchText));
        }
        else if (!char.IsControl(e.KeyChar))
        {
            _searchText += e.KeyChar;
            SearchChanged?.Invoke(this, new SearchEventArgs(_searchText));
        }

        e.Handled = true;
        Invalidate();
        base.OnKeyPress(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(BackColor);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        var bgColor = _isFocused ? (_theme?.Card ?? Color.FromArgb(27, 35, 50)) : (_theme?.Panel ?? Color.FromArgb(20, 27, 36));
        var borderColor = _isFocused ? (_theme?.Accent ?? Color.FromArgb(62, 166, 255)) : (_theme?.Border ?? Color.FromArgb(42, 56, 71));
        var borderWidth = _isFocused ? 2f : 1f;

        using var bgBrush = new SolidBrush(bgColor);
        using var bgPath = CreateRoundedRectangle(new Rectangle(0, 0, Width - 1, Height - 1), _borderRadius);
        e.Graphics.FillPath(bgBrush, bgPath);

        using var borderPen = new Pen(borderColor, borderWidth);
        e.Graphics.DrawPath(borderPen, bgPath);

        var iconRect = new Rectangle(10, (Height - 20) / 2, 20, 20);
        using var iconBrush = new SolidBrush(Color.FromArgb(100, 110, 140));
        using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        e.Graphics.DrawString("🔍", new Font("Segoe UI Emoji", 12), iconBrush, iconRect, format);

        var textRect = new Rectangle(40, 0, Width - 50, Height);
        var textColor = string.IsNullOrEmpty(_searchText) ? Color.FromArgb(100, 110, 140) : Color.White;
        var displayText = string.IsNullOrEmpty(_searchText) ? _placeholder : _searchText;

        using var textBrush = new SolidBrush(textColor);
        using var textFormat = new StringFormat { LineAlignment = StringAlignment.Center };
        e.Graphics.DrawString(displayText, Font, textBrush, textRect, textFormat);

        base.OnPaint(e);
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

public sealed class SearchEventArgs : EventArgs
{
    public string SearchTerm { get; }

    public SearchEventArgs(string searchTerm)
    {
        SearchTerm = searchTerm;
    }
}