using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Project_management.builders;
using Project_management.helpers;
using Project_management.objects;
using Project_management.ui.windows;
using Task = Project_management.objects.Task;

namespace Project_management.ui.pages.project;

public partial class CreateOrUpdate
{
    public ProjectBuilder? ProjectBuilder { get; set; }
    public Project? Project { get; set; }
    public ObservableCollection<object> Tasks { get; set; }

    public CreateOrUpdate(Project? project = null)
    {
        LanguageManager.LanguageChanged += UpdateUiForLanguageChange;
        InitializeComponent();
        var employees = Employee.GetAll();
        EmployeeComboBox.ItemsSource = employees;
        if (project != null)
        {
            Project = project;
            TitleTextBox.Text = project.Title;
            StartDateTextBox.Text = project.StartDate.ToString();
            EndDateTextBox.Text = project.EndDate.ToString();
            if (project.Manager is Employee selectedEmployee)
            {
                EmployeeComboBox.SelectedItem = employees.Find(employee => employee.Id == selectedEmployee.Id);
            }

            TasksDataGrid.ItemsSource = project.GetTasks();

            TitleTextBox.Foreground = Brushes.Black;
            StartDateTextBox.Foreground = Brushes.Black;
            EndDateTextBox.Foreground = Brushes.Black;
            EmployeeComboBox.Foreground = Brushes.Black;
        }
        else
        {
            ProjectBuilder = new ProjectBuilder();
        }

        ControlHelper.RegisterFocusEvents(TitleTextBox);
        ControlHelper.RegisterFocusEvents(StartDateTextBox);
        ControlHelper.RegisterFocusEvents(EndDateTextBox);
        ControlHelper.RegisterFocusEvents(EmployeeComboBox);
        DataContext = this;
    }

    public void AddTaskButton_Click(object sender, RoutedEventArgs e)
    {
        if (Project != null)
        {
            var taskWindow = new CreateTaskWindow(Project);
            taskWindow.TaskAddedToProject += AddTaskWindow_TaskAdded;
            taskWindow.Show();
        }
        else if (ProjectBuilder != null)
        {
            var taskWindow = new CreateTaskWindow(ProjectBuilder);
            taskWindow.TaskAddedToBuilder += AddTaskWindow_TaskAdded;
            taskWindow.Show();
        }
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
            if (employee == null) return;
            if (Window.GetWindow(this) is not ProjectWindow projectWindow) return;
            if (Project != null)
            {
                Project.Update(title, DateTime.Parse(startDate), employee, DateTime.Parse(endDate));
                projectWindow.SendSuccessToast(Strings.Project_successfully_updated);
            }
            else if (ProjectBuilder != null)
            {
                ProjectBuilder.SetTitle(title);
                ProjectBuilder.SetStartDate(DateTime.Parse(startDate));
                ProjectBuilder.SetEndDate(DateTime.Parse(endDate));
                ProjectBuilder.SetManager(employee);
                ProjectBuilder.Build();
                projectWindow.SendSuccessToast(Strings.Project_successfully_added);
            }

            NavigationService?.Navigate(new Index());
        }
        else
        {
            MessageBox.Show(Strings.Please_fill_fields + ": " + Strings.Title + ", " + Strings.Manager 
                            + ", " + Strings.StartDate + ", " + Strings.EndDate + "."
                , Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Index());
    }   
    
    private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
    {
        var clickedButton = sender as Button;
        if (clickedButton?.DataContext is not Task task) return;
        task.Delete();
        TasksDataGrid.ItemsSource = Project.GetTasks();
    }

    private void AddTaskWindow_TaskAdded(object sender, ProjectBuilder projectBuilder)
    {
        ProjectBuilder = projectBuilder;
        TasksDataGrid.ItemsSource = projectBuilder.TaskBuilders;
        if (Window.GetWindow(this) is not ProjectWindow projectWindow) return;
        projectWindow.SendSuccessToast(Strings.Task_added);
    }

    private void AddTaskWindow_TaskAdded(object sender, Project project)
    {
        Project = project;
        TasksDataGrid.ItemsSource = project.GetTasks();
        if (Window.GetWindow(this) is not ProjectWindow projectWindow) return;
        projectWindow.SendSuccessToast(Strings.Task_added);
    }

    private void UpdateUiForLanguageChange()
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        CancelButton.Content = Strings.ResourceManager.GetString("Cancel", culture);
        SaveButton.Content = Strings.ResourceManager.GetString("Save", culture);
        TitleLabel.Content = Strings.ResourceManager.GetString("Title", culture);
        StartDateLabel.Content = Strings.ResourceManager.GetString("StartDate", culture);
        ManagerLabel.Content = Strings.ResourceManager.GetString("Manager", culture);
        EndDateLabel.Content = Strings.ResourceManager.GetString("EndDate", culture);
        AddTaskButton.Content = Strings.ResourceManager.GetString("Add_Task", culture);
    }
}