using System;
using System.Data.SQLite;
using System.IO;

namespace Project_management.helpers;

public class DatabaseHelper
{
    
    private static readonly string DatabaseFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "project_management.sqlite");

    public static SQLiteConnection GetConnection()
    {
        return new SQLiteConnection($"Data Source={DatabaseFilePath};Version=3;");
    }

    public static void CheckAndCreateDatabase()
    {
        if (!File.Exists(DatabaseFilePath))
        {
            SQLiteConnection.CreateFile(DatabaseFilePath);
            Console.WriteLine("Datenbankdatei erstellt.");
        }
        else
        {
            Console.WriteLine("Datenbankdatei existiert bereits.");
        }

        var connection = GetConnection().OpenAndReturn();
        CreateTable(connection, @"
                CREATE TABLE IF NOT EXISTS Department(
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT NOT NULL
                );", "Department");

        CreateTable(connection, @"
                CREATE TABLE IF NOT EXISTS Employee (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    firstname TEXT NOT NULL,
                    lastname TEXT NOT NULL,
                    department_id INTEGER NOT NULL,
                    mobile_phone TEXT NOT NULL,
                     FOREIGN KEY (department_id) REFERENCES Department(id)
                );", "Employee");

        CreateTable(connection, @"
                CREATE TABLE IF NOT EXISTS Project (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT NOT NULL,
                    startdate DATETIME NOT NULL,
                    enddate DATETIME NOT NULL,
                    employee_id INTEGER NOT NULL,
                    FOREIGN KEY (employee_id) REFERENCES Employee(id)
                );", "Project");

        CreateTable(connection, @"
                CREATE TABLE IF NOT EXISTS Task(
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT NOT NULL,
                    description TEXT NOT NULL,
                    duration INTEGER NOT NULL,
                    type TEXT NOT NULL,
                    parent_id INTEGER,
                    project_id INTEGER NOT NULL,
                    FOREIGN KEY (parent_id) REFERENCES Task(id),
                    FOREIGN KEY (project_id) REFERENCES Project(id)
                );", "Task");
        connection.Close();
    }

    private static void CreateTable(SQLiteConnection connection, string createTableQuery, string tableName)
    {
        using var command = new SQLiteCommand(createTableQuery, connection);
        command.ExecuteNonQuery();
        Console.WriteLine($"Tabelle {tableName} überprüft/erstellt.");
    }
}