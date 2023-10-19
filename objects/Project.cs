using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Project_management.helpers;

namespace Project_management.objects;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Employee Manager { get; set; }

    public Project(int id, string title, DateTime startDate, DateTime endDate, Employee manager)
    {
        this.Id = id;
        this.Title = title;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.Manager = manager;
    }

    public static List<Project> GetAll()
    {
        var projects = new List<Project>();
        var command = new SQLiteCommand("SELECT * FROM Project;", DatabaseHelper.GetConnection());
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var title = reader.GetString(1);
            var startDate = reader.GetDateTime(2);
            var endDate = reader.GetDateTime(3);
            var employee = reader.GetInt32(4);
            var project = new Project(id, title, startDate, endDate, Employee.GetById(employee));
            projects.Add(project);
        }
        return projects;
    }
}