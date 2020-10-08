using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EmployeeRecordsMVC.DTOs;
using EmployeeRecordsMVC.Models;
using EmployeeRecordsMVC.Services;
using EmployeeRecordsMVC.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EmployeeRecordsMVC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly UserManager<Employee> _userManager;
        public static int pages { get; set; } = 1;

        //create instance of returned users collection
        //public ICollection<ReturnedUserDTO> returnedUsers { get; set; }

        public DashboardController(ILogger<DashboardController> logger, UserManager<Employee> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pages = 1)
        {
            ViewBag.LoggedIn = true;
            ViewBag.IsDashboard = true;

            List<ReturnedUserDTO> returnedUsers = new List<ReturnedUserDTO>();

            //make GET requests and save user objects to collection
            int numberOfUsers = _userManager.Users.Count();
            await CreateUsersList(returnedUsers, numberOfUsers, pages);
            return View(returnedUsers);
        }

        [HttpGet]
        public async Task<IActionResult> Charts()
        {
            ViewBag.LoggedIn = true;
            ViewBag.IsCharts = true;

            List<ChartVIewModel> allUsers = new List<ChartVIewModel>();

            //make GET requests and save user objects to collection
            int numberOfUsers = _userManager.Users.Count();
            await CreateAllUsersList(allUsers, numberOfUsers, _userManager);
            return View(allUsers);
        }

        [HttpGet]
        public IActionResult Next()
        {
            ViewBag.LoggedIn = true;

            //make GET requests and save user objects to collection
            int numberOfUsers = _userManager.Users.Count();
            if(pages <= numberOfUsers / 6)
            {
                pages++;
            }
            return RedirectToAction("Index", new { pages });
        }

        [HttpGet]
        public IActionResult Previous()
        {
            ViewBag.LoggedIn = true;
            if(pages > 1)
            {
                pages--;
            }
            else if (pages == 0)
            {
                return RedirectToAction("Index", new { pages = 1 });
            }
            return RedirectToAction("Index", new { pages });
        }

        //method to make GET requests and save author objects to author repository list
        private static async Task<ICollection<ChartVIewModel>> CreateAllUsersList(ICollection<ChartVIewModel> allUsers, int numberOfUsers, UserManager<Employee> userManager)
        {
            int numberOfPages = (numberOfUsers / 6) + 1;
            int i = 1;
            while (i <= numberOfPages)
            {
                Task<PaginatedReturnedUsersDTO> page = GetUserRequest("https://localhost:44361/employee/allemployees?page=" + i);
                PaginatedReturnedUsersDTO apiPage = await page;

                foreach (var returnedUser in apiPage.ReturnedUsers)
                {
                    string role = "";
                    var email = returnedUser.Email;
                    var user = await userManager.FindByEmailAsync(email);
                    var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
                    if (isAdmin)
                    {
                        role = "Admin";
                    }
                    else
                    {
                        role = "User";
                    }
                    var model = new ChartVIewModel
                    {
                        FirstName = returnedUser.FirstName,
                        LastName = returnedUser.LastName,
                        Email = returnedUser.Email,
                        Photo = returnedUser.Photo,
                        Department = returnedUser.Department,
                        Role =  role
                    };
                    allUsers.Add(model);
                }
                i++;
            }
            return allUsers;
        }

        //method to make GET requests and save author objects to author repository list
        private static async Task<ICollection<ReturnedUserDTO>> CreateUsersList(ICollection<ReturnedUserDTO> returnedUsers, int numberOfUsers, int pages)
        {
            int numberOfPages = (numberOfUsers / 6) + 1;
            if (pages <= numberOfPages && pages > 0)
            {
                Task<PaginatedReturnedUsersDTO> page = GetUserRequest("https://localhost:44361/employee/allemployees?page=" + pages);
                PaginatedReturnedUsersDTO apiPage = await page;

                foreach (var user in apiPage.ReturnedUsers)
                {
                    returnedUsers.Add(user);
                }
            }
            return returnedUsers;
        }

        //method for making GET request
        async static Task<PaginatedReturnedUsersDTO> GetUserRequest(string url)
        {
            //create HttpClient instance
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            //deserialize string to get page object
            PaginatedReturnedUsersDTO page = JsonConvert.DeserializeObject<PaginatedReturnedUsersDTO>(content);
            return page;
        }


    }
}
