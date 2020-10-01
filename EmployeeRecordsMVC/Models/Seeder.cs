using EmployeeRecordsMVC.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsMVC.Models
{
    //feeds the database with demo data for testing and staging
    public class Seeder
    {
        public static async Task SeederAsync(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<Employee> userManager)
        {
            //ensure that a database exists 
            context.Database.EnsureCreated();

            //if no departments exist in the database
            var allDepartments = context.Departments.ToList();
            if (allDepartments.Count == 0)
            {
                //create list of departments
                allDepartments = new List<Department>
                {
                    new Department { DepartmentName = "HR" },
                    new Department { DepartmentName = "Accounting" }
                };
                context.Departments.AddRange(allDepartments);
                context.SaveChanges();
            }

            //if no roles exist in the database
            if (!roleManager.Roles.Any())
            {
                //create list of roles
                var allRoles = new List<IdentityRole>
                {
                    new IdentityRole("Admin"),
                    new IdentityRole("User")
                };

                //add role(s) to the database
                foreach (var role in allRoles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            if (!userManager.Users.Any())
            {
                //create list of employees
                var allEmployees = new List<Employee>
                {
                    new Employee{FirstName="Segun", LastName="Adaramaja", Email="seguna@gmail.com", UserName="seguna@gmail.com", DepartmentID=1, Photo="~/images/Avatar.png"},
                    new Employee{FirstName="Seun", LastName="Oyetoyan", Email="seuno@gmail.com", UserName="seuno@gmail.com", PhoneNumber="07057893783", DepartmentID=2, Photo="~/images/Avatar.png"},
                    new Employee{FirstName="Micheal", LastName="Nwosu", Email="miken@gmail.com", UserName="miken@gmail.com", PhoneNumber="08036754890", DepartmentID=2, Photo="~/images/Avatar.png"}
                };

                int counter = 0;
                //add employee(s) to the database
                foreach (var employee in allEmployees)
                {
                    var result = await userManager.CreateAsync(employee, "P@$$word1");

                    //if employee successfully added
                    if (result.Succeeded)
                    {
                        //add employee role
                        if (counter == 0)
                        {
                            await userManager.AddToRoleAsync(employee, "Admin");
                        }
                        else
                        {
                            await userManager.AddToRoleAsync(employee, "User");
                        }
                    }
                    counter++;
                }
            }
        }
    }
}
