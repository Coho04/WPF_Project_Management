using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Project_management.objects;

namespace Project_management.ui.pages.department;

public partial class Index
{
    public ObservableCollection<Department> Departments { get; set; }

    public Index()
    {
        LanguageManager.LanguageChanged += UpdateUiForLanguageChange;
        Departments = new ObservableCollection<Department>(Department.GetAll());
        InitializeComponent();
        DataContext = this;
    }

    private void OnCreateButtonClick(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new CreateOrUpdate());
    }

    private void OnListViewSizeChanged(object sender, SizeChangedEventArgs e)
    {
        // double totalWidthOfOtherColumns = 0;
        // var gridView = (GridView)ListView.View;
        // for (var i = 0; i < gridView.Columns.Count - 1; i++)
        // {
        //     totalWidthOfOtherColumns += gridView.Columns[i].ActualWidth;
        // }
        //
        // var remainingWidth = ListView.ActualWidth - totalWidthOfOtherColumns - SystemParameters.VerticalScrollBarWidth;
        // gridView.Columns[4].Width = remainingWidth > 0 ? remainingWidth : 0;
    }

    private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
    {
        var department = (Department)((Button)sender).DataContext;
        department.Delete();
        Departments.Remove(department);
    }

    private void OnEditButtonClick(object sender, RoutedEventArgs e)
    {
        var department = (Department)((Button)sender).DataContext;
        NavigationService?.Navigate(new CreateOrUpdate(department));
    }
    
    private void UpdateUiForLanguageChange()
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        if (ListView.View is GridView gridView)
        {
            if (gridView.Columns.Count > 0)
                gridView.Columns[0].Header = Strings.ResourceManager.GetString("Name", culture);
            if (gridView.Columns.Count > 1)
                gridView.Columns[1].Header = Strings.ResourceManager.GetString("Actions", culture);
        }
        AddButton.Content = Strings.ResourceManager.GetString("Add", culture);
    }
}