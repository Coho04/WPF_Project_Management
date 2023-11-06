using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Project_management.builders;
using Project_management.helpers;
using Project_management.objects;
using Project_management.providers;
using ToastNotifications;
using ToastNotifications.Messages;

namespace Project_management.windows;

public partial class ProjectWindow
{
    public ObservableCollection<Project> Projects { get; set; }
    public ObservableCollection<Employee> Employees { get; set; }
    public ObservableCollection<Task> Tasks { get; set; }
    private readonly Notifier _notifier;
    public ProjectBuilder? ProjectBuilder { get; private set; }

    public ProjectWindow()
    {
        Projects = new ObservableCollection<Project>(Project.GetAll());
        Employees = new ObservableCollection<Employee>(Employee.GetAll());
        Tasks = new ObservableCollection<Task>(Task.GetAll());
        InitializeComponent();
        _notifier = ToastHelper.CreateToast(this);
        DataContext = this;
    }

    private void ProjectEditButton_Click(object sender, RoutedEventArgs e)
    {
        var project = (Project)((Button)sender).DataContext;
        MessageBox.Show(
            $"Bearbeiten: {project.Title} {project.StartDate} {project.EndDate} {project.Manager.FirstName} {project.Manager.LastName}");
    }

    private void ProjectDeleteButton_Click(object sender, RoutedEventArgs e)
    {
        Projects.Remove((Project)((Button)sender).DataContext);
    }

    private void AddProjectButton_Click(object sender, RoutedEventArgs e)
    {
        EmployeeComboBox.ItemsSource = Employee.GetAll();
        ProjectBuilder = new ProjectBuilder();
        Title = "Projekt hinzufügen";
        ToggleFormAreaVisibility(true);
    }

    private void ProjectGraphicButton_Click(object sender, RoutedEventArgs e)
    {
        var clickedButton = sender as Button;
        if (clickedButton?.DataContext is not Project project) return; 
        new GraphicWindow(project).Show();
    }

    private void CancelCreateProjectButton_Click(object sender, RoutedEventArgs e)
    {
        TitleTextBox.Text = string.Empty;
        StartDateTextBox.Text = string.Empty;
        EmployeeComboBox.Text = string.Empty;
        EndDateTextBox.Text = string.Empty;
        Title = "Projekte";
        ProjectBuilder = null;
        ToggleFormAreaVisibility();
    }

    private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
    {
        if (CheckTextBoxAndMark(TitleTextBox) && CheckDatePickerAndMark(StartDateTextBox) &&
            CheckComboBoxAndMark(EmployeeComboBox) && CheckDatePickerAndMark(EndDateTextBox))
        {
            var title = TitleTextBox.Text;
            var startDate = StartDateTextBox.Text;
            var employee = EmployeeComboBox.SelectedItem as Employee;
            var endDate = EndDateTextBox.Text;

            if (ProjectBuilder == null) return;
            if (employee == null) return;
            ProjectBuilder.SetTitle(title);
            ProjectBuilder.SetStartDate(DateTime.Parse(startDate));
            ProjectBuilder.SetEndDate(DateTime.Parse(endDate));
            ProjectBuilder.SetManager(employee);
            Projects.Add(ProjectBuilder.Build());

            TitleTextBox.Text = string.Empty;
            StartDateTextBox.Text = string.Empty;
            EmployeeComboBox.Text = string.Empty;
            EndDateTextBox.Text = string.Empty;
            ProjectBuilder = null;
            Title = "Projekte";
            ToggleFormAreaVisibility();
            _notifier.ShowSuccess("Projekt wurde erfolgreich hinzugefügt.");
        }
        else
        {
            MessageBox.Show("Alle Felder müssen ausgefüllt sein.");
        }
    }

    public bool CheckTextBoxAndMark(TextBox textBox)
    {
        if (string.IsNullOrWhiteSpace(textBox.Text) && ExtraInfoProvider.GetExtraInfo(textBox).Equals(textBox.Text))
        {
            MarkError(textBox);
            return false;
        }
        textBox.Background = Brushes.White;
        textBox.Foreground = Brushes.Black;
        return true;
    }

    public static bool CheckDatePickerAndMark(DatePicker textBox)
    {
        if (string.IsNullOrWhiteSpace(textBox.Text) && ExtraInfoProvider.GetExtraInfo(textBox).Equals(textBox.Text))
        {
            MarkError(textBox);
            return false;
        }
        textBox.Background = Brushes.White;
        textBox.Foreground = Brushes.Black;
        return true;
    }

    public bool CheckComboBoxAndMark(ComboBox textBox)
    {
        if (string.IsNullOrWhiteSpace(textBox.Text) && ExtraInfoProvider.GetExtraInfo(textBox).Equals(textBox.Text))
        {
            MarkError(textBox);
            return false;
        }
        textBox.Background = Brushes.White;
        textBox.Foreground = Brushes.Black;
        return true;
    }

    public static void MarkError(Control textBox)
    {
        textBox.Background = Brushes.Red;
        textBox.Foreground = Brushes.Azure;
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
        textBox.Text = textBox.Name == "TitleTextBox" ? "Title" : "";
        textBox.Text = textBox.Name == "StartDateTextBox" ? "20.09.2023" : "";
        textBox.Text = textBox.Name == "EmployeeComboBox" ? "Manager" : "";
        textBox.Text = textBox.Name == "EndDateTextBox" ? "20.10.2023" : "";
        textBox.Foreground = Brushes.Gray;
    }

    public void AddTaskButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProjectBuilder == null) return;
        var taskWindow = new CreateTaskWindow(ProjectBuilder);
        taskWindow.TaskAdded += AddTaskWindow_TaskAdded;
        taskWindow.Show();
    }

    private void AddTaskWindow_TaskAdded(object sender, ProjectBuilder projectBuilder)
    {
        ProjectBuilder = projectBuilder;
        TasksDataGrid.ItemsSource = projectBuilder.TaskBuilders;
        _notifier.ShowSuccess("Aufgabe hinzugefügt!");
    }

    private void ToggleFormAreaVisibility(bool visible = false)
    {
        FormArea.Visibility = !visible ? Visibility.Collapsed : Visibility.Visible;
        ListArea.Visibility = visible ? Visibility.Collapsed : Visibility.Visible;
    }
}