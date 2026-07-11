# 🎮 Mon Launcher Premium

Un launcher de jeux moderne et professionnel inspiré de **Steam**, **Xbox Series X** et **PlayStation 5**, entièrement développé en **C# WinForms**.

## ✨ Fonctionnalités

### 🎨 Interface Premium
- **Design moderne** avec coins arrondis et ombres
- **Barre latérale** de navigation fluide
- **Barre supérieure** avec heure, batterie et profil
- **Cartes animées** avec effets de survol
- **Recherche instantanée** des jeux

### 🎭 7 Thèmes Professionnels
1. **Sombre violet** - Élégant et moderne
2. **Steam** - Inspiré de Steam
3. **Xbox** - Style Xbox Series X
4. **PS5** - Inspiré de PlayStation 5
5. **Cyber** - Cyberpunk futuriste
6. **Windows 11** - Design Windows 11
7. **OLED** - Optimisé pour écrans OLED

### 🎮 Gestion des Jeux
- ✅ Ajouter des jeux (.exe et .lnk)
- ✅ Drag & Drop pour importer
- ✅ Extraction automatique des icônes
- ✅ Système de favoris
- ✅ Statistiques (lancements, temps de jeu)
- ✅ Lancement direct depuis le launcher

### 👤 Profil Utilisateur
- Pseudo personnalisable
- Niveau et XP
- Statistiques globales
- Photo de profil
- Statut utilisateur

### ⚙️ Paramètres Complets
- Sélection de thème
- Activation/désactivation des animations
- Taille des cartes ajustable
- Sélection de police
- Choix de langue
- Auto-update

### 💾 Sauvegarde Persistante
Tous les paramètres sont sauvegardés en **JSON**:
- `games.json` - Bibliothèque de jeux
- `profile.json` - Profil utilisateur
- `settings.json` - Paramètres de l'application

## 🚀 Installation & Lancement

### Option 1: Avec .NET SDK (Développement)
```bash
# Clone le repository
git clone https://github.com/SolaxXamx/MonLauncherJeux.git
cd MonLauncherJeux

# Lance directement
run-launcher.bat
```

### Option 2: EXE Portable (32-bit)
```bash
# Génère l'EXE autonome
publish-windows-x86.bat

# Lance depuis publish\MonLauncherJeux.exe
```

### Option 3: Commandes Manuelles
```bash
# Restaure les dépendances
dotnet restore

# Compile en Release
dotnet build -c Release

# Lance
dotnet run --project MonLauncherJeux.csproj
```

## 📋 Scripts Disponibles

| Script | Description |
|--------|-------------|
| `run-launcher.bat` | Lance le launcher (dev ou exe) |
| `publish-windows-x86.bat` | Crée un EXE 32-bit autonome |
| `clean.bat` | Nettoie les fichiers temporaires |
| `restore.bat` | Restaure les dépendances NuGet |

## 📁 Structure du Projet

```
MonLauncherJeux/
├── Program.cs                      # Point d'entrée
├── Core/
│   ├── Constants/                  # Chemins et couleurs
│   ├── Models/                     # GameEntry, UserProfile, AppSettings
│   └── Services/                   # DataService, GameService, IconImporter
├── UI/
│   ├── Forms/                      # MainForm, SettingsForm
│   ├── Controls/                   # ModernButton, GameCardControl, SearchBox, etc.
│   ├── Themes/                     # ThemePalette, ThemeManager
│   └── Animations/                 # AnimationManager
└── Assets/                         # Ressources
```

## 🛠️ Prérequis

- **Windows 10/11** (x86 ou x64)
- **.NET 8 SDK** (pour développement)
  - Télécharge: https://dotnet.microsoft.com/download/dotnet/8.0
  - Sélectionne "Windows x86" pour PC 32 bits

## 💻 Développement

### Édition du code
Tu peux éditer le code avec:
- **Visual Studio 2022** (recommandé)
- **Visual Studio Code** + C# extension
- **JetBrains Rider**

### Compilation
```bash
dotnet build -c Release
```

### Tests
```bash
dotnet run --project MonLauncherJeux.csproj
```

## 🐛 Dépannage

### "dotnet: commande introuvable"
**Solution**: Installe .NET 8 SDK depuis https://dotnet.microsoft.com/download/dotnet/8.0

### Erreur lors du lancement
```bash
# Nettoie et réinstalle
clean.bat
restore.bat
run-launcher.bat
```

### L'EXE est trop volumineux
C'est normal! L'EXE inclut le runtime .NET complet (~100-150 MB) pour être autonome.

## 📊 Spécifications Techniques

| Aspect | Détails |
|--------|----------|
| **Langage** | C# 12 (.NET 8) |
| **UI Framework** | WinForms |
| **Architecture** | MVVM-Inspired, Service-Based |
| **Compatibilité** | Windows 10/11 (x86, x64) |
| **Persistence** | JSON (System.Text.Json) |
| **Taille EXE** | ~50 MB (framework) + ~100 MB (runtime) |

## 🎯 Fonctionnalités Futures

- [ ] Support multi-utilisateur
- [ ] Synchronisation cloud
- [ ] Capture d'écran des jeux
- [ ] Statistiques avancées
- [ ] Intégration Steam
- [ ] Shortcuts personnalisés
- [ ] Thème personnalisé

## 📝 Licence

Ce projet est libre d'utilisation. Fais-en ce que tu veux! 😊

## 👨‍💻 Auteur

Développé par **SolaxXamx** | GitHub: https://github.com/SolaxXamx

## 🤝 Contribution

Les contributions sont bienvenues! N'hésite pas à:
- Signaler des bugs
- Proposer des améliorations
- Créer des pull requests

---

**Enjoy ton launcher premium! 🎮✨**
