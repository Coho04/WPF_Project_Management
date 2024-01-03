using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using ControlzEx.Theming;

namespace Project_management.ui.windows
{
    public partial class MainWindow
    {
        private ProjectWindow _projectWindow;
        private EmployeeWindow _employeeWindow;
        private SettingsWindow _settingsWindow;
        public static DepartmentCreateWindow _departmentCreateWindow;
        public static DepartmentWindow _departmentWindow;

        public MainWindow()
        {
            LanguageManager.LanguageChanged += UpdateUIForLanguageChange;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            InitializeComponent();
            _projectWindow = new ProjectWindow();
            _employeeWindow = new EmployeeWindow();
            _departmentCreateWindow = new DepartmentCreateWindow();
        }

        private void ProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_projectWindow == null || !_projectWindow.IsLoaded)
            {
                _projectWindow = new ProjectWindow();
            }
            if (!_projectWindow.IsVisible)
            {
                _projectWindow.Show();
            }
            _projectWindow.Activate();
        }
        
        private void DepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (_departmentWindow == null || !_departmentWindow.IsLoaded)
            {
                _departmentWindow = new DepartmentWindow();
            }
            if (!_departmentWindow.IsVisible)
            {
                _departmentWindow.Show();
            }
            _departmentWindow.Activate();
        }

        private void EmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_employeeWindow == null || !_employeeWindow.IsLoaded)
            {
                _employeeWindow = new EmployeeWindow();
            }
            if (!_employeeWindow.IsVisible)
            {
                _employeeWindow.Show();
            }
            _employeeWindow.Activate();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_settingsWindow == null || !_settingsWindow.IsLoaded)
            {
                _settingsWindow = new SettingsWindow();
            }
            if (!_settingsWindow.IsVisible)
            {
                _settingsWindow.Show();
            }
            _settingsWindow.Activate();
        }
        
        private void UpdateUIForLanguageChange()
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            EmployeeButton.Content = Strings.ResourceManager.GetString("Employee", culture);
            ProjectButton.Content = Strings.ResourceManager.GetString("Project", culture);
            DepartmentButton.Content = Strings.ResourceManager.GetString("Department", culture);
            SettingsButton.Content = Strings.ResourceManager.GetString("Settings", culture);
            Title = Strings.ResourceManager.GetString("HomePage", culture);

        }
    }
}