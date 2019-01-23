using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;

namespace RegistrationSystem.Controllers
{
    public class VersionListingController : Controller
    {
        private IVersionRepository repository;

        public VersionListingController(IVersionRepository repo) => repository = repo;

        public ViewResult Listing(Models.Version ver)
        {
            if (!string.IsNullOrWhiteSpace(ver.Ver_Sys_Code) &&
                !string.IsNullOrWhiteSpace(ver.Ver_Code))
                return View("VersionListing", repository.Versions.Where(r => r.Ver_Sys_Code == ver.Ver_Sys_Code &&
                                                                             r.Ver_Code == ver.Ver_Code));
                
            return View("VersionListing", repository.Versions.Where(r => r.Ver_Sys_Code == ver.Ver_Sys_Code));
        }


        public ViewResult SearchVersion(Models.Version ver)
        {
            ViewBag.ControllerName = "Version";
            return View(repository.Versions.Where(r => r.Ver_Code.Contains(ver.Ver_Code) && 
                                                       r.Ver_Sys_Code == ver.Ver_Sys_Code));
        }

        [HttpGet]
        public RedirectToActionResult BackToVersion() =>
            RedirectToAction(nameof(VersionController.Index), "Version");

    }
}
