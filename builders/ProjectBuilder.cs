using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using Project_management.helpers;
using Project_management.objects;

namespace Project_management.builders;

public class ProjectBuilder
{
    public ObservableCollection<TaskBuilder> TaskBuilders;
    private string Title;
    private DateTime StartDate;
    private DateTime EndDate;
    private Employee Manager;
    
    public ProjectBuilder()
    {
        TaskBuilders = new ObservableCollection<TaskBuilder>();
    }

    public void SetTitle(string title)
    {
        Title = title;
    }

    public void SetStartDate(DateTime startDate)
    {
        StartDate = startDate;
    }

    public void SetEndDate(DateTime endDate)
    {
        this.EndDate = endDate;
    }

    public void SetManager(Employee manager)
    {
        Manager = manager;
    }

    public Project Build()
    {
        var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        const string insertQuery = "INSERT INTO Project (title, startdate, enddate, employee_id)" +
                          " VALUES (@Title, @StartDate, @EndDate, @Manager_Id);" +
                          "SELECT last_insert_rowid();";
        var command = new SQLiteCommand(insertQuery, connection);
        command.Parameters.AddWithValue("@Title", Title);
        command.Parameters.AddWithValue("@StartDate", StartDate);
        command.Parameters.AddWithValue("@EndDate", EndDate);
        command.Parameters.AddWithValue("@Manager_Id", Manager.Id);
        command.ExecuteNonQuery();
        var id = Convert.ToInt32(command.ExecuteScalar());
        connection.Close();
        foreach (var taskBuilder in TaskBuilders)
        {
            taskBuilder.Build(id);
        }
        return new Project(id, Title, StartDate, EndDate, Manager);
    }
}