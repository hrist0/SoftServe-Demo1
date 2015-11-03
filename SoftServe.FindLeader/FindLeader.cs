using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FindLeader
{
    class FindLeader
    {
        static void Main(string[] args)
        {
            // Keep all created employees
            Dictionary<string, Employee> allEmployees = new Dictionary<string, Employee>();
            // Initialize the output variable 
            string leader = string.Empty;
            // Initialize searching employee's leader
            string firstEmployee = string.Empty;
            string secondEmployee = string.Empty;
            Console.Write("Enter first employee: ");
            firstEmployee = Console.ReadLine();
            Console.Write("Enter second employee: ");
            secondEmployee = Console.ReadLine();

            bool hasNext = true;

            while (hasNext)
            {
                // Read current input
                string readInput = Console.ReadLine();
                if (readInput == "End")
                {
                    hasNext = false;
                    break;
                }
                else
                {
                    string input = readInput;
                    string firstMatch;
                    string secondMatch;
                    //Split current input
                    SplitInput(input, out firstMatch, out secondMatch);
                    // Step 1 check if the first name already exists in allEmployees
                    InsertLeader(allEmployees, firstMatch, secondMatch);
                    // Step 2 check if the second name doesn't exists and create new Employee
                    InsertEmployee(allEmployees, firstMatch, secondMatch);
                }
            }

            // Step 3 give all employees positions
            GiveIndexes(allEmployees);
            // Step 4 find the employees leader
            leader = SearchLeader(allEmployees, firstEmployee, secondEmployee, leader);
            // Step 5 print result
            PrintResult(leader, firstEmployee, secondEmployee);
        }

        //Methods

        // Split current input
        private static void SplitInput(string input, out string firstMatch, out string secondMatch)
        {
            Regex reg = new Regex(@"(\w*)\W+(\w*)", RegexOptions.IgnoreCase);
            Match m = reg.Match(input);
            firstMatch = Convert.ToString(m.Groups[1]);
            secondMatch = Convert.ToString(m.Groups[2]);
        }
        // Check and insert elements in employee Dictionary
        private static void InsertLeader(Dictionary<string, Employee> allEmployees, string firstMatch, string secondMatch)
        {
            if (!allEmployees.ContainsKey(firstMatch))
            {
                // Step 1.1 If the second child doesn't exists we create it in allEmployees
                Employee employeeChild = null;
                if (!allEmployees.ContainsKey(secondMatch))
                {
                    employeeChild = new Employee(secondMatch);
                    allEmployees[secondMatch] = employeeChild;
                }
                // Step 1.2 If the second child exists we pull it from allEmployes
                else
                {
                    employeeChild = allEmployees[secondMatch];
                }
                // Step 1.3 Create parent Employee and add his child
                Employee employee = new Employee(firstMatch, employeeChild);
                allEmployees[firstMatch] = employee;
            }
            else
            {
                // Step 1.4 If the firsts child exist we pull it from allEmployees
                Employee currentParent = allEmployees[firstMatch];
                Employee employeeChild = null;
                //Check If employee child exists in allEmployees we pull it
                if (allEmployees.ContainsKey(secondMatch))
                {
                    employeeChild = allEmployees[secondMatch];
                }
                // If the second child doesn't exists we create it 
                else
                {
                    employeeChild = new Employee(secondMatch);
                }
                // Add child to parent and update parent to allEmployees
                currentParent.addEmployees(employeeChild);
                allEmployees[firstMatch] = currentParent;
            }
        }
        //Check, insert, add parents and increase position in employee Dictionary
        private static void InsertEmployee(Dictionary<string, Employee> allEmployees, string firstMatch, string secondMatch)
        {
            if (!allEmployees.ContainsKey(secondMatch))
            {
                // 2.1 If second child do not exists we create it
                Employee employee = new Employee(secondMatch);
                // 2.2 We create parent to that child
                employee.Parent = firstMatch;
                // 2.3 We asume that he is child of someone else and increase his position
                employee.Position++;
                // 2.4 Add it to allEmployees
                allEmployees[secondMatch] = employee;
            }
            // Step 2.5 If child exists we pull it from allEmployee and we set his parent
            else
            {
                Employee employee = allEmployees[secondMatch];
                employee.Parent = firstMatch;
            }
        }
        // Give indexes to all employees 
        private static void GiveIndexes(Dictionary<string, Employee> allEmployees)
        {
            int allEmployeesCount = allEmployees.Count;
            // Step 3 to be sure we'll set all positions of all employees we need to go thru the count of all employees
            for (int i = 0; i < allEmployeesCount; i++)
            {
                // Step 3.1 pull all elements from AllEmployees
                foreach (var parent in allEmployees)
                {
                    // Step 3.2 pull all employees from parent and set the position of every employee from parent which is equal to parent position + 1
                    foreach (var child in allEmployees[parent.Key].Employees)
                    {
                        allEmployees[child.Key].Position = parent.Value.Position + 1;
                    }
                }
            }
        }
        // Search for employees leader
        private static string SearchLeader(Dictionary<string, Employee> allEmployees, string firstEmployee, string secondEmployee, string leader)
        {
            Employee employeeOne = allEmployees[firstEmployee];
            Employee employeeTwo = allEmployees[secondEmployee];
            bool toContinue = true;
            while (toContinue)
            {
                if (employeeOne.Position > employeeTwo.Position)
                {
                    employeeOne = allEmployees[employeeOne.Parent];
                }
                else if (employeeOne.Position < employeeTwo.Position)
                {
                    employeeTwo = allEmployees[employeeTwo.Parent];
                }
                else if (employeeOne.Position == employeeTwo.Position)
                {
                    if (employeeOne.Parent.Equals(employeeTwo.Parent))
                    {
                        leader = employeeOne.Parent;
                        break;
                    }
                    else
                    {
                        employeeOne = allEmployees[employeeOne.Parent];
                        employeeTwo = allEmployees[employeeTwo.Parent];
                    }
                }
            }
            return leader;
        }
        // Print result
        private static void PrintResult(string leader, string firstEmployee, string secondEmployee)
        {
            Console.WriteLine("The employee {0} and employee {1} leader is {2}.", firstEmployee, secondEmployee, leader);
        }
    }
}