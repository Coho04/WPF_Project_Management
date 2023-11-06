using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Project_management.windows;

public partial class DepartmentCreateWindow
{
    public DepartmentCreateWindow()
    {
        InitializeComponent();
    }

    private void OnSaveButtonClick(object sender, RoutedEventArgs e)
    {
        // Ihre Speicherlogik hier
        Console.WriteLine("Speichern Button geklickt");
    }


    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox == null || textBox.Foreground != Brushes.Gray) return;
        textBox.Text = string.Empty;
        textBox.Foreground = Brushes.Black;
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox == null || !string.IsNullOrWhiteSpace(textBox.Text)) return;
        textBox.Text = textBox.Name == TitleTextBox.Name ? "Title" : "";
        textBox.Foreground = Brushes.Gray;
    }
}