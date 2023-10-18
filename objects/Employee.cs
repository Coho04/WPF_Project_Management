namespace Project_management.objects;

public class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public Department Department { get; set; }
    
    public string MobilePhone { get; set; }


    public Employee(string firstName, string lastName, Department department, string mobilePhone)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Department = department;
        this.MobilePhone = mobilePhone;
    }
}