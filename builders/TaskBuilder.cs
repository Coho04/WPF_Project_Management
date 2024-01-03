using System;
using System.Data.SQLite;
using Project_management.helpers;

namespace Project_management.builders;

public class TaskBuilder
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public int? Parent { get; set; }

    public TaskBuilder SetTitle(string title)
    {
        Title = title;
        return this;
    }

    public TaskBuilder SetDescription(string description)
    {
        Description = description;
        return this;
    }

    public TaskBuilder SetDuration(int duration)
    {
        Duration = duration;
        return this;
    }

    public TaskBuilder SetParent(int parent)
    {
        Parent = parent;
        return this;
    }

    public void Build(int projectId)
    {
        using var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        const string insertQuery = "INSERT INTO Task (title, description, duration, parent_id, project_id)" +
                          " VALUES (@Title, @Description, @Duration, @ManagerId, @ProjectId);";
        using var command = new SQLiteCommand(insertQuery, connection);
        command.Parameters.AddWithValue("@Title", Title);
        command.Parameters.AddWithValue("@Description", Description);
        command.Parameters.AddWithValue("@Duration", Duration);
        command.Parameters.AddWithValue("@ManagerId", Parent != null ? Parent : null);
        command.Parameters.AddWithValue("@ProjectId", projectId);
        command.ExecuteNonQuery();
        connection.Close();
    }
}