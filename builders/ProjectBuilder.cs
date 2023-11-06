using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using Project_management.helpers;
using Project_management.objects;

namespace Project_management.builders;

public class ProjectBuilder
{
    public ObservableCollection<TaskBuilder> TaskBuilders = new ObservableCollection<TaskBuilder>();
    private string Title;
    private DateTime StartDate;
    private DateTime EndDate;
    private Employee Manager;

    public void SetTitle(string title)
    {
        this.Title = title;
    }

    public void SetStartDate(DateTime startDate)
    {
        this.StartDate = startDate;
    }

    public void SetEndDate(DateTime endDate)
    {
        this.EndDate = endDate;
    }

    public ProjectBuilder SetManager(Employee manager)
    {
        this.Manager = manager;
        return this;
    }

    public Project Build()
    {
        var connection = DatabaseHelper.GetConnection();
        var insertQuery = "INSERT INTO Project (title, startdate, enddate, employee_id)" +
                          " VALUES (@Title, @StartDate, @EndDate, @Manager_Id);" +
                          "SELECT last_insert_rowid();";
        var command = new SQLiteCommand(insertQuery, connection);
        command.Parameters.AddWithValue("@Title", Title);
        command.Parameters.AddWithValue("@StartDate", StartDate);
        command.Parameters.AddWithValue("@EndDate", EndDate);
        command.Parameters.AddWithValue("@Manager_Id", Manager.Id);
        command.ExecuteNonQuery();
        int id = Convert.ToInt32(command.ExecuteScalar());
        connection.Close();

        foreach (var taskBuilder in TaskBuilders)
        {
            taskBuilder.Build(id);
        }
        return new Project(id, Title, StartDate, EndDate, Manager);
    }
}