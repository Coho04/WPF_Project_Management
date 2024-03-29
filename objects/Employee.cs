using System.Collections.Generic;
using System.Data.SQLite;
using Project_management.helpers;

namespace Project_management.objects;

public class Employee
{
    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Department? Department { get; }
    public string MobilePhone { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public Employee(int id, string firstName, string lastName, Department? department, string mobilePhone)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Department = department;
        MobilePhone = mobilePhone;
    }

    public static List<Employee> GetAll()
    {
        var employees = new List<Employee>();
        using var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        using var command = new SQLiteCommand("SELECT * FROM Employee;", connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var firstName = reader.GetString(1);
            var lastName = reader.GetString(2);
            var department = Department.GetById(reader.GetInt32(3));
            var mobilePhone = reader.GetString(4);
            var employee = new Employee(id, firstName, lastName, department, mobilePhone);
            employees.Add(employee);
        }
        reader.Close();
        connection.Close();
        return employees;
    }

    public static Employee? GetById(int id)
    {
        using var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        using var command = new SQLiteCommand("SELECT * FROM Employee where id = " + id, connection);
        Employee? employee = null;
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var firstName = reader.GetString(1);
            var lastName = reader.GetString(2);
            var department = Department.GetById(reader.GetInt32(3));
            var mobilePhone = reader.GetString(4);
            employee = new Employee(id, firstName, lastName, department, mobilePhone);
        }
        reader.Close();
        connection.Close();
        return employee;
    }

    public static void UpdateOrCreate(string firstName, string lastName, Department department, string mobilePhone,
        int? id = null)
    {
        using var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        using var command = new SQLiteCommand(connection);
        if (id == null)
        {
            const string query = "INSERT INTO Employee (firstname, lastname, department_id, mobile_phone)" +
                                 " VALUES (@FirstName, @LastName, @DepartmentId, @MobilePhone);" +
                                 "SELECT last_insert_rowid();";
            command.CommandText = query;
            command.Parameters.AddWithValue("@FirstName", firstName);
            command.Parameters.AddWithValue("@LastName", lastName);
            command.Parameters.AddWithValue("@DepartmentId", department.Id);
            command.Parameters.AddWithValue("@MobilePhone", mobilePhone);
        }
        else
        {
            var query = "UPDATE Employee SET firstname = '" + firstName + "', lastname = '" + lastName + "', department_id = " + department.Id
                        + ", mobile_phone = '" + mobilePhone + "' WHERE id = " +
                        id + ";";
            command.CommandText = query;
        }

        command.ExecuteNonQuery();
        connection.Close();
    }

    public void Delete()
    {
        using var connection = DatabaseHelper.GetConnection().OpenAndReturn();
        using var command = new SQLiteCommand("DELETE FROM Employee WHERE id = @Id", connection);
        command.Parameters.AddWithValue("@Id", Id);
        command.ExecuteNonQuery();
        connection.Close();
    }
}