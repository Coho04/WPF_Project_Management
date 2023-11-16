using System.Windows;
using MahApps.Metro.Controls;

namespace Project_management.ui.windows
{
    public partial class MainWindow
    {
        private MetroWindow _projectWindow;
        private MetroWindow _employeeWindow;
        public static DepartmentCreateWindow _departmentCreateWindow;
        public static DepartmentWindow _departmentWindow;

        public MainWindow()
        {
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
    }
}