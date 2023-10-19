using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Project_management.helpers;
using Project_management.objects;
using ToastNotifications;
using ToastNotifications.Messages;

namespace Project_management.windows;

public partial class EmployeeWindow : MetroWindow
{
    private readonly Notifier _notifier;
    public ObservableCollection<Employee> Employees { get; set; }

    public EmployeeWindow()
    {
        InitializeComponent();
        Employees = new ObservableCollection<Employee>(Employee.GetAll());
        _notifier = ToastHelper.CreateToast(this);
        this.DataContext = this;
    }

    private void EmployeeEditButton_Click(object sender, RoutedEventArgs e)
    {
        var employee = (Employee)((Button)sender).DataContext;
        MessageBox.Show($"Bearbeiten: {employee.FirstName} {employee.LastName}");
    }

    private void EmployeeDeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var employee = (Employee)((Button)sender).DataContext;
        Employees.Remove(employee);
    }

    private void AddPersonButton_Click(object sender, RoutedEventArgs e)
    {
        this.Title = "Mitglied hinzufügen";
        ToggleFormAreaVisibility(true);
    }

    private void CreateEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        var firstName = FirstNameTextBox.Text;
        var lastName = LastNameTextBox.Text;
        var department = DepartmentTextBox.Text;
        var mobilePhone = MobilePhoneTextBox.Text;

        if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName) && !string.IsNullOrWhiteSpace(department))
        {
            var newEmployee = Employee.CreateEmployee(firstName, lastName, Department.FindOrCreateByTitle(department), mobilePhone);
            Employees.Add(newEmployee);

            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            DepartmentTextBox.Text = string.Empty;
            MobilePhoneTextBox.Text = string.Empty;
            this.Title = "Mitglieder";
            ToggleFormAreaVisibility();
            ShowSuccessToast("Mitglied wurde erfolgreich hinzugefügt.");
        }
        else
        {
            MessageBox.Show("Please enter both first and last name.");
        }
    }

    private void ToggleFormAreaVisibility(bool visible = false)
    {
        FormArea.Visibility = !visible ? Visibility.Collapsed : Visibility.Visible;
        ListArea.Visibility = visible ? Visibility.Collapsed : Visibility.Visible;
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox != null && textBox.Foreground == Brushes.Gray)
        {
            textBox.Text = string.Empty;
            textBox.Foreground = Brushes.Black;
        }
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
        {
            textBox.Text = textBox.Name == "FirstNameTextBox" ? "First Name" : "Last Name";
            textBox.Foreground = Brushes.Gray;
        }
    }

    private void CancelCreateEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        FirstNameTextBox.Text = string.Empty;
        LastNameTextBox.Text = string.Empty;
        DepartmentTextBox.Text = string.Empty;
        MobilePhoneTextBox.Text = string.Empty;

        this.Title = "Mitglieder";
        ToggleFormAreaVisibility();
    }

    public void ShowSuccessToast(string message)
    {
        _notifier.ShowSuccess(message);
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
}