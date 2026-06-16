using System;
using System.Collections.Generic;
using System.Text;

namespace BRICOMA.ECOMMERCE.Models.Models
{
    public class EmailAppointementModel
    {
        public EmailAppointementModel()
        {
            Attendees = new List<AttendeeModel>();
        }

        public string Uid { get; set; }
        public string Summary { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Location { get; set; }
        public OrganizerModel Organizer { get; set; }
        public List<AttendeeModel> Attendees { get; set; }
    }

    public class OrganizerModel
    {
        public string CommonName { get; set; }
        public string Value { get; set; }
        public string SentBy { get; set; }
    }

    public class AttendeeModel
    {
        public string CommonName { get; set; }
        public string Mailto { get; set; }
        public bool Rsvp { get; set; }
    }
}

