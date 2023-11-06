using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Project_management.helpers;
using Project_management.objects;
using ToastNotifications;
using ToastNotifications.Messages;

namespace Project_management.windows;

public partial class EmployeeWindow
{
    private readonly Notifier _notifier;
    public ObservableCollection<Employee> Employees { get; set; }
    public ObservableCollection<Department> Departments { get; set; }

    public EmployeeWindow()
    {
        InitializeComponent();
        Employees = new ObservableCollection<Employee>(Employee.GetAll());
        Departments = new ObservableCollection<Department>(Department.GetAll());
        _notifier = ToastHelper.CreateToast(this);
        DataContext = this;
    }

    private void EmployeeEditButton_Click(object sender, RoutedEventArgs e)
    {
        var employee = (Employee)((Button)sender).DataContext;
        FirstNameTextBox.Text = employee.FirstName;
        LastNameTextBox.Text = employee.LastName;
        // DepartmentTextBox.Text = employee.Department.Title;
        MobilePhoneTextBox.Text = employee.MobilePhone;

        IdTextArea.Visibility = Visibility.Visible;
        IdTextBox.IsEnabled = false;
        IdTextBox.Text = employee.Id.ToString();

        CreateEmployeeButton.Click -= CreateEmployeeButton_Click;
        CreateEmployeeButton.Click += UpdateEmployeeButton_Click;

        RemoveFocusFromTextBox(FirstNameTextBox);
        RemoveFocusFromTextBox(LastNameTextBox);
        // RemoveFocusFromTextBox(DepartmentTextBox);
        RemoveFocusFromTextBox(MobilePhoneTextBox);

        Title = "Mitglied bearbeiten";
        ToggleFormAreaVisibility(true);
    }

    private void EmployeeDeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var employee = (Employee)((Button)sender).DataContext;
        employee.DeleteEmployee();
        Employees.Remove(employee);
    }

    private void AddPersonButton_Click(object sender, RoutedEventArgs e)
    {
        Title = "Mitglied hinzufügen";
        ToggleFormAreaVisibility(true);
    }
    
    private void AddDepartmentButton_Click(object sender, RoutedEventArgs e)
    {
        new DepartmentCreateWindow().Show();
    }

    private void CreateEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        var firstName = FirstNameTextBox.Text;
        var lastName = LastNameTextBox.Text;
        // var department = DepartmentTextBox.Text;
        var mobilePhone = MobilePhoneTextBox.Text;

        if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName) /*&&
            !string.IsNullOrWhiteSpace(department)*/)
        {
            // var newEmployee = Employee.CreateEmployee(firstName, lastName, Department.FindOrCreateByTitle(department),
            //     mobilePhone);
            // Employees.Add(newEmployee);

            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            // DepartmentTextBox.Text = string.Empty;
            MobilePhoneTextBox.Text = string.Empty;
            Title = "Mitglieder";
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
        if (textBox == null) return;
        if (textBox.Foreground != Brushes.Gray) return;
        textBox.Text = string.Empty;
        textBox.Foreground = Brushes.Black;
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox == null) return;
        if (!string.IsNullOrWhiteSpace(textBox.Text)) return;
        textBox.Text = textBox.Name == "FirstNameTextBox" ? "First Name" : "Last Name";
        textBox.Foreground = Brushes.Gray;
    }

    private void CancelCreateEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        FirstNameTextBox.Text = string.Empty;
        LastNameTextBox.Text = string.Empty;
        // DepartmentTextBox.Text = string.Empty;
        MobilePhoneTextBox.Text = string.Empty;

        IdTextArea.Visibility = Visibility.Hidden;
        IdTextBox.IsEnabled = false;
        IdTextBox.Text = string.Empty;

        Title = "Mitglieder";
        ToggleFormAreaVisibility();
    }

    private void ShowSuccessToast(string message)
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

    private void RemoveFocusFromTextBox(TextBox textBox, bool remove = true)
    {
        if (remove)
        {
            textBox.GotFocus -= TextBox_GotFocus;
            textBox.LostFocus -= TextBox_LostFocus;
            textBox.Foreground = Brushes.Black;
        }
        else
        {
            textBox.GotFocus += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;
            textBox.Foreground = Brushes.Gray;
        }
    }

    private void UpdateEmployeeButton_Click(object sender, RoutedEventArgs e)
    {
        var firstName = FirstNameTextBox.Text;
        var lastName = LastNameTextBox.Text;
        // var dep = DepartmentTextBox.Text;
        var mobilePhone = MobilePhoneTextBox.Text;
        var id = IdTextBox.Text;

        if (CheckIfRequiredFieldsFilled())
        {
            // var department = Department.FindOrCreateByTitle(dep);
            // Employee.UpdateEmployee(firstName, lastName, department, mobilePhone, int.Parse(id));
            CreateEmployeeButton.Click += CreateEmployeeButton_Click;
            CreateEmployeeButton.Click -= UpdateEmployeeButton_Click;
            ListView.ItemsSource = Employee.GetAll();
            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            // DepartmentTextBox.Text = string.Empty;
            MobilePhoneTextBox.Text = string.Empty;
            IdTextBox.Text = string.Empty;
            Title = "Mitglieder";
            ToggleFormAreaVisibility();
            ShowSuccessToast("Mitglied wurde erfolgreich aktualisiert.");
        }
        else
        {
            MessageBox.Show("Please enter both first and last name.");
        }
    }

    private bool CheckIfRequiredFieldsFilled()
    {
        return !string.IsNullOrWhiteSpace(FirstNameTextBox.Text) &&
               !string.IsNullOrWhiteSpace(LastNameTextBox.Text)/* &&
               !string.IsNullOrWhiteSpace(DepartmentTextBox.Text)*/;
    }
}