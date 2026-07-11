using MonLauncherJeux.Core.Constants;
using MonLauncherJeux.Core.Models;
using MonLauncherJeux.Core.Services;
using MonLauncherJeux.UI.Controls;
using MonLauncherJeux.UI.Themes;

namespace MonLauncherJeux.UI;

/// <summary>
/// Formulaire principal du launcher moderne
/// </summary>
public sealed class MainForm : Form
{
    private readonly GameService _gameService;
    private readonly DataService _dataService;
    private readonly ThemeManager _themeManager;
    private UserProfile _userProfile;
    private AppSettings _appSettings;

    private TopBar? _topBar;
    private Sidebar? _sidebar;
    private SearchBox? _searchBox;
    private FlowLayoutPanel? _gameLibraryPanel;
    private Label? _titleLabel;

    public MainForm(IEnumerable<string> startupFiles)
    {
        _gameService = new GameService();
        _dataService = new DataService();
        _themeManager = new ThemeManager();

        _appSettings = _dataService.LoadSettings();
        _userProfile = _dataService.LoadProfile();

        ConfigureForm();
        BuildInterface();
        ApplyTheme(_themeManager.CurrentTheme);

        _themeManager.ThemeChanged += OnThemeChanged;
        _gameService.GamesChanged += OnGamesChanged;

        AllowDrop = true;
        DragEnter += OnDragEnter;
        DragDrop += OnDragDrop;

        _gameService.AddGames(startupFiles, showErrors: false);
        RefreshGameLibrary();
    }

    private void ConfigureForm()
    {
        Text = "🎮 Mon Launcher Premium";
        Size = new Size(_appSettings.WindowWidth, _appSettings.WindowHeight);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1200, 800);
        BackColor = Colors.Background;
        ForeColor = Colors.Text;
        Font = new Font(_appSettings.FontFamily, _appSettings.FontSize);

