using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Core.Models
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string[] RoleNames { get; set; }
    }
}
