using System.Threading;
using System.Windows;
using Project_management.helpers;
using Project_management.objects;
using Project_management.ui.windows;

namespace Project_management.ui.pages.department;

public partial class CreateOrUpdate
{
    private readonly Department? _department;

    public CreateOrUpdate(Department? department = null)
    {
        LanguageManager.LanguageChanged += UpdateUiForLanguageChange;
        InitializeComponent();
        if (department != null)
        {
            _department = department;
            TitleTextBox.Text = department.Title;
        }

        ControlHelper.RegisterFocusEvents(TitleTextBox);
        DataContext = this;
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Index());
    }

    private void OnSaveButtonClick(object sender, RoutedEventArgs e)
    {
        var title = TitleTextBox.Text;
        if (!string.IsNullOrWhiteSpace(title) && ValidationHelper.CheckAndMark(TitleTextBox) )
        {
            if (_department != null)
            {
                _department.Update(title);
                NavigationService?.Navigate(new Index());
                if (Window.GetWindow(this) is not DepartmentWindow departmentWindow) return;
                departmentWindow.SendSuccessToast(Strings.Department_successfully_updated);
            }
            else
            {
                Department.FindOrCreateByTitle(title);
                NavigationService?.Navigate(new Index());
                if (Window.GetWindow(this) is not DepartmentWindow departmentWindow) return;
                departmentWindow.SendSuccessToast(Strings.Department_added);
            }
        }
        else
        {
            MessageBox.Show(Strings.Please_fill_fields + ": " + Strings.Title, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void UpdateUiForLanguageChange()
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        SaveButton.Content = Strings.ResourceManager.GetString("Save", culture);
        CancelButton.Content = Strings.ResourceManager.GetString("Cancel", culture);
        TitleLabel.Content = Strings.ResourceManager.GetString("Title", culture);
    }
}