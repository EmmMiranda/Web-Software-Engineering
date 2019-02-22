using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace RegistrationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private ICustomerRepository repository;

        public CustomerController(ICustomerRepository repo) {
            repository = repo;
        }

        public IActionResult Customer() => View();

        [HttpGet]
        public RedirectToActionResult CustomerForm() => RedirectToAction("Customer");

        [HttpPost]
        public ViewResult CustomerForm(RegistrationSystem.Models.Customer cst)
        {
            if (ModelState.IsValid)
            {
                repository.SaveCustomer(cst);
                ModelState.Clear();
            }

            // if model state is not valid present error msg
            return View(nameof(CustomerController.Customer));
        }

        public ViewResult SearchCustomer(Models.Customer cst)
        {
            ViewBag.ControllerName = "Customer";
            if (string.IsNullOrWhiteSpace(cst.Cst_Code))
                return View(repository.Customers);
            return View(repository.Customers.Where(r => r.Cst_Code.Contains(cst.Cst_Code)));
        }

        public ViewResult SelectCustomer(Models.Customer cst) => View(nameof(CustomerController.Customer),
                                                                      new Models.Customer {Cst_Code = cst.Cst_Code});

        [HttpPost]
        public RedirectToActionResult Delete(Models.Customer cst)
        {
            if (ModelState.IsValid)
            {
                repository.DeleteCustomer(cst.Cst_Code);
            }

            // ModelState.Clear();
            return RedirectToAction(nameof(Customer));
        }

        [HttpGet]
        public IActionResult GetFirst()
        {
            ModelState.Clear();
            return View(nameof(CustomerController.Customer), repository.MoveFirst());
        }

        [HttpGet]
        public ViewResult GetPrev(Models.Customer cst)
        {
            ModelState.Clear();
            return View(nameof(CustomerController.Customer), repository.MovePrev(cst.Cst_Code));
        }

        [HttpGet]
        public ActionResult GetNext(Models.Customer cst)
        {
            ModelState.Clear();
            return View(nameof(CustomerController.Customer), repository.MoveNext(cst.Cst_Code));
        }

        [HttpGet]
        public ViewResult GetLast()
        {
            ModelState.Clear();
            return View(nameof(CustomerController.Customer), repository.MoveLast());
        }
    }
}