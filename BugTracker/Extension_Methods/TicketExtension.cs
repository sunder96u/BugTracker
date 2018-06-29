using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace BugTracker.Extension_Methods
{
    public static class TicketExtension
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void RecordChanges(this Ticket ticket, Ticket oldTicket)
        {
            //retrieve the tracked properties from the web config file
            var propertyList = WebConfigurationManager.AppSettings["TrackedTicketProperties"].Split(',');

            //Iterate over the properties of the Ticket Type
            foreach (PropertyInfo prop in ticket.GetType().GetProperties())
            {
                if (!propertyList.Contains(prop.Name))
                    continue;

                var value = prop.GetValue(ticket, null) ?? "";
                var oldValue = prop.GetValue(oldTicket, null) ?? "";

                if (value.ToString() != oldValue.ToString())
                {
                    var ticketHistory = new TicketHistory
                    {
                        Changed = DateTime.Now,
                        Property = prop.Name,
                        NewValue = GetValueFromKey(prop.Name, value),
                        OldValue = GetValueFromKey(prop.Name, oldValue),
                        TicketId = ticket.Id,
                        UserId = HttpContext.Current.User.Identity.GetUserId()
                    };

                    db.TicketHistories.Add(ticketHistory);
                }

                db.SaveChanges();
            }
        }

        private static string GetValueFromKey(string keyName, object key)
        {
            var returnValue = "";
            if (key.ToString() == string.Empty)
                return returnValue;

            switch (keyName)
            {
                case "ProjectId":
                    returnValue = db.Projects.Find(key).Name;
                    break;
                case "TicketTypeId":
                    returnValue = db.TicketTypes.Find(key).Name;
                    break;
                case "TicketPriorityId":
                    returnValue = db.TicketPriorities.Find(key).Name;
                    break;
                default:
                    returnValue = key.ToString();
                    break;
            }
            return returnValue;
        }

        public static void DevChanges(this Ticket ticket, Ticket oldTicket)
        {
            //retrieve the tracked properties from the web config file
            var propertyList = WebConfigurationManager.AppSettings["DevChange"];

            //Iterate over the properties of the Ticket Type
            foreach (PropertyInfo prop in ticket.GetType().GetProperties())
            {
                if (propertyList != prop.Name)
                    continue;

                var value = prop.GetValue(ticket, null) ?? "";
                var oldValue = prop.GetValue(oldTicket, null) ?? "";

                if (value.ToString() != oldValue.ToString())
                {
                    var ticketHistory = new TicketHistory
                    {
                        Changed = DateTime.Now,
                        Property = prop.Name,
                        NewValue = DevChangeKey(prop.Name, value),
                        OldValue = DevChangeKey(prop.Name, oldValue),
                        TicketId = ticket.Id,
                        UserId = HttpContext.Current.User.Identity.GetUserId()
                    };

                    db.TicketHistories.Add(ticketHistory);
                }

                db.SaveChanges();
            }
        }

        private static string DevChangeKey(string keyName, object key)
        {
            var returnValue = "";
            if (key.ToString() == string.Empty)
                return returnValue;

            switch (keyName)
            {
                case "AssignedToUserId":
                    returnValue = db.Users.Find(key).DisplayName;
                    break;

                default:
                    returnValue = key.ToString();
                    break;
            }
            return returnValue;
        }

        public static void SubChanges(this Ticket ticket, Ticket oldTicket)
        {
            //retrieve the tracked properties from the web config file
            var propertyList = WebConfigurationManager.AppSettings["SubChange"];

            //Iterate over the properties of the Ticket Type
            foreach (PropertyInfo prop in ticket.GetType().GetProperties())
            {
                if (propertyList != prop.Name)
                    continue;

                var value = prop.GetValue(ticket, null) ?? "";
                var oldValue = prop.GetValue(oldTicket, null) ?? "";

                if (value.ToString() != oldValue.ToString())
                {
                    var ticketHistory = new TicketHistory
                    {
                        Changed = DateTime.Now,
                        Property = prop.Name,
                        NewValue = SubChangeKey(prop.Name, value),
                        OldValue = SubChangeKey(prop.Name, oldValue),
                        TicketId = ticket.Id,
                        UserId = HttpContext.Current.User.Identity.GetUserId()
                    };

                    db.TicketHistories.Add(ticketHistory);
                }

                db.SaveChanges();
            }
        }

        private static string SubChangeKey(string keyName, object key)
        {
            var returnValue = "";
            if (key.ToString() == string.Empty)
                return returnValue;

            switch (keyName)
            {
                case "OwnerUserId":
                    returnValue = db.Users.Find(key).DisplayName;
                    break;

                default:
                    returnValue = key.ToString();
                    break;
            }
            return returnValue;
        }

        public static void StatusChanges(this Ticket ticket, Ticket oldTicket)
        {
            //retrieve the tracked properties from the web config file
            var propertyList = WebConfigurationManager.AppSettings["StatusChange"];

            //Iterate over the properties of the Ticket Type
            foreach (PropertyInfo prop in ticket.GetType().GetProperties())
            {
                //if (!propertyList.Contains(prop.Name))
                //    continue;
                if (propertyList != prop.Name)
                    continue;

                var value = prop.GetValue(ticket, null) ?? "";
                var oldValue = prop.GetValue(oldTicket, null) ?? "";

                if (value.ToString() != oldValue.ToString())
                {
                    var ticketHistory = new TicketHistory
                    {
                        Changed = DateTime.Now,
                        Property = prop.Name,
                        NewValue = StatusChangeKey(prop.Name, value),
                        OldValue = StatusChangeKey(prop.Name, oldValue),
                        TicketId = ticket.Id,
                        UserId = HttpContext.Current.User.Identity.GetUserId()
                    };

                    db.TicketHistories.Add(ticketHistory);
                }

                db.SaveChanges();
            }
        }

        public static void Status1Changes(this Ticket ticket, Ticket oldTicket)
        {
            //retrieve the tracked properties from the web config file
            var propertyList = WebConfigurationManager.AppSettings["StatusChange"];

            //Iterate over the properties of the Ticket Type
            foreach (PropertyInfo prop in ticket.GetType().GetProperties())
            {
                //if (!propertyList.Contains(prop.Name))
                //    continue;
                if (propertyList != prop.Name)
                    continue;

                var value = prop.GetValue(ticket, null) ?? "";
                var oldValue = prop.GetValue(oldTicket, null) ?? "";

                if (value.ToString() != oldValue.ToString())
                {
                    var ticketHistory = new TicketHistory
                    {
                        Changed = DateTime.Now,
                        Property = prop.Name,
                        NewValue = Status1ChangeKey(prop.Name, value),
                        OldValue = Status1ChangeKey(prop.Name, oldValue),
                        TicketId = ticket.Id,
                        UserId = HttpContext.Current.User.Identity.GetUserId()
                    };

                    db.TicketHistories.Add(ticketHistory);
                }
                var newClose = (ticket.Id == ticket.Id);

                var body = new StringBuilder();
                body.AppendLine("<br/><p><u><b>Message:</b></u></p>");
                body.AppendFormat("<p><b>A Change has been made to your Ticket:</b> {0}</p>", oldTicket.Title);
                body.AppendFormat("<p><b>The Ticket has been closed</b><p>");


                TicketNotification notification = null;
                if (newClose)
                {
                    notification = new TicketNotification
                    {
                        Body = body.ToString(),
                        Created = DateTimeOffset.Now,
                        RecieverId = ticket.AssignedToUserId,
                        TicketId = ticket.Id
                    };
                    db.TicketNotifications.Add(notification);
                }

                db.SaveChanges();
            }
        }

        public static void Status2Changes(this Ticket ticket, Ticket oldTicket)
        {
            //retrieve the tracked properties from the web config file
            var propertyList = WebConfigurationManager.AppSettings["StatusChange"];

            //Iterate over the properties of the Ticket Type
            foreach (PropertyInfo prop in ticket.GetType().GetProperties())
            {
                //if (!propertyList.Contains(prop.Name))
                //    continue;
                if (propertyList != prop.Name)
                    continue;

                var value = prop.GetValue(ticket, null) ?? "";
                var oldValue = prop.GetValue(oldTicket, null) ?? "";

                if (value.ToString() != oldValue.ToString())
                {
                    var ticketHistory = new TicketHistory
                    {
                        Changed = DateTime.Now,
                        Property = prop.Name,
                        NewValue = Status2ChangeKey(prop.Name, value),
                        OldValue = Status2ChangeKey(prop.Name, oldValue),
                        TicketId = ticket.Id,
                        UserId = HttpContext.Current.User.Identity.GetUserId()
                    };

                    db.TicketHistories.Add(ticketHistory);
                }
                var newClose = (ticket.Id == ticket.Id);

                var body = new StringBuilder();
                body.AppendLine("<br/><p><u><b>Message:</b></u></p>");
                body.AppendFormat("<p><b>A Change has been made to your Ticket:</b> {0}</p>", oldTicket.Title);
                body.AppendFormat("<p><b>The Ticket has been Reopenned</b><p>");


                TicketNotification notification = null;
                if (newClose)
                {
                    notification = new TicketNotification
                    {
                        Body = body.ToString(),
                        Created = DateTimeOffset.Now,
                        RecieverId = ticket.AssignedToUserId,
                        TicketId = ticket.Id
                    };
                    db.TicketNotifications.Add(notification);
                }

                db.SaveChanges();
            }
        }

        private static string StatusChangeKey(string keyName, object key)
        {
            var returnValue = "";
            if (key.ToString() == string.Empty)
                return returnValue;

            switch (keyName)
            {
                case "TicketStatusId":
                    returnValue = db.TicketStatuses.Find(key).Name;
                    break;

                default:
                    returnValue = key.ToString();
                    break;
            }
            return returnValue;
        }

        private static string Status1ChangeKey(string keyName, object key)
        {
            var returnValue = "";
            if (key.ToString() == string.Empty)
                return returnValue;

            switch (keyName)
            {
                case "TicketStatusId":
                    returnValue = db.TicketStatuses.Find(key).Name;
                    break;

                default:
                    returnValue = key.ToString();
                    break;
            }
            return returnValue;
        }

        private static string Status2ChangeKey(string keyName, object key)
        {
            var returnValue = "";
            if (key.ToString() == string.Empty)
                return returnValue;

            switch (keyName)
            {
                case "TicketStatusId":
                    returnValue = db.TicketStatuses.Find(key).Name;
                    break;

                default:
                    returnValue = key.ToString();
                    break;
            }
            return returnValue;
        }

        public static async Task DeveloperAssignemt(this Ticket ticket, Ticket oldTicket)
        {
            //create the variable for what the notifications will be sent about
            //Assignment of Developer
            var newAssignment = (ticket.AssignedToUserId != null && oldTicket.AssignedToUserId == null);
            var unAssignment = (ticket.AssignedToUserId == null && oldTicket.AssignedToUserId != null);
            var reAssignment = ((ticket.AssignedToUserId != null && oldTicket.AssignedToUserId != null));


            var body = new StringBuilder();
            body.AppendFormat("<p>Email From: <bold>{0}</bold> ({1})</p>", "Administrator", WebConfigurationManager.AppSettings["emailfrom"]);
            body.AppendLine("<br/><p><u><b>Message:</b></u></p>");
            body.AppendFormat("<p><b>Project Name:</b> {0}</p>", db.Projects.FirstOrDefault(p => p.Id == oldTicket.ProjectId).Name);
            body.AppendFormat("<p><b>Ticket Title:</b> {0} | Id: {1}</p>", oldTicket.Title, oldTicket.Id);
            body.AppendFormat("<p><b>Ticket Created:</b> {0}</p>", oldTicket.Created);
            body.AppendFormat("<p><b>Ticket Type:</b> {0}</p>", db.TicketTypes.Find(oldTicket.TicketTypeId).Name);
            body.AppendFormat("<p><b>Ticket Status:</b> {0}</p>", db.TicketStatuses.Find(oldTicket.TicketStatusId).Name);
            body.AppendFormat("<p><b>Ticket Priority:</b> {0}</p>", db.TicketPriorities.Find(oldTicket.TicketPriorityId).Name);

            //Generate the Email
            IdentityMessage email = null;
            if (newAssignment)
            {
                email = new IdentityMessage()
                {
                    Subject = "Notification: A Ticket has been assigned to you",
                    Body = body.ToString(),
                    Destination = db.Users.Find(ticket.AssignedToUserId).Email
                };

                var svc = new EmailService();
                await svc.SendAsync(email);
            }


            else if (unAssignment)
            {
                email = new IdentityMessage()
                {
                    Subject = "Notification: You have been removed from a ticket",
                    Body = body.ToString(),
                    Destination = db.Users.Find(oldTicket.AssignedToUserId).Email
                };

                var svc = new EmailService();
                await svc.SendAsync(email);
            }

            else if (reAssignment)
            {
                email = new IdentityMessage()
                {
                    Subject = "Notification: A Ticket has been assigned to you",
                    Body = body.ToString(),
                    Destination = db.Users.Find(ticket.AssignedToUserId).Email
                };

                var svc = new EmailService();
                await svc.SendAsync(email);

                email = new IdentityMessage()
                {
                    Subject = "Notification: You have been removed from a ticket",
                    Body = body.ToString(),
                    Destination = db.Users.Find(oldTicket.AssignedToUserId).Email
                };

                svc = new EmailService();
                await svc.SendAsync(email);
            }

            TicketNotification notification = null;
            if (newAssignment)
            {
                notification = new TicketNotification
                {
                    Body = body.ToString() + " has been assigned to the ticket",
                    Created = DateTimeOffset.Now,
                    RecieverId = ticket.AssignedToUserId,
                    TicketId = ticket.Id
                };
                db.TicketNotifications.Add(notification);
            }

            else if (unAssignment)
            {
                notification = new TicketNotification
                {
                    Body = body.ToString() + " has been removed for the ticket",
                    Created = DateTimeOffset.Now,
                    RecieverId = oldTicket.AssignedToUserId,
                    TicketId = ticket.Id
                };
                db.TicketNotifications.Add(notification);
            }
            else if (reAssignment)
            {
                notification = new TicketNotification
                {
                    Body = body.ToString() + " has been assigned to the ticket",
                    Created = DateTimeOffset.Now,
                    RecieverId = ticket.AssignedToUserId,
                    TicketId = ticket.Id
                };
                db.TicketNotifications.Add(notification);

                notification = new TicketNotification
                {
                    Body = body.ToString() + " has been removed for the ticket",
                    Created = DateTimeOffset.Now,
                    RecieverId = oldTicket.AssignedToUserId,
                    TicketId = ticket.Id
                };
                db.TicketNotifications.Add(notification);
            }
            db.SaveChanges();
        }

        public static void CommentAdded(this TicketComment ticketComment, TicketComment oldTicketComment)
        {
            //create the variable for what the notifications will be sent about
            //Assignment of Developer
            var newComment = (ticketComment.TicketId == ticketComment.TicketId);
            var time = ticketComment.Created.ToString().Split(' ');


            var body = new StringBuilder();
            body.AppendLine("<br/><p><u><b>Message:</b></u></p>");
            body.AppendFormat("<p><b>A comment has been added to your Ticket:</b> {0}</p>", ticketComment.Ticket.Title);
            body.AppendFormat("<p><b>Created by:</b> {0} on {1} @ {2} {3}</p>", ticketComment.User.DisplayName , time[0], time[1], time[2]);
            body.AppendFormat("<p><b>Comment:</b> {0}</p>", ticketComment.CommentBody);

            TicketNotification notification = null;
            if (newComment)
            {
                notification = new TicketNotification
                {
                    Body = body.ToString() + " is a comment on the ticket",
                    Created = DateTimeOffset.Now,
                    RecieverId = ticketComment.Ticket.AssignedToUserId,
                    TicketId = ticketComment.Ticket.Id
                };
                db.TicketNotifications.Add(notification);
            }
            
            db.SaveChanges();
        }

        public static void AttachmentAdded(this TicketAttachment ticketAttachment, TicketAttachment oldTicketAttachment)
        {
            //create the variable for what the notifications will be sent about
            //Assignment of Developer

                var newAttachment = (ticketAttachment.TicketId == ticketAttachment.TicketId);
            var time = ticketAttachment.Created.ToString().Split(' ');


            var body = new StringBuilder();
            body.AppendLine("<br/><p><u><b>Message:</b></u></p>");
            body.AppendFormat("<p><b>An Attachment has been added to your Ticket:</b> {0}</p>", ticketAttachment.Ticket.Title);
            body.AppendFormat("<p><b>Created by:</b> {0} on {1} @ {2} {3}</p>", ticketAttachment.User.DisplayName, time[0], time[1], time[2]);
            body.AppendFormat("<p><b>Attachment Description:</b> {0}</p>", ticketAttachment.Description);

            TicketNotification notification = null;
            if (newAttachment)
            {
                notification = new TicketNotification
                {
                    Body = body.ToString() + " is an attachment on the ticket",
                    Created = DateTimeOffset.Now,
                    RecieverId = ticketAttachment.Ticket.AssignedToUserId,
                    TicketId = ticketAttachment.Ticket.Id
                };
                db.TicketNotifications.Add(notification);
            }

            db.SaveChanges();
        }

        public static void AttachmentEditted(this TicketAttachment ticketAttachment, TicketAttachment oldTicketAttachment)
        {
            //create the variable for what the notifications will be sent about
            //Assignment of Developer
            var EditedAttachment = (ticketAttachment.FilePath != oldTicketAttachment.FilePath);

            var body = new StringBuilder();
            body.AppendLine("<br/><p><u><b>Message:</b></u></p>");
            body.AppendFormat("<p><b>An Attachment has been Edited on your Ticket:</b> {0}</p>", ticketAttachment.Ticket.Title);
            body.AppendFormat("<p><b>Edited by:</b> {0} on {1}</p>", ticketAttachment.User.DisplayName, ticketAttachment.Created);
            body.AppendFormat("<p><b>Attachment Description:</b> {0}</p>", ticketAttachment.Description);

            TicketNotification notification = null;
            if (EditedAttachment)
            {
                notification = new TicketNotification
                {
                    Body = body.ToString() + " is an edited attachment on the ticket",
                    Created = DateTimeOffset.Now,
                    RecieverId = ticketAttachment.Ticket.AssignedToUserId,
                    TicketId = ticketAttachment.Ticket.Id
                };
                db.TicketNotifications.Add(notification);
            }

            db.SaveChanges();
        }

    }
}