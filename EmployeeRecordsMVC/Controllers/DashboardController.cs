using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EmployeeRecordsMVC.DTOs;
using EmployeeRecordsMVC.Models;
using EmployeeRecordsMVC.Services;
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

            List<ReturnedUserDTO> returnedUsers = new List<ReturnedUserDTO>();

            //make GET requests and save user objects to collection
            int numberOfUsers = _userManager.Users.Count();
            await CreateUsersList(returnedUsers, numberOfUsers, pages);
            return View(returnedUsers);
        }

        [HttpGet]
        public IActionResult Charts()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Next()
        {
            ViewBag.LoggedIn = true;

            List<ReturnedUserDTO> returnedUsers = new List<ReturnedUserDTO>();

            //make GET requests and save user objects to collection
            int numberOfUsers = _userManager.Users.Count();
            if(pages <= numberOfUsers / 6)
            {
                pages++;
            }
            return RedirectToAction("Index", new { pages });
            //await CreateUsersList(returnedUsers, numberOfUsers, pages);
            //return View(returnedUsers);
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
            //List<ReturnedUserDTO> returnedUsers = new List<ReturnedUserDTO>();

            ////make GET requests and save user objects to collection
            //int numberOfUsers = _userManager.Users.Count();
            //await CreateUsersList(returnedUsers, numberOfUsers, pages);
            //return View(returnedUsers);
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
