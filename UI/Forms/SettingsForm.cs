using MonLauncherJeux.Core.Constants;
using MonLauncherJeux.Core.Models;
using MonLauncherJeux.UI.Controls;
using MonLauncherJeux.UI.Themes;

namespace MonLauncherJeux.UI;

/// <summary>
/// Formulaire de paramètres complets du launcher
/// </summary>
public sealed class SettingsForm : Form
{
    private readonly AppSettings _settings;
    private readonly UserProfile _profile;
    private readonly ThemeManager _themeManager;

    private TextBox? _usernameBox;
    private ComboBox? _themeCombo;
    private CheckBox? _animationsCheckBox;
    private CheckBox? _autoupdateCheckBox;
    private NumericUpDown? _cardSizeSpinner;
    private ComboBox? _languageCombo;
    private ModernButton? _saveButton;
    private ModernButton? _cancelButton;

    public SettingsForm(AppSettings settings, UserProfile profile, ThemeManager themeManager)
    {
        _settings = settings;
        _profile = profile;
        _themeManager = themeManager;

        ConfigureForm();
        BuildInterface();
        LoadSettings();
    }

    private void ConfigureForm()
    {
        Text = "⚙️ Paramètres";
        Size = new Size(600, 500);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Colors.Background;
        ForeColor = Colors.Text;
        Font = new Font("Segoe UI", 10);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
    }

    private void BuildInterface()
    {
        var tableLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 7,
            Padding = new Padding(15),
            BackColor = Colors.Background
        };

        var usernameLabel = new Label { Text = "Pseudo :", AutoSize = true, ForeColor = Colors.Text };
        _usernameBox = new TextBox { Width = 300, Text = _profile.Username, BackColor = Colors.Card, ForeColor = Colors.Text };
        tableLayout.Controls.Add(usernameLabel, 0, 0);
        tableLayout.Controls.Add(_usernameBox, 1, 0);

        var themeLabel = new Label { Text = "Thème :", AutoSize = true, ForeColor = Colors.Text };
        _themeCombo = new ComboBox { Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, BackColor = Colors.Card, ForeColor = Colors.Text };
        foreach (var themeName in _themeManager.AvailableThemes.Keys)
            _themeCombo.Items.Add(themeName);
        tableLayout.Controls.Add(themeLabel, 0, 1);
        tableLayout.Controls.Add(_themeCombo, 1, 1);

        var animationsLabel = new Label { Text = "Animations :", AutoSize = true, ForeColor = Colors.Text };
        _animationsCheckBox = new CheckBox { Text = "Activer les animations", ForeColor = Colors.Text, BackColor = Colors.Background };
        tableLayout.Controls.Add(animationsLabel, 0, 2);
        tableLayout.Controls.Add(_animationsCheckBox, 1, 2);

        var cardSizeLabel = new Label { Text = "Taille des cartes :", AutoSize = true, ForeColor = Colors.Text };
        _cardSizeSpinner = new NumericUpDown { Width = 80, Minimum = 150, Maximum = 300, Value = _settings.CardSize, BackColor = Colors.Card, ForeColor = Colors.Text };
        tableLayout.Controls.Add(cardSizeLabel, 0, 3);
        tableLayout.Controls.Add(_cardSizeSpinner, 1, 3);

        var languageLabel = new Label { Text = "Langue :", AutoSize = true, ForeColor = Colors.Text };
        _languageCombo = new ComboBox { Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, BackColor = Colors.Card, ForeColor = Colors.Text };
        _languageCombo.Items.AddRange(new[] { "Français", "Anglais", "Allemand", "Espagnol" });
        tableLayout.Controls.Add(languageLabel, 0, 4);
        tableLayout.Controls.Add(_languageCombo, 1, 4);

        var autoupdateLabel = new Label { Text = "Mise à jour auto :", AutoSize = true, ForeColor = Colors.Text };
        _autoupdateCheckBox = new CheckBox { Text = "Vérifier les mises à jour", ForeColor = Colors.Text, BackColor = Colors.Background };
        tableLayout.Controls.Add(autoupdateLabel, 0, 5);
        tableLayout.Controls.Add(_autoupdateCheckBox, 1, 5);

        var buttonPanel = new Panel { Height = 50, Dock = DockStyle.Bottom, BackColor = Colors.Background };
        
        _saveButton = new ModernButton { Text = "💾 Enregistrer", Location = new Point(150, 10), Width = 120 };
        _saveButton.Click += (_, _) => SaveAndClose();
        buttonPanel.Controls.Add(_saveButton);

        _cancelButton = new ModernButton { Text = "❌ Annuler", Location = new Point(280, 10), Width = 120 };
        _cancelButton.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
        buttonPanel.Controls.Add(_cancelButton);

        Controls.Add(tableLayout);
        Controls.Add(buttonPanel);
    }

    private void LoadSettings()
    {
        _usernameBox!.Text = _profile.Username;
        _themeCombo!.SelectedItem = _settings.CurrentTheme;
        _animationsCheckBox!.Checked = _settings.EnableAnimations;
        _cardSizeSpinner!.Value = _settings.CardSize;
        _languageCombo!.SelectedIndex = 0;
        _autoupdateCheckBox!.Checked = _settings.EnableAutoUpdate;
    }

    private void SaveAndClose()
    {
        _profile.Username = _usernameBox!.Text;
        _settings.CurrentTheme = _themeCombo!.SelectedItem?.ToString() ?? "Sombre violet";
        _settings.EnableAnimations = _animationsCheckBox!.Checked;
        _settings.CardSize = (int)_cardSizeSpinner!.Value;
        _settings.EnableAutoUpdate = _autoupdateCheckBox!.Checked;

        DialogResult = DialogResult.OK;
        Close();
    }

    public AppSettings GetSettings() => _settings;
    public UserProfile GetProfile() => _profile;
}
