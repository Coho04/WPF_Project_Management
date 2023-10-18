using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Project_management.objects;

namespace Project_management;

public partial class EmployeeWindow : MetroWindow
{
    public ObservableCollection<Employee> Employees { get; set; }

    public EmployeeWindow()
    {
        InitializeComponent();
        Employees = new ObservableCollection<Employee>
        {
            new Employee("Jane", "Doe", new Department("Human resources"), "555-555-5555"),
            new Employee("John", "Doe", new Department("It"), "555-555-5555"),
        };
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

        if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName) &&
            !string.IsNullOrWhiteSpace(department))
        {
            var newPerson = new Employee(firstName, lastName, new Department(department), "0155050505050");
            Employees.Add(newPerson);

            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            DepartmentTextBox.Text = string.Empty;
            this.Title = "Mitglieder";
            ToggleFormAreaVisibility();
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
}