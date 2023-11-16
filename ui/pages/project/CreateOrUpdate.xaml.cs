using System;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.VisualBasic;
using Project_management.builders;
using Project_management.helpers;
using Project_management.objects;
using Project_management.ui.windows;

namespace Project_management.ui.pages.project;

public partial class CreateOrUpdate
{
    public ProjectBuilder? ProjectBuilder { get; set; }
    public Project? Project { get; set; }
    public ObservableCollection<object> Tasks { get; set; }

    public CreateOrUpdate(Project? project = null)
    {
        InitializeComponent();
        Project = project;
        ProjectBuilder = new ProjectBuilder();
        EmployeeComboBox.ItemsSource = Employee.GetAll();
        ControlHelper.RegisterFocusEvents(TitleTextBox);
        ControlHelper.RegisterFocusEvents(StartDateTextBox);
        ControlHelper.RegisterFocusEvents(EndDateTextBox);
        ControlHelper.RegisterFocusEvents(EmployeeComboBox);
        DataContext = this;
    }

    public void AddTaskButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProjectBuilder == null) return;
        var taskWindow = new CreateTaskWindow(ProjectBuilder);
        taskWindow.TaskAdded += AddTaskWindow_TaskAdded;
        taskWindow.Show();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (ValidationHelper.CheckAndMark(TitleTextBox) && ValidationHelper.CheckAndMark(StartDateTextBox) &&
            ValidationHelper.CheckAndMark(EmployeeComboBox) && ValidationHelper.CheckAndMark(EndDateTextBox))
        {
            var title = TitleTextBox.Text;
            var startDate = StartDateTextBox.Text;
            var employee = EmployeeComboBox.SelectedItem as Employee;
            var endDate = EndDateTextBox.Text;

            if (ProjectBuilder == null) return;
            if (employee == null) return;
            if (Window.GetWindow(this) is not ProjectWindow projectWindow) return;
            if (Project != null)
            {
                // Project.Update(title, startDate, employee, endDate);
                projectWindow.SendSuccessToast("Projekt wurde erfolgreich aktualisiert.");
            }
            else
            {
                ProjectBuilder.SetTitle(title);
                ProjectBuilder.SetStartDate(DateTime.Parse(startDate));
                ProjectBuilder.SetEndDate(DateTime.Parse(endDate));
                ProjectBuilder.SetManager(employee);
                ProjectBuilder.Build();
                projectWindow.SendSuccessToast("Projekt wurde erfolgreich hinzugefügt.");
            }

            NavigationService?.Navigate(new Index());
        }
        else
        {
            MessageBox.Show("Alle Felder müssen ausgefüllt sein.");
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Index());
    }

    private void AddTaskWindow_TaskAdded(object sender, ProjectBuilder projectBuilder)
    {
        ProjectBuilder = projectBuilder;
        TasksDataGrid.ItemsSource = projectBuilder.TaskBuilders;
        if (Window.GetWindow(this) is not ProjectWindow projectWindow) return;
        projectWindow.SendSuccessToast("Aufgabe hinzugefügt!");
    }
}