        FormClosing += (_, e) =>
        {
            _appSettings.WindowWidth = Width;
            _appSettings.WindowHeight = Height;
            _dataService.SaveSettings(_appSettings);
            _dataService.SaveProfile(_userProfile);
        };
    }

    private void BuildInterface()
    {
        SuspendLayout();

        _topBar = new TopBar();
        _topBar.SetTheme(_themeManager.CurrentTheme);
        Controls.Add(_topBar);

        var mainContainer = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Colors.Background
        };
        mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
        mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _sidebar = new Sidebar();
        _sidebar.SetTheme(_themeManager.CurrentTheme);
        _sidebar.ItemClicked += OnSidebarItemClicked;
        mainContainer.Controls.Add(_sidebar, 0, 0);

        var contentPanel = new Panel { Dock = DockStyle.Fill, BackColor = Colors.Background };
        
        var headerPanel = new Panel { Height = 80, Dock = DockStyle.Top, BackColor = Colors.Background };
        
        _titleLabel = new Label
        {
            Text = "📚 Bibliothèque de Jeux",
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            ForeColor = Colors.Text,
            BackColor = Colors.Background,
            Location = new Point(20, 15),
            AutoSize = true
        };
        headerPanel.Controls.Add(_titleLabel);

        _searchBox = new SearchBox { Location = new Point(250, 20) };
        _searchBox.SetTheme(_themeManager.CurrentTheme);
        _searchBox.SearchChanged += OnSearchChanged;
        headerPanel.Controls.Add(_searchBox);

        contentPanel.Controls.Add(headerPanel);

        _gameLibraryPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Padding = new Padding(20),
            BackColor = Colors.Background
        };
        contentPanel.Controls.Add(_gameLibraryPanel);

        mainContainer.Controls.Add(contentPanel, 1, 0);
        Controls.Add(mainContainer);

        ResumeLayout(false);
        PerformLayout();
    }

    private void RefreshGameLibrary()
    {
        _gameLibraryPanel?.SuspendLayout();
        _gameLibraryPanel?.Controls.Clear();

        var games = _gameService.GetGames();

        if (games.Count == 0)
        {
            var emptyLabel = new Label
            {
                Text = "Aucun jeu trouvé. Ajoute un jeu avec + ou en déposant un .exe/.lnk.",
                Font = new Font("Segoe UI", 14),
                ForeColor = Colors.TextSecondary,
                BackColor = Colors.Background,
                AutoSize = true,
                Margin = new Padding(20)
            };
            _gameLibraryPanel?.Controls.Add(emptyLabel);
        }
        else
        {
            foreach (var game in games)
            {
                var card = new GameCardControl(game, _themeManager.CurrentTheme);
                card.PlayClicked += (_, args) => _gameService.LaunchGame(args.Game);
                card.FavoriteClicked += (_, args) => _gameService.ToggleFavorite(args.Game);
                _gameLibraryPanel?.Controls.Add(card);
            }
        }

        _gameLibraryPanel?.ResumeLayout(false);
        _gameLibraryPanel?.PerformLayout();
    }

    private void ApplyTheme(ThemePalette theme)
    {
        BackColor = theme.Background;
        ForeColor = theme.Text;

        if (_topBar != null) _topBar.SetTheme(theme);
        if (_sidebar != null) _sidebar.SetTheme(theme);
        if (_searchBox != null) _searchBox.SetTheme(theme);
        if (_gameLibraryPanel != null) _gameLibraryPanel.BackColor = theme.Background;
        if (_titleLabel != null) _titleLabel.ForeColor = theme.Text;

        Invalidate();
    }

    private void OnThemeChanged(object? sender, ThemeChangedEventArgs e)
    {
        ApplyTheme(e.Theme);
        _appSettings.CurrentTheme = e.Theme.Name;
        _dataService.SaveSettings(_appSettings);
    }

    private void OnGamesChanged(object? sender, GamesChangedEventArgs e)
    {
        RefreshGameLibrary();
    }

    private void OnSidebarItemClicked(object? sender, SidebarItemClickedEventArgs e)
    {
        switch (e.Index)
        {
            case 0:
                _titleLabel!.Text = "🏠 Accueil";
                break;
            case 1:
                _titleLabel!.Text = "📚 Bibliothèque";
                RefreshGameLibrary();
                break;
            case 2:
                _titleLabel!.Text = "⭐ Favoris";
                ShowFavorites();
                break;
            case 3:
                _titleLabel!.Text = "📊 Statistiques";
                ShowStatistics();
                break;
            case 4:
                _titleLabel!.Text = "⚙️ Paramètres";
                ShowSettings();
                break;
        }
    }

    private void ShowFavorites()
    {
        _gameLibraryPanel?.SuspendLayout();
        _gameLibraryPanel?.Controls.Clear();

        var favorites = _gameService.GetFavorites();
        if (favorites.Count == 0)
        {
            var label = new Label { Text = "Aucun favori", AutoSize = true, ForeColor = Colors.TextSecondary };
            _gameLibraryPanel?.Controls.Add(label);
        }
        else
        {
            foreach (var game in favorites)
            {
                var card = new GameCardControl(game, _themeManager.CurrentTheme);
                card.PlayClicked += (_, args) => _gameService.LaunchGame(args.Game);
                card.FavoriteClicked += (_, args) => _gameService.ToggleFavorite(args.Game);
                _gameLibraryPanel?.Controls.Add(card);
            }
        }

        _gameLibraryPanel?.ResumeLayout();
    }

    private void ShowStatistics()
    {
        MessageBox.Show($"Profil: {_userProfile.Username}\nNiveau: {_userProfile.GetFormattedLevel()}\nJeux: {_userProfile.TotalGamesOwned}\nTemps de jeu: {_userProfile.GetFormattedPlaytime()}", "Statistiques");
    }

    private void ShowSettings()
    {
        var settingsForm = new SettingsForm(_appSettings, _userProfile, _themeManager);
        if (settingsForm.ShowDialog(this) == DialogResult.OK)
        {
            _appSettings = settingsForm.GetSettings();
            _userProfile = settingsForm.GetProfile();
            _themeManager.SetTheme(_appSettings.CurrentTheme);
            _dataService.SaveSettings(_appSettings);
            _dataService.SaveProfile(_userProfile);
        }
    }

    private void OnSearchChanged(object? sender, SearchEventArgs e)
    {
        FilterGames(e.SearchTerm);
    }

    private void FilterGames(string searchTerm)
    {
        _gameLibraryPanel?.SuspendLayout();
        _gameLibraryPanel?.Controls.Clear();

        var filtered = _gameService.GetGames()
            .Where(g => g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (filtered.Count == 0)
        {
            var label = new Label { Text = $"Aucun jeu trouvé pour '{searchTerm}'", AutoSize = true, ForeColor = Colors.TextSecondary };
            _gameLibraryPanel?.Controls.Add(label);
        }
        else
        {
            foreach (var game in filtered)
            {
                var card = new GameCardControl(game, _themeManager.CurrentTheme);
                card.PlayClicked += (_, args) => _gameService.LaunchGame(args.Game);
                card.FavoriteClicked += (_, args) => _gameService.ToggleFavorite(args.Game);
                _gameLibraryPanel?.Controls.Add(card);
            }
        }

        _gameLibraryPanel?.ResumeLayout();
    }

    private void OnDragEnter(object? sender, DragEventArgs e)
    {
        e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) == true ? DragDropEffects.Copy : DragDropEffects.None;
    }

    private void OnDragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is string[] files)
        {
            int added = _gameService.AddGames(files, showErrors: true);
            if (added > 0)
            {
                MessageBox.Show($"{added} jeu(x) ajouté(s) avec succès!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _gameService?.Dispose();
        }
        base.Dispose(disposing);
    }
}