
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services;

public class JobManager
{
    // Stores all job applications in memory.
    public List<JobApplication> Applications { get; } = new();

    // Adds a new job application.
    public void AddJob(JobApplication application)
    {
        Applications.Add(application);
        Console.WriteLine("Job application added successfully!");
    }

    // Reads input and creates a new application.
    public void AddJobFromInput()
    {
        Console.Write("Company name: ");
        string company = Console.ReadLine() ?? "";

        Console.Write("Position title: ");
        string position = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(company) ||
            string.IsNullOrWhiteSpace(position))
        {
            Console.WriteLine("Company and position are required.");
            return;
        }

        Console.Write("Salary expectation (SEK): ");

        if (!int.TryParse(Console.ReadLine(), out int salary) || salary < 0)
        {
            Console.WriteLine("Invalid salary.");
            return;
        }

        var application = new JobApplication
        {
            CompanyName = company,
            PositionTitle = position,
            SalaryExpectation = salary,
            ApplicationDate = DateTime.Today,
            Status = ApplicationStatus.Applied,
            ResponseDate = null
        };

        AddJob(application);
    }

    
    // Displays all applications with colored statuses.
    public void ShowAll()
    {
        if (Applications.Count == 0)
        {
            Console.WriteLine("No job applications found.");
            return;
        }

        foreach (var application in Applications)
        {
            if (application.Status == ApplicationStatus.Offer)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (application.Status == ApplicationStatus.Rejected)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ResetColor();
            }

            Console.WriteLine(application.GetSummary());
            Console.ResetColor();
        }
    }


    // Filters applications by status using LINQ.
    public void ShowByStatus(ApplicationStatus status)
    {
        var filteredApplications = Applications
            .Where(a => a.Status == status)
            .ToList();

        if (filteredApplications.Count == 0)
        {
            Console.WriteLine("No applications found for this status.");
            return;
        }

        foreach (var application in filteredApplications)
        {
            Console.WriteLine(application.GetSummary());
        }
    }

    // Sorts applications by application date.
    public void ShowSortedByDate()
    {
        var orderedApplications = Applications
            .OrderBy(a => a.ApplicationDate)
            .ToList();

        if (orderedApplications.Count == 0)
        {
            Console.WriteLine("No job applications found.");
            return;
        }

        foreach (var application in orderedApplications)
        {
            Console.WriteLine(application.GetSummary());
        }
    }

    // Shows application statistics using LINQ.
    public void ShowStatistics()
    {
        Console.WriteLine("\n=== Application Statistics ===");
        Console.WriteLine($"Total applications: {Applications.Count()}");

        foreach (ApplicationStatus status in Enum.GetValues<ApplicationStatus>())
        {
            int count = Applications.Count(a => a.Status == status);
            Console.WriteLine($"{status}: {count}");
        }

        var answeredApplications = Applications
            .Where(a => a.ResponseDate.HasValue)
            .ToList();

        if (answeredApplications.Count > 0)
        {
            double averageDays = answeredApplications
                .Average(a =>
                    (a.ResponseDate!.Value - a.ApplicationDate).TotalDays);

            Console.WriteLine($"Average response time: {averageDays:F1} days");
        }
        else
        {
            Console.WriteLine("No applications have received a response yet.");
        }
    }

    // Updates the status of an application.
    public void UpdateStatus()
    {
        if (Applications.Count == 0)
        {
            Console.WriteLine("No applications to update.");
            return;
        }

        for (int i = 0; i < Applications.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Applications[i].GetSummary()}");
        }

        Console.Write("Enter application number: ");

        if (!int.TryParse(Console.ReadLine(), out int number) ||
            number < 1 || number > Applications.Count)
        {
            Console.WriteLine("Invalid application number.");
            return;
        }

        JobApplication application = Applications[number - 1];

        Console.WriteLine("Choose new status: Applied, Interview, Offer, Rejected");
        string? input = Console.ReadLine();

        if (!Enum.TryParse<ApplicationStatus>(input, true, out var newStatus) ||
            !Enum.IsDefined(typeof(ApplicationStatus), newStatus))
        {
            Console.WriteLine("Invalid status.");
            return;
        }

        application.Status = newStatus;

        if (newStatus == ApplicationStatus.Applied)
        {
            application.ResponseDate = null;
        }
        else
        {
            application.ResponseDate ??= DateTime.Today;
        }

        Console.WriteLine("Application status updated successfully!");
    }

    // Deletes an application by its number.
    public void RemoveJob()
    {
        if (Applications.Count == 0)
        {
            Console.WriteLine("No applications to delete.");
            return;
        }

        for (int i = 0; i < Applications.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Applications[i].GetSummary()}");
        }

        Console.Write("Enter application number to delete: ");

        if (!int.TryParse(Console.ReadLine(), out int number) ||
            number < 1 || number > Applications.Count)
        {
            Console.WriteLine("Invalid application number.");
            return;
        }

        Applications.RemoveAt(number - 1);

        Console.WriteLine("Application deleted successfully!");
    }


    // Shows unanswered applications older than 14 days.
    public void ShowUnansweredOlderThan14Days()
    {
        var unanswered = Applications
            .Where(a => a.ResponseDate == null &&
                        a.GetDaysSinceApplied() > 14)
            .OrderBy(a => a.ApplicationDate)
            .ToList();

        if (unanswered.Count == 0)
        {
            Console.WriteLine(
                "No unanswered applications older than 14 days.");
            return;
        }

        foreach (var application in unanswered)
        {
            Console.WriteLine(application.GetSummary());
            Console.WriteLine(
                $"Days since applied: {application.GetDaysSinceApplied()}");
        }
    }
}

