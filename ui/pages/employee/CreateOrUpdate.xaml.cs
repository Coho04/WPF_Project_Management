using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
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
        LanguageManager.LanguageChanged += UpdateUiForLanguageChange;
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
        employeeWindow.SendSuccessToast(Strings.Department_added);
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
            employeeWindow.SendSuccessToast(Strings.Employee_Success_created);
        }
        else
        {
            MessageBox.Show(
                Strings.Please_fill_fields + ": " + Strings.FirstName + ", " + Strings.LastName + ", " +
                Strings.Department + "!", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private Department? SearchDepartmentInCollection(string value)
    {
        return Departments.FirstOrDefault(dept => dept.Title == value);
    }

    private void UpdateUiForLanguageChange()
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        CancelButton.Content = Strings.ResourceManager.GetString("Cancel", culture);
        SaveButton.Content = Strings.ResourceManager.GetString("Save", culture);
        FirstNameLabel.Content = Strings.ResourceManager.GetString("FirstName", culture);
        LastNameLabel.Content = Strings.ResourceManager.GetString("LastName", culture);
        DepartmentLabel.Content = Strings.ResourceManager.GetString("Department", culture);
        PhoneLabel.Content = Strings.ResourceManager.GetString("Phone", culture);
    }
}