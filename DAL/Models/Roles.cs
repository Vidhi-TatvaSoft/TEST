using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public class Roles
{
    [Key]
    public long RoleId { get; set; }

    public string RoleName { get; set; } = null!;
}
