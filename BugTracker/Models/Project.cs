using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }       
        public string ProjectStatusId { get; set; }


        //Navigational Properties

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }


        //Initialize all of the ICollections to HashSets
        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }

        public virtual ProjectStats ProjectStats { get; set; }
    }
}