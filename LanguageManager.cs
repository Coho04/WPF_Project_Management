using System;
using System.Globalization;
using System.Threading;

namespace Project_management;

public static class LanguageManager
{
    public static event Action LanguageChanged;

    public static void ChangeLanguage(string cultureName)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
        LanguageChanged?.Invoke();
    }
}
