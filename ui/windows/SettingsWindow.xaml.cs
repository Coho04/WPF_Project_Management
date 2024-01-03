using System.Threading;
using System.Windows;
using ControlzEx.Theming;
using MahApps.Metro.Controls;

namespace Project_management.ui.windows;

public partial class SettingsWindow
{
    public SettingsWindow()
    {
        LanguageManager.LanguageChanged += UpdateUiForLanguageChange;
        InitializeComponent();
    }

    private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is not ToggleSwitch toggleSwitch) return;
        LanguageManager.ChangeLanguage(toggleSwitch.IsOn ? "de-DE" : "en-US");
    }

    private void DarkMode_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is not ToggleSwitch toggleSwitch) return;
        var baseTheme = toggleSwitch.IsOn ? ThemeManager.BaseColorDark : ThemeManager.BaseColorLight;
        ThemeManager.Current.ChangeThemeBaseColor(Application.Current, baseTheme);
    }
    
    private void UpdateUiForLanguageChange()
    {
        Title = Strings.ResourceManager.GetString("Settings", Thread.CurrentThread.CurrentCulture);
    }
}