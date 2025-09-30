using SchoolResultSystem.Web.Models;
using System.Collections.Generic;
namespace SchoolResultSystem.Web.Areas.Principal.Models
{

    public class PrincipalDashboardModel
    {
        public string? SchoolName { get; set; }
        public List<UserModel>? Teachers { get; set; }
        public List<ClassModel>? Classes { get; set; }
    }
}
