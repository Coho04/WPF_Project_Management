using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Project_management.objects;

namespace Project_management.ui.pages.employee;

public partial class Index
{
    public ObservableCollection<Employee> Employees { get; set; }

    public Index()
    {
        LanguageManager.LanguageChanged += UpdateUiForLanguageChange;
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
    
    
    private void UpdateUiForLanguageChange()
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        if (ListView.View is GridView gridView)
        {
            if (gridView.Columns.Count > 0)
                gridView.Columns[0].Header = Strings.ResourceManager.GetString("FirstName", culture);
            if (gridView.Columns.Count > 1)
                gridView.Columns[1].Header = Strings.ResourceManager.GetString("LastName", culture);
            if (gridView.Columns.Count > 2)
                gridView.Columns[2].Header = Strings.ResourceManager.GetString("Department", culture);
            if (gridView.Columns.Count > 3)
                gridView.Columns[3].Header = Strings.ResourceManager.GetString("Phone", culture);
            if (gridView.Columns.Count > 4)
                gridView.Columns[4].Header = Strings.ResourceManager.GetString("Actions", culture);
        }
        AddButton.Content = Strings.ResourceManager.GetString("Add", culture);
    }
}