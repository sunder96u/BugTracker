using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BugTracker.Action_Filters
{
    public class TicketAuthorization : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ticketId = Convert.ToInt32(filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id").Value);
            var ticket = db.Tickets.Find(ticketId);
            string userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            if (userId == null)
            {
                filterContext.Controller.TempData.Add("OopsMsg", "You have to be logged in and authorized to edit Tickets.");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops2" } });
            }
            else if (ticket == null)
            {
                filterContext.Controller.TempData.Add("OopsMsg", "The ticket does not exist");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops3" } });
            }
            else if (myRole == "Developer" && ticket.AssignedToUserId != userId || myRole == "Submitter" && ticket.AssignedToUserId != userId)
            {
                var userName = db.Users.Find(userId).DisplayName;
                var phrase = myRole == "Developer" ? "are not assigned to" : "do not own";
                var msg = string.Format("Hello {0}", userName);
                filterContext.Controller.TempData.Add("OopsMsg", msg);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops" } });
                
            }
            else if (myRole == "Project Manager")
            {
                var myProjectIds = db.Users.Find(userId).Projects.Select(p => p.Id).ToList();
                if(!myProjectIds.Contains(ticket.ProjectId))
                {
                    var userName = db.Users.Find(userId).DisplayName;
                    var msg = string.Format("Hello {0}", userName);
                    filterContext.Controller.TempData.Add("OopsMsg", msg);
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops" } });
                }
            }

            base.OnActionExecuting(filterContext);
        }

    }

    public class ProjectAuthorization : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var projectId = Convert.ToInt32(filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id").Value);
            var project = db.Projects.Find(projectId);

            if (project == null)
            {
                filterContext.Controller.TempData.Add("OopsMsg", "The Project does not exist");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops4" } });
            }


            base.OnActionExecuting(filterContext);
        }
    }

    public class TicketFinder : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ticketId = Convert.ToInt32(filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id").Value);
            var ticket = db.Tickets.Find(ticketId);

            if (ticket == null)
            {
                filterContext.Controller.TempData.Add("OopsMsg", "The Ticket does not exist");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops3" } });
            }


            base.OnActionExecuting(filterContext);
        }
    }

    public class AdminAuthorization : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            if (userId == null)
            {
                filterContext.Controller.TempData.Add("OopsMsg", "You have to be logged in to view this page.");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops2" } });
            }
            else if(myRole == "Developer")
            {
                var userName = db.Users.Find(userId).DisplayName;
                var phrase = myRole == "Developer" ? "You are not authorized to be here" : "do not belong";
                var msg = string.Format("hello {0}", userName);
                filterContext.Controller.TempData.Add("OopsMsg", msg);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops2" } });
            }
            else if (myRole == "Project Manager")
            {
                var userName = db.Users.Find(userId).DisplayName;
                var phrase = myRole == "Project Manager" ? "You are not authorized to be here" : "do not belong";
                var msg = string.Format("hello {0}", userName);
                filterContext.Controller.TempData.Add("OopsMsg", msg);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops2" } });
            }
            else if (myRole == "Submitter")
            {
                var userName = db.Users.Find(userId).DisplayName;
                var phrase = myRole == "Submitter" ? "You are not authorized to be here" : "do not belong";
                var msg = string.Format("hello {0}", userName);
                filterContext.Controller.TempData.Add("OopsMsg", msg);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops2" } });
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class Admin_PMAuthorization : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var projectId = Convert.ToInt32(filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id").Value);
            var project = db.Projects.Find(projectId);

            if (userId == null)
            {
                filterContext.Controller.TempData.Add("OopsMsg", "You have to be logged in to view this page.");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops2" } });
            }
            else if (myRole == "Developer")
            {
                var userName = db.Users.Find(userId).DisplayName;
                var phrase = myRole == "Developer" ? "You are not authorized to be here" : "do not belong";
                var msg = string.Format("hello {0}", userName);
                filterContext.Controller.TempData.Add("OopsMsg", msg);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops2" } });
            }
            else if (myRole == "Submitter")
            {
                var userName = db.Users.Find(userId).DisplayName;
                var phrase = myRole == "Submitter" ? "You are not authorized to be here" : "do not belong";
                var msg = string.Format("hello {0}", userName);
                filterContext.Controller.TempData.Add("OopsMsg", msg);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops2" } });
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class SubmitterAuthorization : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            if (userId == null)
            {
                filterContext.Controller.TempData.Add("OopsMsg", "You have to be logged in to view this page.");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops2" } });
            }
            else if (myRole == "Developer")
            {
                var userName = db.Users.Find(userId).DisplayName;
                var phrase = myRole == "Developer" ? "You are not authorized to be here" : "do not belong";
                var msg = string.Format("hello {0}", userName);
                filterContext.Controller.TempData.Add("OopsMsg", msg);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops2" } });
            }
            else if (myRole == "Project Manager")
            {
                var userName = db.Users.Find(userId).DisplayName;
                var phrase = myRole == "Project Manager" ? "You are not authorized to be here" : "do not belong";
                var msg = string.Format("hello {0}", userName);
                filterContext.Controller.TempData.Add("OopsMsg", msg);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops2" } });
            }
            else if (myRole == "Admin")
            {
                var userName = db.Users.Find(userId).DisplayName;
                var phrase = myRole == "Admin" ? "You are not authorized to be here" : "do not belong";
                var msg = string.Format("hello {0}", userName);
                filterContext.Controller.TempData.Add("OopsMsg", msg);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "action", "Oops2" } });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}