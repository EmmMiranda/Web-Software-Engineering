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
                db_entry.Cst_Name = cst.Cst_Name;
                db_entry.Cst_Description = cst.Cst_Description;
                db_entry.Cst_AdressLine1 = cst.Cst_AdressLine1;
                db_entry.Cst_AdressLine2 = cst.Cst_AdressLine2;
                db_entry.Cst_AdressLine3 = cst.Cst_AdressLine3;
                db_entry.Cst_City = cst.Cst_City;
                db_entry.Cst_State = cst.Cst_State;
                db_entry.Cst_ZipCode = cst.Cst_ZipCode;
                db_entry.Cst_TelephoneNo = cst.Cst_TelephoneNo;
                db_entry.Cst_EmailAddress = cst.Cst_EmailAddress;
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

        public Customer MoveFirst() => context.Customers.FirstOrDefault();

        public Customer MovePrev(string current_cst_code)
        {
            var first_entry = context.Customers.FirstOrDefault();
            if ((string.IsNullOrWhiteSpace(current_cst_code)) ||
                (string.Compare(first_entry.Cst_Code, current_cst_code) == 0))
                return first_entry;

            return context.Customers.LastOrDefault(
                cst => string.Compare(cst.Cst_Code, current_cst_code) < 0);
        }

        public Customer MoveLast() => context.Customers.LastOrDefault();

        public Customer MoveNext(string current_cst_code)
        {
            if (string.IsNullOrWhiteSpace(current_cst_code))
                return context.Customers.FirstOrDefault();

            var last_entry = context.Customers.LastOrDefault();
            if (string.Compare(last_entry.Cst_Code, current_cst_code) == 0)
                return last_entry;

            return context.Customers.FirstOrDefault(
                cst => string.Compare(cst.Cst_Code, current_cst_code) > 0);
        }

        
    }
}
