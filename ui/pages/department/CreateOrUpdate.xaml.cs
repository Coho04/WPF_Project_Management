using System.Windows;
using Project_management.helpers;
using Project_management.objects;

namespace Project_management.ui.pages.department;

public partial class CreateOrUpdate
{
    private readonly Department? _department;

    public CreateOrUpdate(Department? department = null)
    {
        InitializeComponent();
        if (department != null)
        {
            // _department = department;
            // FirstNameTextBox.Text = employee.FirstName;
        }
        ControlHelper.RegisterFocusEvents(FirstNameTextBox);
        DataContext = this;
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Index());
    }

    private void OnSaveButtonClick(object sender, RoutedEventArgs e)
    {
        // var firstName = FirstNameTextBox.Text;
        // var lastName = LastNameTextBox.Text;
        // var department = DepartmentComboBox.SelectionBoxItem as Department;
        // var mobilePhone = MobilePhoneTextBox.Text;
        // if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName) && department != null)
        // {
        //     Employee.UpdateOrCreate(firstName, lastName, department, mobilePhone, _employee?.Id ?? null);
        //     NavigationService?.Navigate(new Index());
        //     if (Window.GetWindow(this) is not EmployeeWindow employeeWindow) return;
        //     employeeWindow.SendSuccessToast("Mitglied wurde erfolgreich hinzugefügt.");
        // }
        // else
        // {
        //     MessageBox.Show("Please enter both first and last name.");
        // }
    }

    // private void RemoveFocusFromTextBox(TextBox textBox, bool remove = true)
    // {
    //     if (remove)
    //     {
    //         textBox.GotFocus -= TextBox_GotFocus;
    //         textBox.LostFocus -= TextBox_LostFocus;
    //         textBox.Foreground = Brushes.Black;
    //     }
    //     else
    //     {
    //         textBox.GotFocus += TextBox_GotFocus;
    //         textBox.LostFocus += TextBox_LostFocus;
    //         textBox.Foreground = Brushes.Gray;
    //     }
    // }
}