using MonLauncherJeux.UI.Themes;

namespace MonLauncherJeux.UI.Controls;

/// <summary>
/// Barre supérieure affichant l'heure, date, batterie et profil
/// </summary>
public sealed class TopBar : Control
{
    private readonly Label _dateTimeLabel;
    private readonly Label _batteryLabel;
    private readonly Label _networkLabel;
    private readonly Label _profileLabel;
    private readonly PictureBox _profileImage;
    private readonly System.Windows.Forms.Timer _updateTimer;
    private ThemePalette? _theme;

    public event EventHandler? ProfileClicked;
    public event EventHandler? SettingsClicked;

    public TopBar()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.FromArgb(20, 27, 36);
        ForeColor = Color.White;
        Height = 50;
        Dock = DockStyle.Top;
        Padding = new Padding(15);

        _dateTimeLabel = new Label { Text = DateTime.Now.ToString("HH:mm"), Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent, AutoSize = true };
        Controls.Add(_dateTimeLabel);

        _batteryLabel = new Label { Text = "🔋 100%", Font = new Font("Segoe UI", 10), ForeColor = Color.White, BackColor = Color.Transparent, AutoSize = true };
        Controls.Add(_batteryLabel);

        _networkLabel = new Label { Text = "📡 Connecté", Font = new Font("Segoe UI", 10), ForeColor = Color.White, BackColor = Color.Transparent, AutoSize = true };
        Controls.Add(_networkLabel);

        _profileLabel = new Label { Text = "Profil", Font = new Font("Segoe UI", 10), ForeColor = Color.White, BackColor = Color.Transparent, AutoSize = true, Cursor = Cursors.Hand };
        _profileLabel.Click += (_, _) => ProfileClicked?.Invoke(this, EventArgs.Empty);
        Controls.Add(_profileLabel);

        _profileImage = new PictureBox { Width = 32, Height = 32, SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.Transparent };
        Controls.Add(_profileImage);

        _updateTimer = new System.Windows.Forms.Timer { Interval = 1000 };
        _updateTimer.Tick += (_, _) => UpdateInfo();
        _updateTimer.Start();
    }

    public void SetTheme(ThemePalette theme)
    {
        _theme = theme;
        BackColor = theme.Panel;
        Invalidate();
    }

    private void UpdateInfo()
    {
        _dateTimeLabel.Text = DateTime.Now.ToString("HH:mm\ndd/MM");
        Invalidate();
    }

    protected override void OnLayout(LayoutEventArgs e)
    {
        base.OnLayout(e);

        int xPos = Padding.Left;
        
        _dateTimeLabel.Location = new Point(xPos, (Height - _dateTimeLabel.Height) / 2);
        xPos += _dateTimeLabel.Width + 30;

        _batteryLabel.Location = new Point(xPos, (Height - _batteryLabel.Height) / 2);
        xPos += _batteryLabel.Width + 20;

        _networkLabel.Location = new Point(xPos, (Height - _networkLabel.Height) / 2);
        xPos += _networkLabel.Width + 20;

        _profileImage.Location = new Point(Width - Padding.Right - _profileImage.Width - 50, (Height - _profileImage.Height) / 2);
        _profileLabel.Location = new Point(_profileImage.Left - _profileLabel.Width - 10, (Height - _profileLabel.Height) / 2);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(BackColor);
        base.OnPaint(e);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _updateTimer?.Dispose();
        }
        base.Dispose(disposing);
    }
}