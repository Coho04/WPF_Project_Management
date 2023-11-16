using System;
using System.Windows.Controls;
using System.Windows.Media;
using Project_management.providers;

namespace Project_management.helpers;

public class ValidationHelper
{
    public static bool CheckAndMark(Control control)
    {
        var validation = control switch
        {
            TextBox textBox => CheckTextBoxAndMark(textBox),
            ComboBox comboBox => CheckComboBoxAndMark(comboBox),
            DatePicker datePicker => CheckDatePickerAndMark(datePicker),
            _ => throw new ArgumentOutOfRangeException(nameof(control), control, null)
        };
        if (validation)
        {
            control.Background = Brushes.White;
            control.Foreground = Brushes.Black;
        }
        else
        {
            control.Background = Brushes.Red;
            control.Foreground = Brushes.Azure;
        }

        return validation;
    }

    private static bool CheckComboBoxAndMark(ComboBox textBox)
    {
        return !string.IsNullOrWhiteSpace(textBox.Text) && !ExtraInfoProvider.GetExtraInfo(textBox).Equals(textBox.Text);
    }

    private static bool CheckTextBoxAndMark(TextBox textBox)
    {
        return !string.IsNullOrWhiteSpace(textBox.Text) && !ExtraInfoProvider.GetExtraInfo(textBox).Equals(textBox.Text);
    }

    private static bool CheckDatePickerAndMark(DatePicker textBox)
    {
        return !string.IsNullOrWhiteSpace(textBox.Text) && !ExtraInfoProvider.GetExtraInfo(textBox).Equals(textBox.Text);
    }
}