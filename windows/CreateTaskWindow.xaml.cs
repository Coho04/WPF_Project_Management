using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Project_management.builders;

namespace Project_management.windows;

public partial class CreateTaskWindow 
{
    public delegate void TaskAddedEventHandler(object sender, ProjectBuilder task);
    public event TaskAddedEventHandler? TaskAdded;

    public TaskBuilder TaskBuilder { get; set; }
    private ProjectBuilder ProjectBuilder { get; set; }

    public CreateTaskWindow(ProjectBuilder projectBuilder)
    {
        InitializeComponent();
        ProjectBuilder = projectBuilder;
        TaskBuilder = new TaskBuilder();
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox == null) return;
        if (textBox.Foreground != Brushes.Gray) return;
        textBox.Text = string.Empty;
        textBox.Foreground = Brushes.Black;
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox == null) return;
        if (!string.IsNullOrWhiteSpace(textBox.Text)) return;
        textBox.Foreground = Brushes.Gray;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        TaskBuilder.SetTitle(TitleTextBox.Text);
        TaskBuilder.SetDescription(DescriptionTextBox.Text);
        TaskBuilder.SetDuration(int.Parse(DurationTextBox.Text));
        
        var input = ParentTextBox.Text;
        var success = int.TryParse(input, out var result);
        TaskBuilder.SetParent(success ? result : 0);

        ProjectBuilder.TaskBuilders.Add(TaskBuilder);
        TaskAdded?.Invoke(this, ProjectBuilder);
        Close();
    }

    public void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}