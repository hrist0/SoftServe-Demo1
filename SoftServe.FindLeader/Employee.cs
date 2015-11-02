using System.Collections.Generic;

public class Employee
{
    // Private member-variables
    private string parent;
    private string name;
    private Dictionary<string, Employee> employees = new Dictionary<string, Employee>();

    // We use it to check position of employees 
    public int Position { get; set; }
    // We use it to change positon of employees
    public string Parent { get; set; }
    // The name of employee
    public string Name
    {
        get
        {
            return this.name;
        }
        set
        {
            this.Name = value;
        }
    }
    // Dictionary that contains employees
    public Dictionary<string, Employee> Employees
    {
        get
        {
            return this.employees;
        }
        set
        {
            this.employees = value;
        }
    }
    // We use it to create an Employee
    public Employee(string name)
    {
        this.name = name;
    }
    // We use it to create Employee with child elements
    public Employee(string name, Employee employee)
    {
        this.name = name;
        if (employee.name != "")
        {
            this.parent = name;
            this.addEmployees(employee);
        }
    }
    // We use it to add children to another Employee
    public void addEmployees(Employee employee)
    {
        this.employees[employee.name] = employee;
    }
}