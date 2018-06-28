﻿using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolesHelper = new UserRolesHelper();

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var flag = project.Users.Any(u => u.Id == userId);
            return (flag);
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);

            var projects = user.Projects.ToList();
            return (projects);
        }


        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);

                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        internal IEnumerable<object> ListUserRoles(string PMs)
        {
            throw new NotImplementedException();
        }

        public ICollection<Project> myProjects(string userId)
        {
            var Projects = new List<Project>();
            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
            switch(myRole)
            {
                case ("Admin"):
                    Projects.AddRange(db.Projects.ToList());
                    break;
                case ("Project Manager"):
                    foreach (var proj in ListUserProjects(userId))
                    {
                        Projects.AddRange(db.Projects.Where(t => t.Id == proj.Id).ToList());
                    }
                    break;
                case ("Developer"):
                    foreach (var proj in ListUserProjects(userId))
                    {
                        Projects.AddRange(db.Projects.Where(t => t.Id == proj.Id).ToList());
                    }
                    break;
                case ("Submitter"):
                    foreach (var proj in ListUserProjects(userId))
                    {
                        Projects.AddRange(db.Projects.Where(t => t.Id == proj.Id).ToList());
                    }
                    break;
            }
            return Projects;
            //foreach (var proj in ListUserProjects(userId))
            //{
            //    Projects.AddRange(db.Projects.Where(t => t.Id == proj.Id).ToList());
            //}
            //return Projects;
        }

        public ICollection<Project> allProjects(string userId)
        {
            var Projects = new List<Project>();
            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
            switch(myRole)
            {
                case ("Admin"):
                    Projects.AddRange(db.Projects.ToList());
                    break;
                case ("Project Manager"):
                    Projects.AddRange(db.Projects.ToList());
                    break;
            }
            return Projects;
        }

        public ICollection<ApplicationUser> UsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }

        public ICollection<ApplicationUser> UsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }

    }
}