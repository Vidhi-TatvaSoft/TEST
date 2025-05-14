using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class Users
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string password {get ; set; } = null!;

    

    [ForeignKey("Roles")]
    public long RoleId { get ; set ; }
    public Roles Roles {get ; set;} 
}
