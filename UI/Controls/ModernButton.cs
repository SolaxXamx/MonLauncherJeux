using MonLauncherJeux.UI.Themes;

namespace MonLauncherJeux.UI.Controls;

/// <summary>
/// Bouton moderne avec effets de survol et animations
/// </summary>
public sealed class ModernButton : Control
{
    private Color _normalColor;
    private Color _hoverColor;
    private Color _pressedColor;
    private bool _isHovered;
    private bool _isPressed;
    private StringFormat _stringFormat = StringFormat.GenericDefault;
    private int _borderRadius = 6;
    private int _shadowOffset = 2;

    public string ButtonText
    {
        get => Text;
        set { Text = value; Invalidate(); }
    }

    public Color NormalColor
    {
        get => _normalColor;
        set { _normalColor = value; Invalidate(); }
    }

    public Color HoverColor
    {
        get => _hoverColor;
        set { _hoverColor = value; Invalidate(); }
    }

    public Color PressedColor
    {
        get => _pressedColor;
        set { _pressedColor = value; Invalidate(); }
    }

    public int BorderRadius
    {
        get => _borderRadius;
        set { _borderRadius = value; Invalidate(); }
    }

    public ModernButton()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.Transparent;
        ForeColor = Color.White;
        Size = new Size(120, 40);
        Font = new Font("Segoe UI", 10, FontStyle.Bold);

        _normalColor = Color.FromArgb(62, 166, 255);
        _hoverColor = Color.FromArgb(100, 190, 255);
        _pressedColor = Color.FromArgb(40, 140, 230);

        _stringFormat.Alignment = StringAlignment.Center;
        _stringFormat.LineAlignment = StringAlignment.Center;
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
        _isPressed = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        _isPressed = true;
        Invalidate();
        base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        _isPressed = false;
        Invalidate();
        base.OnMouseUp(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(BackColor);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        var currentColor = _isPressed ? _pressedColor : (_isHovered ? _hoverColor : _normalColor);

        if (_isHovered || _isPressed)
        {
            using var shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0));
            using var shadowPath = CreateRoundedRectangle(new Rectangle(1, 1, Width - 2 - _shadowOffset, Height - 2 - _shadowOffset), _borderRadius);
            e.Graphics.FillPath(shadowBrush, shadowPath);
        }

        using var buttonBrush = new SolidBrush(currentColor);
        using var buttonPath = CreateRoundedRectangle(new Rectangle(0, 0, Width - 1, Height - 1), _borderRadius);
        e.Graphics.FillPath(buttonBrush, buttonPath);

        using var borderPen = new Pen(Color.FromArgb(100, currentColor), 1);
        e.Graphics.DrawPath(borderPen, buttonPath);

        using var textBrush = new SolidBrush(ForeColor);
        e.Graphics.DrawString(Text, Font, textBrush, ClientRectangle, _stringFormat);

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

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _stringFormat?.Dispose();
        }
        base.Dispose(disposing);
    }
}