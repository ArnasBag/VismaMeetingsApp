using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaMeetingsApp.Models;
using VismaMeetingsApp.Repositories;

namespace VismaMeetingsApp.Services
{
    public class MeetingService
    {
        private readonly MeetingRepository _repository;
        private readonly Person _loggedInPerson;

        public MeetingService(MeetingRepository repository, Person loggedInPerson)
        {
            _repository = repository;
            _loggedInPerson = loggedInPerson;
        }
        public void AddMeeting(Meeting meeting)
        {
            if (!_repository.Exists(meeting))
            {
                _repository.AddMeeting(meeting);
                Console.WriteLine($"Successfully created meeting \"{meeting.Name}\"");
            }
            else
            {
                Console.WriteLine("Meeting with this name already exists!");
            }
        }
        public void DeleteMeeting(string meetingName)
        {
            var meeting = _repository.GetMeetingByName(meetingName);

            if (meeting == null)
            {
                Console.WriteLine($"Meeting with the name \"{meetingName}\" does not exist!");
            }
            else
            {
                if (!meeting.ResponsiblePerson.Equals(_loggedInPerson))
                {
                    Console.WriteLine($"Only the person responsible can delete meeting \"{meetingName}\"!");
                }
                else
                {
                    var deleted = _repository.DeleteMeeting(meeting);
                    if (deleted)
                    {
                        Console.WriteLine($"Meeting \"{meetingName}\" successfully deleted!");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to delete meeting \"{meetingName}\"");
                    }
                }
            }
        }
        public void AddPersonToMeeting(string meetingName, Person person)
        {
            var meeting = _repository.GetMeetingByName(meetingName);

            if (meeting.Attendees.Contains(person) ||
                meeting.ResponsiblePerson.Equals(person))
            {
                Console.WriteLine($"Failed to add {person.FirstName} {person.LastName} to the meeting!");
            }
            else
            {
                var personsMeetings = _repository.ReadAllMeetings()
                    .Where(m => m.Attendees.Contains(person));

                if (personsMeetings.Any(m => !(m.StartDate > meeting.EndDate || meeting.StartDate < m.EndDate)))
                {
                    Console.WriteLine("Warning: this person has other meetings at the time of this meeting!");
                }

                _repository.AddPersonToMeeting(meeting, person);
                Console.WriteLine($"Successfully added {person.FirstName} {person.LastName} to meeting {meetingName}");
            }
        }
        public void RemovePersonFromMeeting(string meetingName, Person person)
        {
            var meeting = _repository.GetMeetingByName(meetingName);

            if (meeting == null)
            {
                Console.WriteLine($"Meeting with the name \"{meetingName}\" does not exist!");
            }

            else if (meeting.ResponsiblePerson.Equals(person))
            {
                Console.WriteLine("Cannot remove person responsible for this meeting");
            }
            else
            {
                var removed = _repository.RemovePersonFromMeeting(meeting, person);

                if (!removed)
                {
                    Console.WriteLine("This person is not assigned to this meeting");
                }
                else
                {
                    Console.WriteLine($"Successfully removed {person.FirstName} {person.LastName} from meeting {meetingName}");
                }
            }
        }
    }
}
