using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SoftServe.FindLeader.Test
{
    class Test
    {
        static void Main(string[] args)
        {
            Dictionary<string, Employee> allEmployees = new Dictionary<string, Employee>();
            string leader = string.Empty;
            string firstEmployee = string.Empty;
            string secondEmployee = string.Empty;
            Console.Write("Enter first employee: ");
            firstEmployee = Console.ReadLine();
            Console.Write("Enter second employee: ");
            secondEmployee = Console.ReadLine();

            bool hasNext = true;

            while (hasNext)
            {
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
                    SplitInput(input, out firstMatch, out secondMatch);
                    InsertLeader(allEmployees, firstMatch, secondMatch);
                    InsertEmployee(allEmployees, firstMatch, secondMatch);
                }
            }

            GiveIndexes(allEmployees);
            leader = SearchLeader(allEmployees, firstEmployee, secondEmployee, leader);
            PrintResult(leader, firstEmployee, secondEmployee);
        }

        private static void SplitInput(string input, out string firstMatch, out string secondMatch)
        {
            Regex reg = new Regex(@"(\w*)\W+(\w*)", RegexOptions.IgnoreCase);
            Match m = reg.Match(input);
            firstMatch = Convert.ToString(m.Groups[1]);
            secondMatch = Convert.ToString(m.Groups[2]);
        }
        private static void InsertLeader(Dictionary<string, Employee> allEmployees, string firstMatch, string secondMatch)
        {
            if (!allEmployees.ContainsKey(firstMatch))
            {
                Employee employeeChild = null;
                if (!allEmployees.ContainsKey(secondMatch))
                {
                    employeeChild = new Employee(secondMatch);
                    allEmployees[secondMatch] = employeeChild;
                }
                else
                {
                    employeeChild = allEmployees[secondMatch];
                }
                Employee employee = new Employee(firstMatch, employeeChild);
                allEmployees[firstMatch] = employee;
            }
            else
            {
                Employee currentParent = allEmployees[firstMatch];
                Employee employeeChild = null;
                if (allEmployees.ContainsKey(secondMatch))
                {
                    employeeChild = allEmployees[secondMatch];
                }
                else
                {
                    employeeChild = new Employee(secondMatch);
                }
                currentParent.addEmployees(employeeChild);
                allEmployees[firstMatch] = currentParent;
            }
        }
        private static void InsertEmployee(Dictionary<string, Employee> allEmployees, string firstMatch, string secondMatch)
        {
            if (!allEmployees.ContainsKey(secondMatch))
            {
                Employee employee = new Employee(secondMatch);
                employee.Parent = firstMatch;
                employee.Position++;
                allEmployees[secondMatch] = employee;
            }
            else
            {
                Employee employee = allEmployees[secondMatch];
                employee.Parent = firstMatch;
            }
        }
        private static void GiveIndexes(Dictionary<string, Employee> allEmployees)
        {
            int allEmployeesCount = allEmployees.Count;
            for (int i = 0; i < allEmployeesCount; i++)
            {
                foreach (var parent in allEmployees)
                {
                    foreach (var child in allEmployees[parent.Key].Employees)
                    {
                        allEmployees[child.Key].Position = parent.Value.Position + 1;
                    }
                }
            }
        }
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
        private static void PrintResult(string leader, string firstEmployee, string secondEmployee)
        {
            Console.WriteLine("The employee {0} and employee {1} leader is {2}.", firstEmployee, secondEmployee, leader);
        }
    }
}

/*
* Check the result with one leader with two employees.
* 
* 
*                                                                         *Leader
* 
*                                                                *e1         |           *e2
*                                                           
* 
e1
e2
Leader-e1
Leader-e2
End
* 
* 
* Check the result with bottom left branch and bottom right branch. 
* 
*                                                                         *Leader
* 
*                                                                *e1         |           *e2
*                                                           
*                                                          *e3    |   *e4    |     *e5     |      *e6
*                                                          
*                                                      *e7 | *e8 | *e9 | *e10 | *e11 | *e12 |  *e13 | *e14
* 
e5
e10
e1-e3
e4-e10
e3-e7
e1-e4
Leader-e1
e2-e5
e6-e13
e2-e6
Leader-e2
e3-e8
e4-e9
e5-e11
e6-e14
e5-e12
End
* 
* 
* Check the result with top left branch and bottom right branch.
* 
* 
*                                                                         *Leader
* 
*                                                                *e1         |           *e2
*                                                           
*                                                          *e3    |   *e4    |     *e5     |      *e6
*                                                          
*                                                      *e7 | *e8 | *e9 | *e10 | *e11 | *e12 |  *e13 | *e14
* 
e1
e14
e1-e3
e4-e10
e3-e7
e1-e4
Leader-e1
e2-e5
e6-e13
e2-e6
Leader-e2
e3-e8
e4-e9
e5-e11
e6-e14
e5-e12
End
*
*
* Check the result with bottom left branch and top right branch from main left branch
* 
* 
*                                                                         *Leader
* 
*                                                                *e1         |           *e2
*                                                           
*                                                          *e3    |   *e4    |     *e5     |      *e6
*                                                          
*                                                      *e7 | *e8 | *e9 | *e10 | *e11 | *e12 |  *e13 | *e14
*                                                      
*                                                  *e15 | *e16     
*                                                      
* 
* 
e15
e4
e1-e3
e4-e10
e3-e7
e1-e4
Leader-e1
e2-e5
e6-e13
e2-e6
Leader-e2
e3-e8
e4-e9
e5-e11
e6-e14
e5-e12
e7-e15
e7-e16
e15-17
e15-e18
End
*/