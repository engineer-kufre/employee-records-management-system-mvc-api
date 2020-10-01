using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsMVC.DTOs
{
    public class PaginatedReturnedUsersDTO
    {
        //model for a Paginated List of Returned User Data Transfer Objects
        public string CurrentPage { get; set; }
        public List<ReturnedUserDTO> ReturnedUsers { get; set; }
    }
}
