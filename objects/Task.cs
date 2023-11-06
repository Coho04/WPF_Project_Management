using System.Collections.Generic;
using System.Data.SQLite;
using Project_management.helpers;

namespace Project_management.objects;

public class Task
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public int Duration { get; set; }
    public Task Parent { get; set; }

    public Task(int id, string description, string title, int duration, Project project, Task parent)
    {
        Id = id;
        Description = description;
        Title = title;
        Duration = duration;
        Parent = parent;
    }

    public Task(int id, string title, string description, int duration, Project project)
    {
        Id = id;
        Description = description;
        Title = title;
        Duration = duration;
        Parent = null;
    }

    public static List<Task> GetAll()
    {
        var tasks = new List<Task>();
        var command = new SQLiteCommand("SELECT * FROM Task;", DatabaseHelper.GetConnection());
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var title = reader.GetString(1);
            var description = reader.GetString(2);
            var duration = reader.GetInt32(3);
            var project = Project.GetById(reader.GetInt32(5));
            if (project == null) continue;
            if (reader.IsDBNull(4))
            {
                var employee = new Task(id, description, title, duration, project);
                tasks.Add(employee);
            }
            else
            {
                var employee = new Task(id, description, title, duration, project, GetById(reader.GetInt32(4)));
                tasks.Add(employee);
            }
        }

        return tasks;
    }

    public static Task GetById(int id)
    {
        var tasksMap = new Dictionary<int, Task>();
        var stack = new Stack<int>();
        stack.Push(id);
        while (stack.Count > 0)
        {
            var currentId = stack.Pop();
            if (tasksMap.ContainsKey(currentId)) continue;
            var command = new SQLiteCommand($"SELECT * FROM Task WHERE id = {currentId}",
                DatabaseHelper.GetConnection());
            var reader = command.ExecuteReader();
            if (!reader.Read()) continue;
            var title = reader.GetString(1);
            var description = reader.GetString(2);
            var duration = reader.GetInt32(3);
            var projectId = reader.GetInt32(5);
            var project = Project.GetById(projectId);
            if (project == null) continue;
            if (reader.IsDBNull(4))
            {
                var task = new Task(id, description, title, duration, project);
                tasksMap[currentId] = task;
            }
            else
            {
                var task = new Task(id, description, title, duration, project,
                    GetById(reader.GetInt32(4)));
                tasksMap[currentId] = task;
            }
        }

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