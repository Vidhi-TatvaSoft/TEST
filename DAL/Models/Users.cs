using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class Users
{
    [Key]
    public long UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public string password {get ; set; } = null!;

    

    [ForeignKey("Roles")]
    public long RoleId { get ; set ; }
    public Roles Roles {get ; set;}
}
