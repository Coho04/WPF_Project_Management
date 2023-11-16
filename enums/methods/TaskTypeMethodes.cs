using System;
using System.Windows.Media;
using Project_management.objects;

namespace Project_management.enums.methods;

public class TaskTypeMethodes
{
    public static string GetTitle(TaskType taskType) => taskType switch
    {
        TaskType.Initiation => "Initiierung",
        TaskType.Planning => "Planung",
        TaskType.Execution => "Ausführung",
        TaskType.MonitoringAndControl => "ÜberwachungUndSteuerung",
        TaskType.Closure => "Abschluss",
        TaskType.QualityControl => "Qualitätskontrolle",
        TaskType.Documentation => "Dokumentation",
        TaskType.Acceptance => "Abnahme",
        TaskType.ProjectHandover => "Projektübergabe",
        TaskType.PostMortemAnalysis => "Nachbereitung",
        _ => "Unbekannt"
    };

    public static TaskType GetType(Task task) => task.Type switch
    {
        "Initiierung" => TaskType.Initiation,
        "Planung" => TaskType.Planning,
        "Ausführung" => TaskType.Execution,
        "ÜberwachungUndSteuerung" => TaskType.MonitoringAndControl,
        "Abschluss" => TaskType.Closure,
        "Qualitätskontrolle" => TaskType.QualityControl,
        "Dokumentation" => TaskType.Documentation,
        "Abnahme" => TaskType.Acceptance,
        "Projektübergabe" => TaskType.ProjectHandover,
        "Nachbereitung" => TaskType.PostMortemAnalysis,
        _ => TaskType.Unknown
    };

    public static Brush GetColor(Task task)
    {
        Console.WriteLine(GetType(task));
        Console.WriteLine(task.Type);
        return GetType(task) switch
        {
            TaskType.Initiation => Brushes.Aqua,
            TaskType.Planning => Brushes.Aquamarine,
            TaskType.Execution => Brushes.Blue,
            TaskType.MonitoringAndControl => Brushes.Brown,
            TaskType.Closure => Brushes.Chartreuse,
            TaskType.QualityControl => Brushes.Chocolate,
            TaskType.Documentation => Brushes.Crimson,
            TaskType.Acceptance => Brushes.Gold,
            TaskType.ProjectHandover => Brushes.Fuchsia,
            TaskType.PostMortemAnalysis => Brushes.Indigo,
            _ => Brushes.Blue
        };
    }
}