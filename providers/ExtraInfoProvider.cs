using System.Windows;

namespace Project_management.providers;

public static class ExtraInfoProvider
{
    public static readonly DependencyProperty ExtraInfoProperty =
        DependencyProperty.RegisterAttached(
            "ExtraInfo",
            typeof(string),
            typeof(ExtraInfoProvider),
            new PropertyMetadata(string.Empty)
        );

    public static string GetExtraInfo(DependencyObject obj)
    {
        return (string)obj.GetValue(ExtraInfoProperty);
    }

    public static void SetExtraInfo(DependencyObject obj, string value)
    {
        obj.SetValue(ExtraInfoProperty, value);
    }
}
