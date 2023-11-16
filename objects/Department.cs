using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Project_management.helpers;

namespace Project_management.objects;

public class Department
{
    public int Id { get; set; }
    public string Title { get; set; }

    public Department(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public static Department GetById(int id)
    {
        var connection = DatabaseHelper.GetConnection().OpenAndReturn();
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
        var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        var command = new SQLiteCommand("INSERT INTO Department (title) VALUES (@Title);", connection);
        command.Parameters.AddWithValue("@Title", title);
        command.ExecuteNonQuery();

        var id = (int)new SQLiteCommand("SELECT last_insert_rowid();", connection).ExecuteScalar();
        connection.Close();
        return new Department(id, title);
    }

    public static List<Department> GetAll()
    {
        var departments = new List<Department>();
        var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        var command = new SQLiteCommand("SELECT * FROM Department;",connection);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            departments.Add(new Department(reader.GetInt32(0), reader.GetString(1)));
        }
        connection.Close();
        return departments;
    }

    public static Department FindOrCreateByTitle(string title)
    {
        var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        const string findQuery = "SELECT Id FROM Department WHERE Title = @Title;";
        var findCommand = new SQLiteCommand(findQuery, connection);
        findCommand.Parameters.AddWithValue("@Title", title);
        var result = findCommand.ExecuteScalar();
        if (result != null)
        {
            connection.Close();
            return new Department(Convert.ToInt32(result), title);
        }

        const string insertQuery = "INSERT INTO Department (Title) VALUES (@Title); SELECT last_insert_rowid();";
        var insertCommand = new SQLiteCommand(insertQuery, connection);
        insertCommand.Parameters.AddWithValue("@Title", title);
        var lastId = (long)insertCommand.ExecuteScalar();
        connection.Close();
        return new Department((int)lastId, title);
    }

    public void Delete()
    {
        var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        var command = new SQLiteCommand("DELETE FROM Department WHERE id = @Id;", connection);
        command.Parameters.AddWithValue("@Id", Id);
        command.ExecuteNonQuery();
        connection.Close();
    }
}