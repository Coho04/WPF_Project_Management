using System.Collections.ObjectModel;
using System.Windows;
using MahApps.Metro.Controls;
using Project_management.objects;

namespace Project_management;

public partial class GraphicWindow : MetroWindow
{
    public ObservableCollection<Task> Tasks { get; set; }

    public GraphicWindow()
    {
        Task task1 = new Task("1", "Task 1", 10, null);
        Task task2 = new Task("2", "Task 2", 5, task1);
        Task task3 = new Task("3", "Task 3", 5, task2);
        Task task4 = new Task("4", "Task 4", 5, task3);


        Tasks = new ObservableCollection<Task>
        {
            task1,
            task2,
            task3,
            task4
        };

        InitializeComponent();
    }
}