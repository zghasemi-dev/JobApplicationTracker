
namespace JobApplicationTracker.Models;

public class JobApplication
{
    public string CompanyName { get; set; } = "";
    public string PositionTitle { get; set; } = "";

    public ApplicationStatus Status { get; set; }

    public DateTime ApplicationDate { get; set; }
    public DateTime? ResponseDate { get; set; }

    public int SalaryExpectation { get; set; }

    // Returns the number of days since the application was sent.
    public int GetDaysSinceApplied()
    {
        return (DateTime.Today - ApplicationDate.Date).Days;
    }

    // Returns a short summary of the application.
    public string GetSummary()
    {
        return $"{CompanyName} - {PositionTitle} | " +
               $"Status: {Status} | " +
               $"Applied: {ApplicationDate:yyyy-MM-dd} | " +
               $"Salary: {SalaryExpectation} SEK";
    }
}

