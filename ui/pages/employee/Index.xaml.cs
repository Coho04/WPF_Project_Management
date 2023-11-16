using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Project_management.objects;

namespace Project_management.ui.pages.employee;

public partial class Index
{
    public ObservableCollection<Employee> Employees { get; set; }

    public Index()
    {
        Employees = new ObservableCollection<Employee>(Employee.GetAll());
        InitializeComponent();
        DataContext = this;
    }

    private void OnCreateButtonClick(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new CreateOrUpdate());
    }

    private void OnListViewSizeChanged(object sender, SizeChangedEventArgs e)
    {
        double totalWidthOfOtherColumns = 0;
        var gridView = (GridView)ListView.View;
        for (var i = 0; i < gridView.Columns.Count - 1; i++)
        {
            totalWidthOfOtherColumns += gridView.Columns[i].ActualWidth;
        }
        var remainingWidth = ListView.ActualWidth - totalWidthOfOtherColumns - SystemParameters.VerticalScrollBarWidth;
        gridView.Columns[4].Width = remainingWidth > 0 ? remainingWidth : 0;
    }

    private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
    {
        var employee = (Employee)((Button)sender).DataContext;
        Employees.Remove(employee);
        employee.Delete();
    }

    private void OnEditButtonClick(object sender, RoutedEventArgs e)
    {
        var employee = (Employee)((Button)sender).DataContext;
        NavigationService?.Navigate(new CreateOrUpdate(employee));
    }
}