using System;
using System.Collections.Generic;
using Project_management.objects;

namespace Project_management.windows;

public partial class GraphicWindow
{
    public List<Task> Tasks { get; set; }

    public GraphicWindow(Project project)
    {
        InitializeComponent();
        Tasks = project.GetTasks();
        ganttCanvas.Tasks = Tasks;
        ganttCanvas.ProjectStartDate = DateTime.Now.Date;
        ganttCanvas.Project = project;
    }
}