using Microsoft.AspNetCore.Http;

namespace DAL.ViewModels;

public class JobApplicationViewModel
{
    public int ApplicationId { get; set; }
    public int JobId { get; set; }

    public string JobName { get; set; } = null!;

    public string status { get; set; }

    public string Resume { get; set; }

    public IFormFile ResumePdf {get;set;}

    public long UserId { get; set; }
    public string UserName {get; set; } = null!;

}
