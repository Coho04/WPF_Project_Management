using System.ComponentModel;
using System.Threading;

namespace Project_management.providers;

public class ResourceProvider : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public string this[string resourceName]
    {
        get => Strings.ResourceManager.GetString(resourceName, Thread.CurrentThread.CurrentCulture);
    }

    public void NotifyLanguageChanged()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
    }
}
