﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;

namespace RegistrationSystem.Controllers
{
    public class SystemListingController : Controller
    {
        private ISystemRepository repository;

        public SystemListingController(ISystemRepository repo)
        {
            repository = repo;
        }

        public ViewResult Listing() => View("SystemListing",repository.Systems);

        public ViewResult Search(Models.System sys)
        {
            ViewBag.ControllerName = "System";
            if (string.IsNullOrWhiteSpace(sys.Sys_Code))
                return View(repository.Systems);
            return View(repository.Systems.Where(r => r.Sys_Code.Contains(sys.Sys_Code)));
        }

        [HttpGet]
        public RedirectToActionResult BackToSystem() =>
            RedirectToAction(nameof(SystemController.Index), nameof(System));

    }
}
