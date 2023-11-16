using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Project_management.helpers;
using Project_management.objects;
using Project_management.providers;
using Project_management.ui.windows;

namespace Project_management.ui.pages.employee;

public partial class CreateOrUpdate
{
    private readonly Employee? _employee;
    public ObservableCollection<Department> Departments { get; set; }

    public CreateOrUpdate(Employee? employee = null)
    {
        Departments = new ObservableCollection<Department>(Department.GetAll());
        InitializeComponent();
        if (employee != null)
        {
            _employee = employee;
            FirstNameTextBox.Text = employee.FirstName;
            LastNameTextBox.Text = employee.LastName;
            if (employee.Department != null)
            {
                DepartmentComboBox.SelectedItem = SearchDepartmentInCollection(employee.Department.Title);
            }

            MobilePhoneTextBox.Text = employee.MobilePhone;
        }

        ControlHelper.RegisterFocusEvents(FirstNameTextBox);
        ControlHelper.RegisterFocusEvents(LastNameTextBox);
        ControlHelper.RegisterFocusEvents(DepartmentComboBox);
        ControlHelper.RegisterFocusEvents(MobilePhoneTextBox);
        DataContext = this;
    }

    private void OnAddDepartmentButtonClick(object sender, RoutedEventArgs e)
    {
        var window = MainWindow._departmentCreateWindow;
        if (window == null || !window.IsLoaded)
        {
            window = new DepartmentCreateWindow();
            window.DepartmentAdded += AddDepartmentWindow;
            MainWindow._departmentCreateWindow = window;
        }

        if (!window.IsVisible)
        {
            window.Show();
        }

        window.Activate();
    }

    private void AddDepartmentWindow(object sender, Department department)
    {
        var departmentInCollection = SearchDepartmentInCollection(department.Title);
        if (departmentInCollection == null)
        {
            DepartmentComboBox.SelectedItem = department;
            Departments.Add(department);
        }
        else
        {
            DepartmentComboBox.SelectedItem = departmentInCollection;
        }

        if (Window.GetWindow(this) is not EmployeeWindow employeeWindow) return;
        employeeWindow.SendSuccessToast("Abteilung hinzugefügt!");
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Index());
    }

    private void OnSaveButtonClick(object sender, RoutedEventArgs e)
    {
        if (ValidationHelper.CheckAndMark(FirstNameTextBox) && ValidationHelper.CheckAndMark(LastNameTextBox) &&
            ValidationHelper.CheckAndMark(DepartmentComboBox))
        {
            var firstName = FirstNameTextBox.Text;
            var lastName = LastNameTextBox.Text;
            var department = DepartmentComboBox.SelectionBoxItem as Department;
            var mobilePhone = MobilePhoneTextBox.Text;
            if (mobilePhone == ExtraInfoProvider.GetExtraInfo(MobilePhoneTextBox))
            {
                mobilePhone = "";
            }

            Employee.UpdateOrCreate(firstName, lastName, department, mobilePhone, _employee?.Id ?? null);
            NavigationService?.Navigate(new Index());
            if (Window.GetWindow(this) is not EmployeeWindow employeeWindow) return;
            employeeWindow.SendSuccessToast("Mitglied wurde erfolgreich hinzugefügt.");
        }
        else
        {
            MessageBox.Show("Please enter both first and last name.");
        }
    }

    private Department? SearchDepartmentInCollection(string value)
    {
        return Departments.FirstOrDefault(dept => dept.Title == value);
    }
}