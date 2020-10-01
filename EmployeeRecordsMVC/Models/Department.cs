using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsMVC.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [MaxLength(30, ErrorMessage = "Entry is longer than 30 characters")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
