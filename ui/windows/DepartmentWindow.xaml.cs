using Project_management.helpers;
using Project_management.ui.pages.department;
using ToastNotifications;
using ToastNotifications.Messages;

namespace Project_management.ui.windows;

public partial class DepartmentWindow
{
    private readonly Notifier _notifier;

    public DepartmentWindow()
    {
        InitializeComponent();
        _notifier = ToastHelper.CreateToast(this);
        DepartmentFrame.Navigate(new Index());
    }
    
    public void SendSuccessToast(string message)
    {
        _notifier.ShowSuccess(message);
    }
}