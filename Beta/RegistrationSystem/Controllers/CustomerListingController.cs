using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace RegistrationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerListingController : Controller
    {
        private ICustomerRepository repository;

        public CustomerListingController(ICustomerRepository repo) => repository = repo;

        public ViewResult Listing() => View("CustomerListing", repository.Customers);

        /*
        public ViewResult Listing(Models.Customer cst) =>
            View("CustomerListing", repository.Customers.Where(f => f.Cst_Code == cst.Cst_Code));
            */
        public ViewResult SearchCustomer(Models.Customer cst)
        {
            ViewBag.ControllerName = "Customer";
            return View(repository.Customers.Where(r => r.Cst_Code.Contains(cst.Cst_Code)));
        }

        [HttpGet]
        public RedirectToActionResult BackToCustomer() =>
            RedirectToAction(nameof(CustomerController.Customer), "Customer");

    }
}
