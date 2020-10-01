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
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Required Field")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [StringLength(15, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
