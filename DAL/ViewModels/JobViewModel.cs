using System.ComponentModel.DataAnnotations;

namespace DAL.ViewModels;

public class JobViewModel
{
    public int JobId { get; set; }

    [Required(ErrorMessage = "Job Name is required.")]
    public string JobName { get; set; }

    public string JobDescription { get; set; } = null!;

    [Required(ErrorMessage = "Comapany Name is required.")]
    public string CompanyName { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    public string Location { get; set; }

    public int NoOfAplicants { get; set; }

    [Required(ErrorMessage = "status is required.")]
    public string status { get; set; }
}
