using SchoolResultSystem.Web.Models;

namespace SchoolResultSystem.Web.Areas.Principal.Models
{
    public class AdSt : StudentModel
    {
        public int ClassId { get; set; }
    }

    public class AdmiForm
    {
        public List<AdSt> AdmissionForm { get; set; } = new List<AdSt>();
    }
}
