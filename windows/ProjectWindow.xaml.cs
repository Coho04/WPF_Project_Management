using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Project_management.helpers;
using Project_management.objects;
using ToastNotifications;
using ToastNotifications.Messages;

namespace Project_management.windows;

public partial class ProjectWindow : MetroWindow
{
    public ObservableCollection<Project> Projects { get; set; }
    private Notifier _notifier;

    public ProjectWindow()
    {
        Projects = new ObservableCollection<Project>(Project.GetAll());
        InitializeComponent();
        _notifier = ToastHelper.CreateToast(this);
        this.DataContext = this;
    }


    private void ProjectEditButton_Click(object sender, RoutedEventArgs e)
    {
        var project = (Project)((Button)sender).DataContext;
        MessageBox.Show(
            $"Bearbeiten: {project.Title} {project.StartDate} {project.EndDate} {project.Manager.FirstName} {project.Manager.LastName}");
    }

    private void ProjectDeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var project = (Project)((Button)sender).DataContext;
        Projects.Remove(project);
    }

    private void AddProjectButton_Click(object sender, RoutedEventArgs e)
    {
        // var newProject = new Project("Project 3", new DateTime(2023, 10, 18), new DateTime(2023, 10, 18),
        // new Employee("John", "Doe", new Department("It"), "555-555-5555"));
        // Projects.Add(newProject);
    }

    private void ProjectGraphicButton_Click(object sender, RoutedEventArgs e)
    {
        var graphicWindow = new GraphicWindow();
        graphicWindow.Show();
    }

    public void ShowSuccessToast(string message)
    {
        _notifier.ShowSuccess(message);
    }
}