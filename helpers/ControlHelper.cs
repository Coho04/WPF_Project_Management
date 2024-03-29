using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Project_management.providers;

namespace Project_management.helpers;

public class ControlHelper
{
    public static void RegisterFocusEvents(Control control)
    {
        control.GotFocus += GotFocus;
        control.LostFocus += LostFocus;
    }

    private static void LostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox) return;
        if (string.IsNullOrWhiteSpace(textBox.Text))
        {
            textBox.Undo();
            textBox.Foreground = Brushes.Gray;
        }
        else
        {
            textBox.Foreground = Brushes.Black;
        }
    }

    private static void GotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox) return;
        if (textBox.Foreground != Brushes.Gray) return;
        textBox.Text = textBox.Text == "" || textBox.Text == ExtraInfoProvider.GetExtraInfo(textBox)
            ? textBox.Text = ExtraInfoProvider.GetExtraInfo(textBox)
            : string.Empty;
        textBox.Foreground = Brushes.Black;
    }
}