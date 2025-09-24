using SchoolResultSystem.Web.Models;
namespace SchoolResultSystem.Web.Models.Principal
{
    using System.Collections.Generic;

    public class PrincipalDashboardModel
    {
        public string SchoolName { get; set; } = "Your School";
        public string AdminName { get; set; } = "Admin";

        public List<UserModel>? Teachers { get; set; }
        public List<ClassModel>? Classes { get; set; }
    }
}
