﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult CustomeError(string message)
        {
            ViewBag.message = message;

            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}