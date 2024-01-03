using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Project_management.objects;

namespace Project_management.ui.windows;

public partial class DepartmentCreateWindow
{
    public delegate void DepartmentAddedEventHandler(object sender, Department department);
    public event DepartmentAddedEventHandler? DepartmentAdded;
    
    public DepartmentCreateWindow()
    {
        LanguageManager.LanguageChanged += UpdateUiForLanguageChange;
        InitializeComponent();
    }

    private void OnSaveButtonClick(object sender, RoutedEventArgs e)
    {
        var department = Department.FindOrCreateByTitle(TitleTextBox.Text);
        DepartmentAdded?.Invoke(this, department);
        Close();
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox) return;
        if (textBox.Foreground != Brushes.Gray) return;
        textBox.Text = string.Empty;
        textBox.Foreground = Brushes.Black;
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox) return;
        if (!string.IsNullOrWhiteSpace(textBox.Text)) return;
        textBox.Text = textBox.Name == TitleTextBox.Name ? "Title" : "";
        textBox.Foreground = Brushes.Gray;
    }
    
    private void UpdateUiForLanguageChange()
    {
        SaveButton.Content = Strings.ResourceManager.GetString("Generate", Thread.CurrentThread.CurrentCulture);
    }
}