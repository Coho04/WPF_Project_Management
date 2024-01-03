using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Project_management.helpers;

namespace Project_management.objects;

public class Department
{
    public int Id { get; }
    public string Title { get; set; }

    public Department(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public static Department? GetById(int id)
    {
        using var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        using var command = new SQLiteCommand("SELECT * FROM Department where id = @Id;", connection);
        command.Parameters.AddWithValue("@Id", id);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var title = reader.GetString(1);
            reader.Close();
            connection.Close();
            return new Department(0, title);
        }

        reader.Close();
        connection.Close();
        return null;
    }

    public static List<Department> GetAll()
    {
        var departments = new List<Department>();
        using var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        using var command = new SQLiteCommand("SELECT * FROM Department;", connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            departments.Add(new Department(reader.GetInt32(0), reader.GetString(1)));
        }

        reader.Close();
        connection.Close();
        return departments;
    }

    public void Update(string title)
    {
        using var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        using var updateCommand = new SQLiteCommand("UPDATE Department SET Title = @Title WHERE Id = @Id;", connection);
        updateCommand.Parameters.AddWithValue("@Title", title);
        updateCommand.Parameters.AddWithValue("@Id", Id);
        updateCommand.ExecuteNonQuery();
    }

    public static Department FindOrCreateByTitle(string title)
    {
        using var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        using var findCommand = new SQLiteCommand("SELECT Id FROM Department WHERE Title = @Title;", connection);
        findCommand.Parameters.AddWithValue("@Title", title);
        var result = findCommand.ExecuteScalar();
        if (result != null)
        {
            connection.Close();
            return new Department(Convert.ToInt32(result), title);
        }

        const string insertQuery = "INSERT INTO Department (Title) VALUES (@Title); SELECT last_insert_rowid();";
        using var insertCommand = new SQLiteCommand(insertQuery, connection);
        insertCommand.Parameters.AddWithValue("@Title", title);
        var lastId = (long)insertCommand.ExecuteScalar();
        connection.Close();
        return new Department((int)lastId, title);
    }

    public void Delete()
    {
        using var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        using var command = new SQLiteCommand("DELETE FROM Department WHERE id = @Id;", connection);
        command.Parameters.AddWithValue("@Id", Id);
        command.ExecuteNonQuery();
        connection.Close();
    }
}