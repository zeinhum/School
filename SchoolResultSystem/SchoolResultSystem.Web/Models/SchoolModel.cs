using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolResultSystem.Web.Models;

public class SchoolInfoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    public string? Name { get; set; } 
    public string? Address { get; set; } 
    public string? Phone { get; set; } 
    public string? Email { get; set; }
   
}