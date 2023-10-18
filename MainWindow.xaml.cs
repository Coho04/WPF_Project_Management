using System.Windows;
using MahApps.Metro.Controls;

namespace Project_management
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var secondWindow = new ProjectWindow();
            secondWindow.Show();
        }

        private void EmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var thirdWindow = new EmployeeWindow();
            thirdWindow.Show();
        }
    }
}