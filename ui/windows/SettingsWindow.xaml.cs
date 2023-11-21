using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using ControlzEx.Theming;
using MahApps.Metro.Controls;

namespace Project_management.ui.windows;

public partial class SettingsWindow
{
    public SettingsWindow()
    {
        InitializeComponent();
    }

    private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is not ToggleSwitch toggleSwitch) return;
        var newCulture = toggleSwitch.IsOn ? "de-DE" : "en-US";
        var localizationManager = Application.Current.Resources["LocalizationManager"] as LocalizationManager;
        localizationManager?.ChangeCulture(new CultureInfo(newCulture));
        Console.WriteLine(Thread.CurrentThread.CurrentCulture.Name);
    }

    private void DarkMode_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is not ToggleSwitch toggleSwitch) return;
        var baseTheme = toggleSwitch.IsOn ? ThemeManager.BaseColorDark : ThemeManager.BaseColorLight;
        ThemeManager.Current.ChangeThemeBaseColor(Application.Current, baseTheme);
    }
}