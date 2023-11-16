using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Project_management.objects;

namespace Project_management.ui.pages.department;

public partial class Index
{
    public ObservableCollection<Department> Departments { get; set; }

    public Index()
    {
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
}