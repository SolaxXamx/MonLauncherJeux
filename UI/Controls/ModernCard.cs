using MonLauncherJeux.UI.Themes;

namespace MonLauncherJeux.UI.Controls;

/// <summary>
/// Carte moderne pour afficher les jeux avec effets de survol
/// </summary>
public sealed class ModernCard : Control
{
    private Color _cardColor;
    private Color _borderColor;
    private Color _hoverColor;
    private bool _isHovered;
    private int _borderRadius = 12;
    private int _shadowSize = 8;
    private float _scale = 1f;
    private const float MaxScale = 1.05f;

    public Color CardColor
    {
        get => _cardColor;
        set { _cardColor = value; Invalidate(); }
    }

    public Color BorderColor
    {
        get => _borderColor;
        set { _borderColor = value; Invalidate(); }
    }

    public Color HoverColor
    {
        get => _hoverColor;
        set { _hoverColor = value; Invalidate(); }
    }

    public int BorderRadius
    {
        get => _borderRadius;
        set { _borderRadius = value; Invalidate(); }
    }

    public ModernCard()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.Transparent;
        Size = new Size(200, 240);
        Padding = new Padding(14);

        _cardColor = Color.FromArgb(27, 35, 50);
        _borderColor = Color.FromArgb(42, 56, 71);
        _hoverColor = Color.FromArgb(62, 166, 255);
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

        DrawShadow(e.Graphics);
        DrawCardBackground(e.Graphics);
        DrawBorder(e.Graphics);

        base.OnPaint(e);
    }

    private void DrawShadow(Graphics g)
    {
        using var shadowPath = CreateRoundedRectangle(new Rectangle(0, 0, Width - 1, Height - 1), _borderRadius);
        var shadowColor = _isHovered ? Color.FromArgb(60, 0, 0, 0) : Color.FromArgb(30, 0, 0, 0);
        using var shadowBrush = new SolidBrush(shadowColor);
        g.FillPath(shadowBrush, shadowPath);
    }

    private void DrawCardBackground(Graphics g)
    {
        var bgColor = _isHovered ? Color.FromArgb(35, 45, 70) : _cardColor;
        using var bgBrush = new SolidBrush(bgColor);
        using var bgPath = CreateRoundedRectangle(new Rectangle(0, 0, Width - 1, Height - 1), _borderRadius);
        g.FillPath(bgBrush, bgPath);
    }

    private void DrawBorder(Graphics g)
    {
        var borderC = _isHovered ? _hoverColor : _borderColor;
        using var borderPen = new Pen(borderC, 1.5f);
        using var borderPath = CreateRoundedRectangle(new Rectangle(0, 0, Width - 1, Height - 1), _borderRadius);
        g.DrawPath(borderPen, borderPath);
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