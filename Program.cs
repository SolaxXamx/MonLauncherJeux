using System.Diagnostics;
using MonLauncherJeux.UI;

namespace MonLauncherJeux;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        using var form = new MainForm(args);
        Application.Run(form);
    }
}