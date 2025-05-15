using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class JobApplication
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int JobAppliactionId {get; set ;}

    [ForeignKey("Jobs")]
    public int JobId{get ; set ;}
    public Jobs Jobs {get; set;}

    public string status {get ; set ; } 

    public string Resume {get ; set ; }

    // public DateTime CreatedAt {get; set;} = DateTime.Now;
    public bool IsDelete {get; set;}

    [ForeignKey("Users")]
    public long UserId { get; set; }
    public Users Users { get; set; } = null!;
    
}
