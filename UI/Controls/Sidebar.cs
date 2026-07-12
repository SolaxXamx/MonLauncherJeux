using MonLauncherJeux.UI.Themes;

namespace MonLauncherJeux.UI.Controls;

/// <summary>
/// Barre laterale de navigation moderne
/// </summary>
public sealed class Sidebar : Panel
{
    private readonly List<SidebarItem> _items = new();
    private int _selectedIndex = -1;
    private ThemePalette? _theme;
    private const int ItemHeight = 50;

    public event EventHandler<SidebarItemClickedEventArgs>? ItemClicked;

    public Sidebar()
    {
        BackColor = Color.FromArgb(20, 27, 36);
        ForeColor = Color.White;
        Width = 80;
        Dock = DockStyle.Left;
        AutoScroll = true;

        AddItem("H", "Accueil");
        AddItem("G", "Bibliotheque");
        AddItem("*", "Favoris");
        AddItem("S", "Statistiques");
        AddItem("C", "Parametres");
    }

    public void SetTheme(ThemePalette theme)
    {
        _theme = theme;
        BackColor = theme.Panel;
        Invalidate();
    }

    public void AddItem(string icon, string tooltip)
    {
        _items.Add(new SidebarItem { Icon = icon, Tooltip = tooltip, Index = _items.Count });
        Height = _items.Count * ItemHeight + 10;
        Invalidate();
    }

    public void SelectItem(int index)
    {
        if (index >= 0 && index < _items.Count)
        {
            _selectedIndex = index;
            Invalidate();
        }
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        int index = e.Y / ItemHeight;
        if (index >= 0 && index < _items.Count)
        {
            SelectItem(index);
            ItemClicked?.Invoke(this, new SidebarItemClickedEventArgs(index, _items[index]));
        }
        base.OnMouseClick(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(BackColor);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        for (int i = 0; i < _items.Count; i++)
        {
            DrawItem(e.Graphics, _items[i], i * ItemHeight, i == _selectedIndex);
        }

        base.OnPaint(e);
    }

    private void DrawItem(Graphics g, SidebarItem item, int y, bool isSelected)
    {
        var itemRect = new Rectangle(0, y, Width, ItemHeight);
        
        if (isSelected)
        {
            using var selectedBrush = new SolidBrush(_theme?.Accent ?? Color.FromArgb(62, 166, 255));
            g.FillRectangle(selectedBrush, itemRect);
        }

        var font = new Font("Segoe UI", 12, FontStyle.Bold);
        var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        using var textBrush = new SolidBrush(isSelected ? Color.White : Color.FromArgb(160, 167, 180));
        g.DrawString(item.Icon, font, textBrush, itemRect, format);
    }

    private sealed class SidebarItem
    {
        public string Icon { get; set; } = string.Empty;
        public string Tooltip { get; set; } = string.Empty;
        public int Index { get; set; }
    }
}

public sealed class SidebarItemClickedEventArgs : EventArgs
{
    public int Index { get; }
    public string Tooltip { get; }

    public SidebarItemClickedEventArgs(int index, dynamic item)
    {
        Index = index;
        Tooltip = item.Tooltip;
    }
}