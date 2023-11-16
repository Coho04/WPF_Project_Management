using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Project_management.objects;
using Project_management.ui.windows;

namespace Project_management.ui.pages.project;

public partial class Index
{
    public ObservableCollection<Project> Projects { get; set; }

    public Index()
    {
        Projects = new ObservableCollection<Project>(Project.GetAll());
        InitializeComponent();
        DataContext = this;
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        var project = (Project)((Button)sender).DataContext;
        NavigationService?.Navigate(new CreateOrUpdate(project));
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var project = (Project)((Button)sender).DataContext;
        Projects.Remove(project);
        project.Delete();
    }

    private void GraphicButton_Click(object sender, RoutedEventArgs e)
    {
        var clickedButton = sender as Button;
        if (clickedButton?.DataContext is not Project project) return;
        new GraphicWindow(project).Show();
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new CreateOrUpdate());
    }
}