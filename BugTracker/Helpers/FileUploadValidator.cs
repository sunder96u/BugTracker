using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using BugTracker.Models;

namespace BugTracker.Views
{
    public class FileUploadValidator
    {
        public static ApplicationDbContext db = new ApplicationDbContext();

        public static bool IsWebFriendlyFile(HttpPostedFileBase file)
        {
            if (file == null)
                return false;

            try
            {
                var allowedFileTypes = db.AllowedFileTypes.Select(t => t.Type).ToList();
                var fileExt = Path.GetExtension(file.FileName);
                fileExt = fileExt.Split('.')[1];
                return allowedFileTypes.Contains(fileExt);
            }
            catch
            {
                return false;
            }
        }

        //public static bool IsWebFriendlyFile(HttpPostedFileBase file)
        //{
        //    if (file == null)
        //        return false;

        //    try
        //    {
        //        var allowedFileTypes = new List<string>
        //        {
        //            ".txt",
        //            ".doc",
        //            ".docx",
        //            ".pdf",
        //            ".xls",
        //            ".xlsx",
        //            ".jpg",
        //            ".jpeg",
        //            ".png",".PNG",
        //            ".tiff",
        //            ".gif"

        //        };

        //        var fileExt = Path.GetExtension(file.FileName);
        //        return allowedFileTypes.Contains(fileExt);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

    }

}