using System.Linq;

namespace RegistrationSystem.Models
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> Customers { get; }

        void SaveCustomer(Customer mod);
        Customer DeleteCustomer(string cst_code);
        Customer MoveFirst();
        Customer MoveNext(string current_cst_code);
        Customer MoveLast();
        Customer MovePrev(string current_cst_code);
    }
}
