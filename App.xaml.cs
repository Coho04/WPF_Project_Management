using System.Windows;
using Project_management.helpers;

namespace Project_management
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DatabaseHelper.CheckAndCreateDatabase();
        }
    }
}