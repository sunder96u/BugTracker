﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult Error500()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Oops()
        {
            if (TempData["oopsMsg"] != null)
            {
                ViewBag.OopsMsg = TempData["oopsMsg"].ToString();
            }
            return View();
        }

        public ActionResult Oops2()
        {
            if (TempData["oopsMsg"] != null)
            {
                ViewBag.OopsMsg = TempData["oopsMsg"].ToString();
            }
            return View();
        }

        public ActionResult Oops3()
        {
            if (TempData["oopsMsg"] != null)
            {
                ViewBag.OopsMsg = TempData["oopsMsg"].ToString();
            }
            return View();
        }

        public ActionResult Oops4()
        {
            if (TempData["oopsMsg"] != null)
            {
                ViewBag.OopsMsg = TempData["oopsMsg"].ToString();
            }
            return View();
        }
    }
}