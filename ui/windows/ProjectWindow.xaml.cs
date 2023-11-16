using Project_management.helpers;
using Project_management.ui.pages.project;
using ToastNotifications;
using ToastNotifications.Messages;

namespace Project_management.ui.windows;

public partial class ProjectWindow
{
    private readonly Notifier _notifier;

    public ProjectWindow()
    {
        InitializeComponent();
        _notifier = ToastHelper.CreateToast(this);
        ProjectFrame.Navigate(new Index());
    }
    
    public void SendSuccessToast(string message)
    {
        _notifier.ShowSuccess(message);
    }
}