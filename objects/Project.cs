using System;

namespace Project_management.objects;

public class Project
{
    public string Title { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public Employee Manager { get; set; }
    
    public Project(string title, DateTime startDate, DateTime endDate, Employee manager)
    {
        this.Title = title;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.Manager = manager;
    }
}