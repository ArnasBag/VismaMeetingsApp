using System.ComponentModel;
using VismaMeetingsApp.Models;
using VismaMeetingsApp.Models.Enums;
using VismaMeetingsApp.Repositories;
using VismaMeetingsApp.Services;

namespace VismaMeetingsApp
{
    public static class IOUtils
    {
        public static void PrintWelcomeScreen(Person loggedInPerson)
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine($"Welcome to the Visma meeting management application, {loggedInPerson.FirstName}!");
            Console.WriteLine("Choose one out of the following operations:");
            Console.WriteLine("create - create a meeting with specified data\n" +
                "delete - delete meeting by specified meeting name\n" +
                "list - view all meetings\n" +
                "add person - add attendee to a specified meeting\n" +
                "remove person - remove attendee from a specified meeting\n" +
                "quit - quit this program");
            Console.WriteLine("-------------------------------------------------------------------");
        }
        private static void PrintFiltered(IEnumerable<Meeting> filtered)
        {
            if(filtered.Count() == 0)
            {
                Console.WriteLine("No meetings found matching specified filters!");
            }
            else
            {
                foreach (var meeting in filtered)
                {
                    Console.WriteLine(meeting);
                }
            }
        }
        public static T PromptUser<T>(string message)
        {
            var typeDescriptor = TypeDescriptor.GetConverter(typeof(T));
            Console.Write(message);
            var value = Console.ReadLine();

            while (!typeDescriptor.IsValid(value))
            {
                Console.WriteLine("Invalid input. Please try again!");
                Console.Write(message);
                value = Console.ReadLine();
            }

            return (T)typeDescriptor.ConvertFromString(value);
        }
        public static void DisplayAddMeetingCommandUI(MeetingService meetingService, Person loggedInPerson)
        {
            var name = PromptUser<string>("Name of the meeting: ");
            var desc = PromptUser<string>("Description of the meeting: ");
            var category = PromptUser<MeetingCategory>("Category of the meeting (CodeMonkey, Hub, Short or TeamBuilding): ");
            var type = PromptUser<MeetingType>("Type of the meeting (Live or InPerson): ");
            var startDate = PromptUser<DateTime>("Start time of the meeting (YYYY-MM-dd HH:mm): ");
            var endDate = PromptUser<DateTime>("End time of the meeting (YYYY-MM-dd HH:mm): ");

            var meeting = new Meeting
            {
                Name = name,
                Description = desc,
                ResponsiblePerson = loggedInPerson,
                Category = category,
                Type = type,
                StartDate = startDate,
                EndDate = endDate,
            };

            meetingService.AddMeeting(meeting);
        }
        public static void DisplayDeleteMeetingCommandUI(MeetingService meetingService)
        {
            var meetingToDeleteName = PromptUser<string>("Name of the meeting you want to delete: ");
            
            meetingService.DeleteMeeting(meetingToDeleteName);
        }
        public static void DisplayListMeetingsCommandUI(MeetingRepository repository)
        {
            var meetings = repository.ReadAllMeetings();
            Console.WriteLine("Choose one of the following filters:\n" +
                "0 - no filter\n" +
                "1 - filter by description\n" +
                "2 - filter by responsible person\n" +
                "3 - filter by category\n" +
                "4 - filter by type\n" +
                "5 - filter by dates\n" +
                "6 - filter by number of attendees\n");
            var input = PromptUser<string>("Type in the corresponding number: ");

            switch(input){
                case "1":
                    var keyword = PromptUser<string>("Type in filter keyword: ");
                    PrintFiltered(meetings.Where(m => m.Description.Contains(keyword)));
                    break;
                case "2":
                    var firstName = PromptUser<string>("Type in first name: ");
                    var lastName = PromptUser<string>("Type in last name: ");
                    PrintFiltered(meetings.Where(m => m.ResponsiblePerson == new Person { FirstName = firstName, LastName = lastName }));
                    break;
                case "3":
                    var category = PromptUser<MeetingCategory>("Type in category (CodeMonkey, Hub, Short or TeamBuilding): ");
                    PrintFiltered(meetings.Where(m => m.Category == category));
                    break;
                case "4":
                    var type = PromptUser<MeetingType>("Type in meeting type (Live or InPerson): ");
                    PrintFiltered(meetings.Where(m => m.Type == type));
                    break;
                case "5":
                    var startDate = PromptUser<DateTime>("Type in meeting start date (YYYY-MM-dd HH:mm): ");
                    var endDate = PromptUser<DateTime>("Type in meeting end date (YYYY-MM-dd HH:mm) (optional): ");

                    if(endDate == DateTime.MinValue)
                    {
                        PrintFiltered(meetings.Where(m => m.StartDate >= startDate));
                    }
                    else
                    {
                        PrintFiltered(meetings.Where(m => m.StartDate >= startDate).Where(m => m.EndDate <= endDate));
                    }
                    break;
                case "6":
                    var numOfAttendees = PromptUser<int>("Type in the number of attendees: ");
                    PrintFiltered(meetings.Where(m => m.Attendees.Count >= numOfAttendees));
                    break;
                case "0":
                    PrintFiltered(meetings);
                    break;
                default:
                    Console.WriteLine("Unrecognized command!");
                    break;
            }
        }
        public static void DisplayAddPersonCommandUI(MeetingService meetingService)
        {
            var meetingName = PromptUser<string>("Type in the name of a meeting you want the person to be added to: ");
            var firstName = PromptUser<string>("Type in the first name of the person you want added to the meeting: ");
            var lastName = PromptUser<string>("Type in the last name of the person you want added to the meeting: ");

            meetingService.AddPersonToMeeting(meetingName, new Person { FirstName = firstName, LastName = lastName });
        }
        public static void DisplayRemovePersonCommandUI(MeetingService meetingService)
        {
            var meetingName = PromptUser<string>("Type in the name of a meeting you want the person to be removed from: ");
            var firstName = PromptUser<string>("Type in the first name of the person you want added to the meeting: ");
            var lastName = PromptUser<string>("Type in the last name of the person you want added to the meeting: ");

            meetingService.RemovePersonFromMeeting(meetingName, new Person { FirstName = firstName, LastName = lastName });
        }
    }
}
