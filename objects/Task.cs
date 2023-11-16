using System.Collections.Generic;
using System.Data.SQLite;
using Project_management.helpers;

namespace Project_management.objects;

public class Task
{
    public int Id { get; }
    public string Description { get; set; }
    public string Title { get; }
    public int Duration { get; }
    public string Type { get; }
    public Task? Parent { get; }

    public Task(int id, string title, string description, int duration, string type, Task parent)
    {
        Id = id;
        Description = description;
        Title = title;
        Duration = duration;
        Type = type;
        Parent = parent;
    }

    public Task(int id, string title, string description, int duration, string type)
    {
        Id = id;
        Description = description;
        Title = title;
        Duration = duration;
        Type = type;
        Parent = null;
    }

    public static Task GetById(int id)
    {
        var tasksMap = new Dictionary<int, Task>();
        var stack = new Stack<int>();
        stack.Push(id);
        var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        while (stack.Count > 0)
        {
            var currentId = stack.Pop();
            if (tasksMap.ContainsKey(currentId)) continue;
            var command = new SQLiteCommand($"SELECT * FROM Task WHERE id = {currentId}", connection);
            var reader = command.ExecuteReader();
            if (!reader.Read()) continue;
            var title = reader.GetString(1);
            var description = reader.GetString(2);
            var duration = reader.GetInt32(3);
            var projectId = reader.GetInt32(5);
            var project = Project.GetById(projectId);
            if (project == null) continue;
            var type = reader.GetString(6);
            if (reader.IsDBNull(4))
            {
                var task = new Task(id, description, title, duration, type);
                tasksMap[currentId] = task;
            }
            else
            {
                var task = new Task(id, description, title, duration, type, GetById(reader.GetInt32(4)));
                tasksMap[currentId] = task;
            }
        }

        connection.Close();
        return tasksMap.ContainsKey(id) ? tasksMap[id] : null;
    }

    public int GetTaskLevel()
    {
        var level = 0;
        var parent = Parent;
        while (parent != null)
        {
            level++;
            parent = parent.Parent;
        }

        return level * 20;
    }
}