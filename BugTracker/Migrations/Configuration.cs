namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Administrator"))
            {
                roleManager.Create(new IdentityRole { Name = "Administrator" });
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "Stevenunderwood2@hotmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Stevenunderwood2@hotmail.com",
                    Email = "Stevenunderwood2@hotmail.com",
                    FirstName = "Steven",
                    LastName = "Underwood",
                    DisplayName = "#ImAwesome"
                }, "Abc123!");
            }

            var userId = userManager.FindByEmail("Stevenunderwood2@hotmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                    new TicketPriority { Name = "Immediate" },
                    new TicketPriority { Name = "High" },
                    new TicketPriority { Name = "Medium" },
                    new TicketPriority { Name = "Low" },
                    new TicketPriority { Name = "No Priority" },
                    new TicketPriority { Name = "Ignore" }
                );

            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                    new TicketType { Name = "Bug" },
                    new TicketType { Name = "Crash" },
                    new TicketType { Name = "Update" }
                );

            context.TicketStatuses.AddOrUpdate(
                t => t.Name,
                    new TicketStatus { Name = "Unassigned" },
                    new TicketStatus { Name = "Not in Progress" },
                    new TicketStatus { Name = "In Progress" },
                    new TicketStatus { Name = "On Hold" },
                    new TicketStatus { Name = "Resolved" },
                    new TicketStatus { Name = "Closed" }
                );

            context.ProjectStatuses.AddOrUpdate(
                p => p.Name,
                   new ProjectStats { Name = "Open" },
                   new ProjectStats { Name = "Closed" },
                   new ProjectStats { Name = "On Hold" },
                   new ProjectStats { Name = "Waiting on Approval" }
                );

            context.AllowedFileTypes.AddOrUpdate(
                 t => t.Type,
                    new AllowedFileType { Type = "txt" },
                    new AllowedFileType { Type = "png" },
                    new AllowedFileType { Type = "PNG" },
                    new AllowedFileType { Type = "doc" },
                    new AllowedFileType { Type = "docx" },
                    new AllowedFileType { Type = "jpeg" },
                    new AllowedFileType { Type = "jpg" },
                    new AllowedFileType { Type = "xls" },
                    new AllowedFileType { Type = "xlsx" },
                    new AllowedFileType { Type = "gif" },
                    new AllowedFileType { Type = "tiff" }
                );

        }
    }
}
