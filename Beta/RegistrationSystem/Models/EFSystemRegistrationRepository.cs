using System.Linq;
using System;

namespace RegistrationSystem.Models
{
    public class EFSystemRegistrationRepository : ISystemRegistrationRepository
    {
        private ApplicationDbContext context;
        public IQueryable<SystemRegistration> SystemRegistrations => context.SystemRegistrations;

        public EFSystemRegistrationRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public void AddSystemRegistration(SystemRegistration sysReg)
        {
            SystemRegistration db_entry = context.SystemRegistrations.Find(sysReg.Syr_Cst_Code,
                                                                           sysReg.Syr_Sys_Code,
                                                                           sysReg.Syr_Ver_Code);
            if (db_entry == null)
            {
                sysReg.Syr_CreatedDate = DateTime.Now;
                sysReg.Syr_CreatedTime = DateTime.Now;
                context.SystemRegistrations.Add(sysReg);
                context.SaveChanges();
            } else {
                db_entry.Syr_SerialNum = sysReg.Syr_SerialNum;
                db_entry.Syr_CustomerKey = sysReg.Syr_CustomerKey;
                db_entry.Syr_UnlockKey = sysReg.Syr_UnlockKey;
                db_entry.Syr_ProdKey = sysReg.Syr_ProdKey;
                context.SaveChanges();
            }
        }

        public SystemRegistration DeleteSystemRegistration(string cst_code, string sys_code, string ver_code)
        {
            SystemRegistration db_entry = context.SystemRegistrations.FirstOrDefault(s => s.Syr_Cst_Code == cst_code &&
                                                                                          s.Syr_Sys_Code == sys_code &&
                                                                                          s.Syr_Ver_Code == ver_code);
            if (db_entry != null)
            {
                context.SystemRegistrations.Remove(db_entry);
                context.SaveChanges();
            }

            return db_entry;
        }

        public SystemRegistration MoveFirst() => context.SystemRegistrations.FirstOrDefault();

        public SystemRegistration MovePrev(string current_cst_code, string current_sys_code, string current_ver_code)
        {
            var first_entry = context.SystemRegistrations.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(current_cst_code) ||
                string.IsNullOrWhiteSpace(current_sys_code) ||
                string.IsNullOrWhiteSpace(current_ver_code) ||
                string.Compare(first_entry.Syr_Cst_Code, current_cst_code) == 0 &&
                string.Compare(first_entry.Syr_Sys_Code, current_sys_code) == 0 &&
                string.Compare(first_entry.Syr_Ver_Code, current_ver_code) == 0)
                return first_entry;

            return context.SystemRegistrations.LastOrDefault(
                sys => string.Compare(sys.Syr_Cst_Code + sys.Syr_Sys_Code + sys.Syr_Ver_Code,
                                      current_cst_code + current_sys_code + current_ver_code) < 0);
        }

        public SystemRegistration MoveLast() => context.SystemRegistrations.LastOrDefault();

        public SystemRegistration MoveNext(string current_cst_code, string current_sys_code, string current_ver_code)
        {

            if (string.IsNullOrWhiteSpace(current_cst_code) ||
                string.IsNullOrWhiteSpace(current_sys_code) ||
                string.IsNullOrWhiteSpace(current_ver_code))
                return context.SystemRegistrations.FirstOrDefault();

            var last_entry = context.SystemRegistrations.LastOrDefault();
            if (string.Compare(last_entry.Syr_Cst_Code + last_entry.Syr_Sys_Code + last_entry.Syr_Ver_Code,
                               current_cst_code + current_sys_code + current_ver_code) == 0)
                return last_entry;

            return context.SystemRegistrations.FirstOrDefault(
                sys => string.Compare(sys.Syr_Cst_Code + sys.Syr_Sys_Code + sys.Syr_Ver_Code,
                                      current_cst_code + current_sys_code + current_ver_code) > 0);
        }

        public IQueryable<SystemRegistration> Search(string cst_code, string sys_code, string ver_code)
        {
            string key = cst_code + sys_code + ver_code;
            if (string.IsNullOrWhiteSpace(key))
                return context.SystemRegistrations;
            return context.SystemRegistrations.Where(sys => (sys.Syr_Cst_Code +
                                                             sys.Syr_Sys_Code +
                                                             sys.Syr_Ver_Code)
                                                             .StartsWith(key));
        }

            /*
            return context.SystemRegistrations.Where(sys => sys.Syr_Cst_Code.Contains(cst_code) &&
                                                            sys.Syr_Sys_Code.Contains(sys_code) &&
                                                            sys.Syr_Ver_Code.Contains(ver_code));
                                                            */
        
        
    }
}
