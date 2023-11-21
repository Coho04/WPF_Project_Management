using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace Project_management;

public class LocalizationManager: INotifyPropertyChanged
{
    private Dictionary<string, string> _localizedResources = new Dictionary<string, string>();

    public event PropertyChangedEventHandler PropertyChanged;

    public string this[string key]
    {
        get => _localizedResources.TryGetValue(key, out var value) ? value : $"[{key}]";
        set
        {
            _localizedResources[key] = value;
            OnPropertyChanged(key);
        }
    }

    public void ChangeCulture(CultureInfo culture)
    {
        this["Greeting"] = LoadResource(culture, "Greeting");
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private string LoadResource(CultureInfo culture, string key)
    {
        var resourceDictionary = new ResourceDictionary()
        {
            Source = new Uri($"pack://application:,,,/YourAssemblyName;component/Localization/{culture.Name}.xaml")
        };

        return resourceDictionary[key] as string;
    }
}