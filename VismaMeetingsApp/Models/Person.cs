using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaMeetingsApp.Models
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override bool Equals(object? obj)
        {
            var person = obj as Person;

            if(person is null)
            {
                return false;
            }
            return FirstName == person.FirstName && LastName == person.LastName;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
