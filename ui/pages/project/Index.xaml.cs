using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
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
        LanguageManager.LanguageChanged += UpdateUiForLanguageChange;
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

    private void UpdateUiForLanguageChange()
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        if (ProjectGrid.Columns[0] is not DataGridTextColumn numberColumn) return;
        numberColumn.Header = Strings.ResourceManager.GetString("Number", culture);

        if (ProjectGrid.Columns[1] is not DataGridTextColumn titleColumn) return;
        titleColumn.Header = Strings.ResourceManager.GetString("Title", culture);

        if (ProjectGrid.Columns[2] is not DataGridTextColumn startDateColumn) return;
        startDateColumn.Header = Strings.ResourceManager.GetString("StartDate", culture);

        if (ProjectGrid.Columns[3] is not DataGridTextColumn endDateColumn) return;
        endDateColumn.Header = Strings.ResourceManager.GetString("EndDate", culture);

        if (ProjectGrid.Columns[4] is not DataGridTextColumn managerColumn) return;
        managerColumn.Header = Strings.ResourceManager.GetString("Manager", culture);

        if (ProjectGrid.Columns[5] is not DataGridTextColumn actionsColumn) return;
        actionsColumn.Header = Strings.ResourceManager.GetString("Actions", culture);
        
        AddButton.Content = Strings.ResourceManager.GetString("Add", culture);
    }
}