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
        Id = id;
        Title = title;
        StartDate = startDate;
        EndDate = endDate;
        Manager = manager;
    }

    public static List<Project> GetAll()
    {
        var projects = new List<Project>();
        var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        var command = new SQLiteCommand("SELECT * FROM Project;", connection);
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

        connection.Close();
        return projects;
    }

    public static Project? GetById(int id)
    {
        var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        var command = new SQLiteCommand("SELECT * FROM Project where id = " + id, connection);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var title = reader.GetString(1);
            var startDate = reader.GetDateTime(2);
            var endDate = reader.GetDateTime(3);
            var employeeId = reader.GetInt32(4);
            connection.Close();
            return new Project(id, title, startDate, endDate, Employee.GetById(employeeId));
        }

        connection.Close();
        return null;
    }

    public List<Task> GetTasks()
    {
        var tasks = new List<Task>();
        var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        var command = new SQLiteCommand("SELECT * FROM Task where project_id = @Id;", connection);
        command.Parameters.AddWithValue("@Id", Id);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var title = reader.GetString(1);
            var description = reader.GetString(2);
            var duration = reader.GetInt32(3);
            var task = reader.GetString(6);
            if (reader.IsDBNull(4))
            {
                var project = new Task(id, title, description, duration, task);
                tasks.Add(project);
            }
            else
            {
                var parentTask = Task.GetById(reader.GetInt32(4));
                var project = new Task(id, title, description, duration, task, parentTask);
                tasks.Add(project);
            }
        }

        connection.Close();
        return tasks;
    }

    public void Delete()
    {
        var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        var command = new SQLiteCommand("DELETE FROM Project WHERE id = @Id;", connection);
        command.Parameters.AddWithValue("@Id", Id);
        command.ExecuteNonQuery();
        connection.Close();
    }

    public long GetCompleteTaskDuration()
    {
        var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        var command = new SQLiteCommand("SELECT SUM(duration) FROM Task where project_id = @Id;", connection);
        command.Parameters.AddWithValue("@Id", Id);
        var result = command.ExecuteScalar();
        connection.Close();
        if (result == DBNull.Value) return 0;
        return (long)result;
    }

    private bool HasTasks()
    {
        var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        var command = new SQLiteCommand("SELECT count(*) FROM Task where project_id = @Id;", connection);
        command.Parameters.AddWithValue("@Id", Id);
        var result = command.ExecuteScalar();
        connection.Close();
        return Convert.ToInt32(result) > 0;
    }

    public bool HasTasksProperty => HasTasks();
}