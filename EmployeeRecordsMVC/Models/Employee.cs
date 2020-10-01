using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeRecordsMVC.Models
{
    public class Employee : IdentityUser
    {
        [Required(ErrorMessage = "Required Field")]
        [MaxLength(30, ErrorMessage = "Entry is longer than 30 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [MaxLength(30, ErrorMessage = "Entry is longer than 30 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Photo { get; set; }

        public DateTime EmployedOn { get; set; } = DateTime.Now;

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }

        public Department Department { get; set; }
    }
}
