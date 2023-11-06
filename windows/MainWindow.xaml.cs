using System.Windows;
using MahApps.Metro.Controls;

namespace Project_management.windows
{
    public partial class MainWindow
    {
        private MetroWindow _projectWindow;
        private MetroWindow _employeeWindow;

        public MainWindow()
        {
            InitializeComponent();
            _projectWindow = new ProjectWindow();
            _employeeWindow = new EmployeeWindow();
        }

        private void ProjectButton_Click(object sender, RoutedEventArgs e)
        {
            _projectWindow.Show();
            _projectWindow.Activate();
        }

        private void EmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            _employeeWindow.Show();
            _employeeWindow.Activate();
        }
    }
}