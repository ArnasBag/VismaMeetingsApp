using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VismaMeetingsApp.Models;
using VismaMeetingsApp.Models.Enums;
using VismaMeetingsApp.Repositories;
using VismaMeetingsApp.Services;
using Xunit;

namespace VismaMeetingsApp.Tests
{
    public class MeetingServiceTests
    {
        private void SetupFile(List<Meeting> meetings = null)
        {
            if (File.Exists("data.json"))
            {
                File.Delete("data.json");
            }
            if(meetings != null)
            {
                var repository = new MeetingRepository();
                repository.WriteAllMeetings(meetings);
            }
            

        }

        [Fact]
        public void ReadAllMeetings_EmptyFile_ReturnsEmpyList()
        {
            //setup
            SetupFile();

            var meetingRepository = new MeetingRepository();
            
            var result = meetingRepository.ReadAllMeetings();

            Assert.Equal(new List<Meeting>(), result);
        }

        [Fact]
        public void ReadAllMeetings_FileWithData_ReturnsListOfMeetings()
        {
            var meetings = new List<Meeting>
            {
                new Meeting
                {
                    Name = "test1",
                    Description = "test1",
                    Category = MeetingCategory.TeamBuilding,
                    Type = MeetingType.Live,
                    ResponsiblePerson = new Person
                    {
                        FirstName = "testname",
                        LastName = "testname"
                    },
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                },
                new Meeting
                {
                    Name = "test2",
                    Description = "test2",
                    Category = MeetingCategory.TeamBuilding,
                    Type = MeetingType.Live,
                    ResponsiblePerson = new Person
                    {
                        FirstName = "testname",
                        LastName = "testname"
                    },
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                },
            };

            SetupFile(meetings);

            var meetingRepository = new MeetingRepository();

            var result = meetingRepository.ReadAllMeetings();

            Assert.Equal(meetings, result);
        }

        [Fact]
        public void CreateMeeting_NewMeeting_AddsMeetingToJsonFile()
        {
            SetupFile();

            var meeting = new Meeting
            {
                Name = "test1",
                Description = "test1",
                Category = MeetingCategory.TeamBuilding,
                Type = MeetingType.Live,
                ResponsiblePerson = new Person
                {
                    FirstName = "testname",
                    LastName = "testname"
                },
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };
            var repository = new MeetingRepository();
            var service = new MeetingService(repository, new Person { FirstName = "test", LastName = "test"});
            service.AddMeeting(meeting);

            var result = repository.ReadAllMeetings();

            Assert.Equal(meeting, result.SingleOrDefault());
        }

