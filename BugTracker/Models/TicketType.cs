﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Navigational Properties

        public virtual ICollection<Ticket> Tickets { get; set; }

        //Onitilaize all of the ICollections to HashSets
        public TicketType ()
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}