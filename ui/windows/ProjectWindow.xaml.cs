using System.Threading;
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
        LanguageManager.LanguageChanged += UpdateUiForLanguageChange;
    }

    public void SendSuccessToast(string message)
    {
        _notifier.ShowSuccess(message);
    }

    private void UpdateUiForLanguageChange()
    {
        Title = Strings.ResourceManager.GetString("Project", Thread.CurrentThread.CurrentCulture);
    }
}