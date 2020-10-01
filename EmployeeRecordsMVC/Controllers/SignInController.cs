using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmployeeRecordsMVC.Models;
using EmployeeRecordsMVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using EmployeeRecordsMVC.ViewModels;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace EmployeeRecordsMVC.Controllers
{
    public class SignInController : Controller
    {
        private readonly ILogger<SignUpController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly IConfiguration _configuration;

        public SignInController(ILogger<SignUpController> logger, AppDbContext context, UserManager<Employee> userManager, SignInManager<Employee> signInManager, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                //search database for an employee with inputted email
                var matchEmail = await _userManager.FindByEmailAsync(model.Email);

                //check that returned user's password matches that inputted
                var matchPassword = await _userManager.CheckPasswordAsync(matchEmail, model.Password);

                //check that an employee with that email and password exists in the database
                if (matchEmail == null || !matchPassword)
                {
                    ViewBag.NotValid = "Invalid credentials!";
                    return View("Index", model);
                }
                else
                {
                    //sign in
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: model.RememberMe, false);

                    //create claims array
                    var claims = new[]
                    {
                        new Claim("Email", model.Email),
                        new Claim(ClaimTypes.NameIdentifier, matchEmail.Id)
                    };

                    //obtain JWT secret key to encrypt token
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

                    //generate signin credentials
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                    //create security token descriptor(builds the token)
                    var securityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.Now.AddDays(1), //how many days before token expires
                        SigningCredentials = creds
                    };

                    //build token handler
                    var tokenHandler = new JwtSecurityTokenHandler();

                    //create token
                    var token = tokenHandler.CreateToken(securityTokenDescriptor);

                    var tokenAsString = tokenHandler.WriteToken(token);

                    if (result.Succeeded)
                    {
                        if (await _userManager.IsInRoleAsync(matchEmail, "Admin"))
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            return RedirectToAction("Index", "User", new { email = matchEmail.Email });
                        }
                    }
                }
            }

            return View("Index", model);
        }
    }
}
