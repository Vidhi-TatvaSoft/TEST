using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class Jobs
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int JobId {get; set;}

    public string JobName {get; set;}

    public string JobDescription {get; set;} = null!;
 
    public string CompanyName {get; set;}

    public string Location {get; set;}

    public int NoOfAplicants {get; set;} 
    // public DateTime CreatedAt {get; set;} = DateTime.Now;

    public string Status {get;set;}

    public bool IsDelete {get; set;}

    public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

}
