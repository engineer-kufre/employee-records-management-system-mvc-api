using EmployeeRecordsMVC.DTOs;
using EmployeeRecordsMVC.Models;
using EmployeeRecordsMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace EmployeeRecordsMVC.API.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly UserManager<Employee> _userManager;

        public EmployeeController(ILogger<EmployeeController> logger, UserManager<Employee> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        //method to fetch all registered users
        // /employee/allemployees
        [AllowAnonymous]
        [HttpGet("AllEmployees")]
        public IActionResult AllEmployees(int page = 1)
        {
            int zeroIndexedPage = page;
            int start = (zeroIndexedPage - 1) * 6;

            //get all employees from AspNetUsers table and include their departments
            var allEmployees = _userManager.Users
                                                  .Include(e => e.Department)
                                                  .Skip(start)
                                                  .Take(6)
                                                  .Select(e => new ReturnedUserDTO
                                                  {
                                                      FirstName = e.FirstName,
                                                      LastName = e.LastName,
                                                      Email = e.Email,
                                                      Photo = e.Photo,
                                                      Department = e.Department.DepartmentName
                                                  }).ToList();

            var result = new PaginatedReturnedUsersDTO
            {
                CurrentPage = $"Page {page}",
                ReturnedUsers = allEmployees,
            };

            return Ok(result);
        }
    }
}
