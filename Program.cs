
using JobApplicationTracker.Models;
using JobApplicationTracker.Services;

JobManager manager = new JobManager();

bool running = true;

while (running)
{
    Console.WriteLine("\n=== Job Application Tracker ===");
    Console.WriteLine("1. Add new application");
    Console.WriteLine("2. Show all applications");
    Console.WriteLine("3. Filter applications by status");
    Console.WriteLine("4. Sort applications by date");
    Console.WriteLine("5. Show statistics");
    Console.WriteLine("6. Update application status");
    Console.WriteLine("7. Delete application");
    Console.WriteLine("8. Show unanswered applications older than 14 days");
    Console.WriteLine("9. Exit");
    Console.Write("Choose an option: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            manager.AddJobFromInput();
            break;

        case "2":
            manager.ShowAll();
            break;

        case "3":
            Console.WriteLine(
                "Choose status: Applied, Interview, Offer, Rejected");

            string? input = Console.ReadLine();

            if (Enum.TryParse<ApplicationStatus>(
                input, true, out ApplicationStatus status) &&
                Enum.IsDefined(typeof(ApplicationStatus), status))
            {
                manager.ShowByStatus(status);
            }
            else
            {
                Console.WriteLine("Invalid status.");
            }
            break;

        case "4":
            manager.ShowSortedByDate();
            break;

        case "5":
            manager.ShowStatistics();
            break;

        case "6":
            manager.UpdateStatus();
            break;

        case "7":
            manager.RemoveJob();
            break;

        case "8":
            manager.ShowUnansweredOlderThan14Days();
            break;

        case "9":
            running = false;
            Console.WriteLine("Goodbye!");
            break;

        default:
            Console.WriteLine("Invalid choice. Please enter a number from 1 to 9.");
            break;
    }
}

