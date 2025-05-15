namespace DAL.ViewModels;

public class JobViewModel
{
    public int JobId {get; set;}

    public string JobName {get; set;}

    public string JobDescription {get; set;} = null!;

    public string CompanyName {get; set;}

    public string Location {get; set;}

    public int NoOfAplicants {get; set;}

    public string status {get;set;}
}
