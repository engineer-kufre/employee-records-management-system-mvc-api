using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeRecordsMVC.DTOs;
using EmployeeRecordsMVC.Models;
using EmployeeRecordsMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeRecordsMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<Employee> _userManager;

        public UserController(ILogger<UserController> logger, UserManager<Employee> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            ViewBag.LoggedIn = true;

            var user = await _userManager.FindByIdAsync(id);
            var model = new ReturnedUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Photo = user.Photo
            };

            return View(model);
        }
    }
}
