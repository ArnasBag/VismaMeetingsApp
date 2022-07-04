using System.Text.Json;
using System.Text.Json.Serialization;
using VismaMeetingsApp.Models;

namespace VismaMeetingsApp.Repositories
{
    public class MeetingRepository
    {
        private readonly JsonSerializerOptions options;
        private List<Meeting> meetings;
        public MeetingRepository()
        {
            options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            options.WriteIndented = true;

            meetings = ReadAllMeetings();
        }
        public void AddMeeting(Meeting meeting)
        {
            meetings.Add(meeting);
            WriteAllMeetings(meetings);               
        }
        public bool DeleteMeeting(Meeting meeting)
        {
            var removed = meetings.Remove(meeting);

            if (removed)
            {
                WriteAllMeetings(meetings);
            }
            return removed;
        }
        public Meeting GetMeetingByName(string meetingName)
        {
            return meetings.SingleOrDefault(m => m.Name == meetingName);
        }
        public bool Exists(Meeting meeting)
        {
            return meetings.Contains(meeting);
        }
        public void AddPersonToMeeting(Meeting meeting, Person personToAdd)
        {
            meeting.Attendees.Add(personToAdd);
            WriteAllMeetings(meetings);
        }
        public bool RemovePersonFromMeeting(Meeting meeting, Person personToRemove)
        {
            var removed = meeting.Attendees.Remove(personToRemove);

            if (removed)
            {
                WriteAllMeetings(meetings);
            }
            return removed;
        }
        public List<Meeting> ReadAllMeetings()
        {
            if (!File.Exists("data.json"))
            {
                File.Create("data.json").Dispose();
            }   

            string data = File.ReadAllText("data.json");

            if(data == "")
            {
                return new List<Meeting>();
            }

            var meetings = JsonSerializer.Deserialize<List<Meeting>>(data, options);
            return meetings;
        }
        public void WriteAllMeetings(List<Meeting> meetings)
        {
            var json = JsonSerializer.Serialize(meetings, options);
            File.WriteAllText("data.json", json);
        }
    }
}
