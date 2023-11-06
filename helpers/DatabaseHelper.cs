using System;
using System.Data.SQLite;
using System.IO;

namespace Project_management.helpers;

public class DatabaseHelper
{
    private static readonly string DatabaseFileName = "project_management.sqlite";

    private static readonly string DatabaseFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFileName);

    public static readonly string ConnectionString = $"Data Source={DatabaseFilePath};Version=3;";

    public static SQLiteConnection GetConnection()
    {
        var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        return connection;
    }

    public static void CheckAndCreateDatabase()
    {
        var databaseFilePath = DatabaseFilePath;
        if (!File.Exists(databaseFilePath))
        {
            SQLiteConnection.CreateFile(databaseFilePath);
            Console.WriteLine("Datenbankdatei erstellt.");
        }
        else
        {
            Console.WriteLine("Datenbankdatei existiert bereits.");
        }

        var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
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
                    parent_id INTEGER,
                    project_id INTEGER NOT NULL,
                    FOREIGN KEY (parent_id) REFERENCES Task(id),
                    FOREIGN KEY (project_id) REFERENCES Project(id)
                );", "Task");
        connection.Close();
    }

    private static void CreateTable(SQLiteConnection connection, string createTableQuery, string tableName)
    {
        new SQLiteCommand(createTableQuery, connection).ExecuteNonQuery();
        Console.WriteLine($"Tabelle {tableName} überprüft/erstellt.");
    }
}