        [Fact]
        public void CreateMeeting_NewMeetingWithExistingName_DoesNotAddToJsonFile()
        {
            var meetings = new List<Meeting>
            {
                new Meeting
                {
                    Name = "test1",
                    Description = "test1",
                    Category = MeetingCategory.TeamBuilding,
                    Type = MeetingType.Live,
                    ResponsiblePerson = new Person
                    {
                        FirstName = "testname",
                        LastName = "testname"
                    },
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                }
            };

            SetupFile(meetings);

            var meeting = new Meeting
            {
                Name = "test1",
                Description = "test1",
                Category = MeetingCategory.TeamBuilding,
                Type = MeetingType.Live,
                ResponsiblePerson = new Person
                {
                    FirstName = "testname",
                    LastName = "testname"
                },
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            var repository = new MeetingRepository();
            var service = new MeetingService(repository, new Person { FirstName = "test", LastName = "test" });
            service.AddMeeting(meeting);

            var result = repository.ReadAllMeetings();

            Assert.Equal(meetings.Count, result.Count);
        }

        [Fact]
        public void DeleteMeeting_MeetingWithExistingNameAndResponsiblePerson_RemovesMeetingFromJsonFile()
        {
            var meetings = new List<Meeting>
            {
                new Meeting
                {
                    Name = "test1",
                    Description = "test1",
                    Category = MeetingCategory.TeamBuilding,
                    Type = MeetingType.Live,
                    ResponsiblePerson = new Person
                    {
                        FirstName = "testname",
                        LastName = "testname"
                    },
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                }
            };

            SetupFile(meetings);

            var meetingName = "test1";

            var repository = new MeetingRepository();
            var service = new MeetingService(repository, new Person { FirstName = "testname", LastName = "testname" });
            service.DeleteMeeting(meetingName);

            var result = repository.ReadAllMeetings();

            Assert.Empty(result);
        }

        [Fact]
        public void DeleteMeeting_MeetingThatDoesntExist_DoesNotRemoveMeetingFromJsonFile()
        {
            var meetings = new List<Meeting>
            {
                new Meeting
                {
                    Name = "test1",
                    Description = "test1",
                    Category = MeetingCategory.TeamBuilding,
                    Type = MeetingType.Live,
                    ResponsiblePerson = new Person
                    {
                        FirstName = "testname",
                        LastName = "testname"
                    },
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                }
            };

            SetupFile(meetings);

            var meetingName = "test2";

            var repository = new MeetingRepository();
            var service = new MeetingService(repository, new Person { FirstName = "test", LastName = "test" });
            service.DeleteMeeting(meetingName);

            var result = repository.ReadAllMeetings();

            Assert.Equal(meetings.Count, result.Count);
        }

        [Fact]
        public void DeleteMeeting_PersonNotResponsible_DoesNotRemoveMeetingFromJsonFile()
        {
            var meetings = new List<Meeting>
            {
                new Meeting
                {
                    Name = "test1",
                    Description = "test1",
                    Category = MeetingCategory.TeamBuilding,
                    Type = MeetingType.Live,
                    ResponsiblePerson = new Person
                    {
                        FirstName = "testname",
                        LastName = "testname"
                    },
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                }
            };

            SetupFile(meetings);

            var meetingName = "test1";

            var repository = new MeetingRepository();
            var service = new MeetingService(repository, new Person { FirstName = "nottestname", LastName = "nottestname" });
            service.DeleteMeeting(meetingName);

            var result = repository.ReadAllMeetings();

            Assert.Equal(meetings.Count, result.Count);
        }

        [Fact]
        public void AddPersonToMeeting_ValidMeeting_AddsPersonToMeeting()
        {
            var meetings = new List<Meeting>
            {
                new Meeting
                {
                    Name = "test1",
                    Description = "test1",
                    Category = MeetingCategory.TeamBuilding,
                    Type = MeetingType.Live,
                    ResponsiblePerson = new Person
                    {
                        FirstName = "testname",
                        LastName = "testname"
                    },
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                }
            };

            SetupFile(meetings);

            var meetingName = "test1";

            var repository = new MeetingRepository();
            var service = new MeetingService(repository, new Person { FirstName = "test", LastName = "test" });
            service.AddPersonToMeeting(meetingName, new Person { FirstName = "person", LastName = "person"});

            var result = repository.ReadAllMeetings().Single().Attendees;

            Assert.Equal(new List<Person>() { new Person { FirstName = "person", LastName = "person"} }, result);
        }

        [Fact]
        public void AddPersonToMeeting_PersonWithSameName_DoesNotAddPersonToMeeting()
        {
            var meetings = new List<Meeting>
            {
                new Meeting
                {
                    Name = "test1",
                    Description = "test1",
                    Category = MeetingCategory.TeamBuilding,
                    Type = MeetingType.Live,
                    ResponsiblePerson = new Person
                    {
                        FirstName = "testname",
                        LastName = "testname"
                    },
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Attendees = new List<Person>
                    {
                        new Person
                        {
                            FirstName = "person",
                            LastName = "person"
                        }
                    }
                }
            };

            SetupFile(meetings);

            var meetingName = "test1";

            var repository = new MeetingRepository();
            var service = new MeetingService(repository, new Person { FirstName = "test", LastName = "test" });
            service.AddPersonToMeeting(meetingName, new Person { FirstName = "person", LastName = "person"});

            var result = repository.ReadAllMeetings().Single().Attendees;

            Assert.Equal(new List<Person>() { new Person { FirstName = "person", LastName = "person"} }, result);
        }

        [Fact]
        public void RemovePersonFromMeeting_ValidMeetingWithPerson_RemovesPersonFromMeeting()
        {
            var meetings = new List<Meeting>
            {
                new Meeting
                {
                    Name = "test1",
                    Description = "test1",
                    Category = MeetingCategory.TeamBuilding,
                    Type = MeetingType.Live,
                    ResponsiblePerson = new Person
                    {
                        FirstName = "testname",
                        LastName = "testname"
                    },
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Attendees = new List<Person>
                    {
                        new Person
                        {
                            FirstName = "person",
                            LastName = "person"
                        }
                    }
                }
            };

            SetupFile(meetings);

            var meetingName = "test1";

            var repository = new MeetingRepository();
            var service = new MeetingService(repository, new Person { FirstName = "test", LastName = "test" });
            service.RemovePersonFromMeeting(meetingName, new Person { FirstName = "person", LastName = "person" });

            var result = repository.ReadAllMeetings().Single().Attendees;

            Assert.Empty(result);
        }
    }
}