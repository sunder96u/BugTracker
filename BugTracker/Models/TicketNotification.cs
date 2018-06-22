using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Body { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
        public DateTimeOffset Created { get; set; }

        //Navigational properties
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}