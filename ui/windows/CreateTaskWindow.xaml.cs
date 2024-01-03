using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Project_management.builders;
using Project_management.objects;

namespace Project_management.ui.windows;

public partial class CreateTaskWindow
{
    public delegate void TaskAddedToBuilderEventHandler(object sender, ProjectBuilder task);
    public delegate void TaskAddedToProjectEventHandler(object sender, Project task);

    public event TaskAddedToBuilderEventHandler? TaskAddedToBuilder;
    public event TaskAddedToProjectEventHandler? TaskAddedToProject;

    public TaskBuilder TaskBuilder { get; set; }
    private ProjectBuilder ProjectBuilder { get; set; }

    private Project Project { get; set; }

    public CreateTaskWindow(ProjectBuilder projectBuilder)
    {
        LanguageManager.LanguageChanged += UpdateUiForLanguageChange;
        InitializeComponent();
        ProjectBuilder = projectBuilder;
        TaskBuilder = new TaskBuilder();
        ParentComboBox.ItemsSource = ProjectBuilder.TaskBuilders;
    }

    public CreateTaskWindow(Project project)
    {
        LanguageManager.LanguageChanged += UpdateUiForLanguageChange;
        InitializeComponent();
        Project = project;
        TaskBuilder = new TaskBuilder();
        ParentComboBox.ItemsSource = project.GetTasks();
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

        var input = ParentComboBox.SelectedItem;
        if (input is TaskBuilder taskBuilder)
        {
            TaskBuilder.SetParent(taskBuilder.Id);
        } else if (input is Task task)
        {
            TaskBuilder.SetParent(task.Id);
        }
        else
        {
            TaskBuilder.SetParent(0);
        }

        if (Project != null)
        {
            TaskBuilder.Build(Project.Id);
            TaskAddedToProject?.Invoke(this, Project);
        } else if (ProjectBuilder != null)
        {
            ProjectBuilder.TaskBuilders.Add(TaskBuilder);
            TaskAddedToBuilder?.Invoke(this, ProjectBuilder);
        }

        Close();
    }

    public void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void UpdateUiForLanguageChange()
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        SaveButton.Content = Strings.ResourceManager.GetString("Save", culture);
        CancelButton.Content = Strings.ResourceManager.GetString("Cancel", culture);
        TitleLabel.Content = Strings.ResourceManager.GetString("Title", culture);
        DescriptionLabel.Content = Strings.ResourceManager.GetString("Description", culture);
        DurationLabel.Content = Strings.ResourceManager.GetString("Duration", culture);
        ParentLabel.Content = Strings.ResourceManager.GetString("Parent", culture);
    }
}