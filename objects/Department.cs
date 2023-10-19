using System;
using System.Data.SQLite;
using Project_management.helpers;

namespace Project_management.objects;

public class Department
{
    public int Id { get; set; }
    public string Title { get; set; }

    public Department(int id, string title)
    {
        this.Id = id;
        this.Title = title;
    }

    public static Department GetById(int id)
    {
        var connection = DatabaseHelper.GetConnection();
        var command = new SQLiteCommand("SELECT * FROM Department where id = @Id;", connection);
        command.Parameters.AddWithValue("@Id", id);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var title = reader.GetString(1);
            return new Department(0, title);
        }
        connection.Close();
        return null;
    }

    public static Department CreateDepartment(string title)
    {
        var connection = DatabaseHelper.GetConnection();
        var command = new SQLiteCommand("INSERT INTO Department (title) VALUES (@Title);", connection);
        command.Parameters.AddWithValue("@Title", title);
        command.ExecuteNonQuery();

        var id = (int)new SQLiteCommand("SELECT last_insert_rowid();", connection).ExecuteScalar();
        connection.Close();
        return new Department(id, title);
    }

    public static Department FindOrCreateByTitle(string title)
    {
        var connection = new SQLiteConnection(DatabaseHelper.ConnectionString);
        connection.Open();

        string findQuery = "SELECT Id FROM Department WHERE Title = @Title;";
        var findCommand = new SQLiteCommand(findQuery, connection);
        findCommand.Parameters.AddWithValue("@Title", title);
        var result = findCommand.ExecuteScalar();
        if (result != null)
        {
            return new Department(Convert.ToInt32(result), title);
        }

        string insertQuery = "INSERT INTO Department (Title) VALUES (@Title); SELECT last_insert_rowid();";
        var insertCommand = new SQLiteCommand(insertQuery, connection);
        insertCommand.Parameters.AddWithValue("@Title", title);
        long lastId = (long)insertCommand.ExecuteScalar();
        return new Department((int)lastId, title);
    }
}