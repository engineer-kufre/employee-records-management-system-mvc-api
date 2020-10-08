using EmployeeRecordsMVC.Models;
using EmployeeRecordsMVC.Services;
using EmployeeRecordsMVC.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsMVC.Controllers
{
    public class SignUpController : Controller
    {
        private readonly ILogger<SignUpController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SignUpController(ILogger<SignUpController> logger, AppDbContext context, UserManager<Employee> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Employee> signInManager, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var deptOptions = new SelectList(_context.Departments.ToList(), "DepartmentId", "DepartmentName");
            var roleOptions = new SelectList(_roleManager.Roles.ToList());
            var model = new SignUpViewModel
            {
                Department = deptOptions,
                Roles = roleOptions
            };

            if(_signInManager.IsSignedIn(User) && User.IsInRole("Admin")){
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SignUpViewModel model)
        {
            var deptOptions = new SelectList(_context.Departments.ToList(), "DepartmentId", "DepartmentName");
            var roleOptions = new SelectList(_roleManager.Roles.ToList());
            model.Department = deptOptions;
            model.Roles = roleOptions;

            if(model.SelectedRole == null)
            {
                model.SelectedRole = "User";
            }

            if (ModelState.IsValid)
            {
                string PhotoLink;

                if (model.Photo != null)
                {
                    var folderLink = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    var photoName = Guid.NewGuid() + "_" + model.Photo.FileName;

                    PhotoLink = Path.Combine(folderLink, photoName);

                    using (var file = new FileStream(PhotoLink, FileMode.Create))
                    {
                        model.Photo.CopyTo(file);
                    }
                }
                else
                {
                    PhotoLink = "~/images/Avatar1.jpg";
                }

                var employee = new Employee
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    DepartmentID = int.Parse(model.SelectedDepartment),
                    Photo = PhotoLink
                };

                //add employee to database
                var result = await _userManager.CreateAsync(employee, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(employee, model.SelectedRole);

                    return RedirectToAction("Index", "SignIn");
                }
            }

            return View("Index", model);
        }

    }
}
