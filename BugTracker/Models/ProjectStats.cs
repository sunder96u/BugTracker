using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ProjectStats
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Navigational Properties

        public virtual ICollection<Project> Projects { get; set; }

        //Initialize all of the ICollections to HashSets
        public ProjectStats()
        {
            Projects = new HashSet<Project>();
        }
    }
}