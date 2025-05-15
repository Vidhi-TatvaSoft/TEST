using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DAL.ViewModels;

public class JobApplicationViewModel
{
    public int ApplicationId { get; set; }
    public int JobId { get; set; }

    [Required(ErrorMessage = "Job Name is required.")]
    public string JobName { get; set; } = null!;

    [Required(ErrorMessage = "status is required.")]
    public string status { get; set; }

    public string Resume { get; set; }

    public IFormFile ResumePdf { get; set; }

    public long UserId { get; set; }
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Comapany Name is required.")]
    public string CompanyName { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    public string Location { get; set; }


}
