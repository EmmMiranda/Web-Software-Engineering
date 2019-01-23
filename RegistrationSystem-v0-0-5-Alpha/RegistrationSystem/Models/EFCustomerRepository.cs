using System.Linq;
using System;

namespace RegistrationSystem.Models
{
    public class EFCustomerRepository : ICustomerRepository
    {
        private ApplicationDbContext context;
        public IQueryable<Customer> Customers => context.Customers;

        public EFCustomerRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public void SaveCustomer(Customer cst)
        {
            Customer db_entry = context.Customers.Find(cst.Cst_Code);
            if (db_entry == null)
            {
                cst.Cst_CreatedDate = DateTime.Now;
                cst.Cst_CreatedTime = DateTime.Now;
                context.Customers.Add(cst);
                context.SaveChanges();
            } else {
                db_entry = cst;
                context.SaveChanges();
            }
        }

        public Customer DeleteCustomer(string cst_code)
        {
            Customer db_entry = context.Customers.FirstOrDefault(r => r.Cst_Code == cst_code);
            if (db_entry != null)
            {
                context.Customers.Remove(db_entry);
                context.SaveChanges();
            }

            return db_entry;
        }

        public Customer MoveFirst() => context.Customers.First();

        public Customer MovePrev(string current_cst_code)
        {
            if ((string.IsNullOrWhiteSpace(current_cst_code)) ||
                (string.Compare(context.Customers.First().Cst_Code, current_cst_code) == 0))
                return context.Customers.First();

            return context.Customers.LastOrDefault(
                cst => string.Compare(cst.Cst_Code, current_cst_code) < 0);
        }

        public Customer MoveLast() => context.Customers.Last();

        public Customer MoveNext(string current_cst_code)
        {
            if (string.IsNullOrWhiteSpace(current_cst_code))
                return context.Customers.First();

            if (string.Compare(context.Customers.Last().Cst_Code, current_cst_code) == 0)
                return context.Customers.Last();

            return context.Customers.FirstOrDefault(
                cst => string.Compare(cst.Cst_Code, current_cst_code) > 0);
        }

        
    }
}
