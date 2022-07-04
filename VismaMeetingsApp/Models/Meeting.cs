using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaMeetingsApp.Models.Enums;

namespace VismaMeetingsApp.Models
{
    public class Meeting
    {
        public string Name { get; set; }
        public Person ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public MeetingCategory Category { get; set; }
        public MeetingType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Person> Attendees { get; set; } = new();

        public override string ToString()
        {
            return $"Name: \t{Name}\n" +
                $"Responsible person: \t{ResponsiblePerson.FirstName} {ResponsiblePerson.LastName}\n" +
                $"Description: \t{Description}\n" +
                $"Category: \t{Category}\n" +
                $"Type: \t{Type}\n" +
                $"Start date: \t{StartDate}\n" +
                $"End date: \t{EndDate}\n" +
                $"------------------------------------------";
        }
        public override bool Equals(object? obj)
        {
            var meeting = obj as Meeting;

            if (meeting is null)
            {
                return false;
            }
            return Name == meeting.Name;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
