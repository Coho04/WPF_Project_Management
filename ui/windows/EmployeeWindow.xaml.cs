using Project_management.helpers;
using Project_management.ui.pages.employee;
using ToastNotifications;
using ToastNotifications.Messages;

namespace Project_management.ui.windows;

public partial class EmployeeWindow
{
    private readonly Notifier _notifier;

    public EmployeeWindow()
    {
        InitializeComponent();
        EmployeeFrame.Navigate(new Index());
        _notifier = ToastHelper.CreateToast(this);
        DataContext = this;
    }

    public void SendSuccessToast(string message)
    {
        _notifier.ShowSuccess(message);
    }
}