using EmployeeRecordsMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsMVC.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Required Field")]
        [MaxLength(30, ErrorMessage = "Entry is longer than 30 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [MaxLength(30, ErrorMessage = "Entry is longer than 30 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        //[Required]
        //public string DepartmentName { get; set; }

        //[Required]
        //public string Role { get; set; }

        public SelectList Department { get; set; }
        public string SelectedDepartment { get; set; }
        public SelectList Roles { get; set; }
        public string SelectedRole { get; set; }

        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(15, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
