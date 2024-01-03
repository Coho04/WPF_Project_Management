using System;
using System.Collections.Generic;
using System.Threading;
using Project_management.objects;

namespace Project_management.ui.windows;

public partial class GraphicWindow
{
    private List<Task> Tasks { get; }

    public GraphicWindow(Project project)
    {
        LanguageManager.LanguageChanged += UpdateUiForLanguageChange;
        InitializeComponent();
        Tasks = project.GetTasks();
        GanttCanvas.Tasks = Tasks;
        GanttCanvas.ProjectStartDate = DateTime.Now.Date;
        GanttCanvas.Project = project;
    }
    
    private void UpdateUiForLanguageChange()
    {
        Title = Strings.ResourceManager.GetString("Graphic", Thread.CurrentThread.CurrentCulture);
    }
}