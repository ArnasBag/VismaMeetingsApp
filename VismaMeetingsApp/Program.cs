using VismaMeetingsApp;
using VismaMeetingsApp.Models;
using VismaMeetingsApp.Repositories;
using VismaMeetingsApp.Services;

var repo = new MeetingRepository();

Console.WriteLine("To use this application, you must login first!\n");
var firstName = IOUtils.PromptUser<string>("First name: ");
var lastName = IOUtils.PromptUser<string>("Last name: ");

var loggedInPerson = new Person
{
    FirstName = firstName,
    LastName = lastName
};

var service = new MeetingService(repo, loggedInPerson);

IOUtils.PrintWelcomeScreen(loggedInPerson);
bool running = true;

while (running)
{
    var input = IOUtils.PromptUser<string>("Type in the operation: ");

    switch (input.ToLower())
    {
        case "create":
            IOUtils.DisplayAddMeetingCommandUI(service, loggedInPerson);
            break;
        case "delete":
            IOUtils.DisplayDeleteMeetingCommandUI(service);
            break;
        case "list":
            IOUtils.DisplayListMeetingsCommandUI(repo);
            break;
        case "add person":
            IOUtils.DisplayAddPersonCommandUI(service);
            break;
        case "remove person":
            IOUtils.DisplayRemovePersonCommandUI(service);
            break;
        case "quit":
            running = false;
            break;
        default:
            Console.WriteLine("Unrecognized command. Try again!");
            break;
    }
}



