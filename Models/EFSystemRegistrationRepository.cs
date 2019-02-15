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
                db_entry = sysReg;
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

        public SystemRegistration MoveFirst() => context.SystemRegistrations.First();

        public SystemRegistration MovePrev(string current_cst_code, string current_sys_code, string current_ver_code)
        {
            if (string.IsNullOrWhiteSpace(current_cst_code) || 
                string.IsNullOrWhiteSpace(current_sys_code) ||
                string.IsNullOrWhiteSpace(current_ver_code) ||  
                string.Compare(context.SystemRegistrations.First().Syr_Cst_Code, current_cst_code) == 0 ||
                string.Compare(context.SystemRegistrations.First().Syr_Sys_Code, current_sys_code) == 0 ||
                string.Compare(context.SystemRegistrations.First().Syr_Ver_Code, current_ver_code) == 0)
                return context.SystemRegistrations.First();

            return context.SystemRegistrations.LastOrDefault(
                sys => string.Compare(sys.Syr_Cst_Code + sys.Syr_Sys_Code + sys.Syr_Ver_Code, 
                                      current_cst_code + current_sys_code + current_ver_code) < 0);
        }

        public SystemRegistration MoveLast() => context.SystemRegistrations.Last();

        public SystemRegistration MoveNext(string current_cst_code, string current_sys_code, string current_ver_code)
        {
            if (string.IsNullOrWhiteSpace(current_cst_code + current_sys_code + current_ver_code))
                return context.SystemRegistrations.First();

            if (string.Compare(context.SystemRegistrations.Last().Syr_Cst_Code +
                              context.SystemRegistrations.Last().Syr_Sys_Code +
                              context.SystemRegistrations.Last().Syr_Ver_Code,
                              current_cst_code + current_sys_code + current_ver_code) == 0)
                return context.SystemRegistrations.Last();

            return context.SystemRegistrations.FirstOrDefault(
                sys => string.Compare(sys.Syr_Cst_Code + sys.Syr_Sys_Code + sys.Syr_Ver_Code,
                                      current_cst_code + current_sys_code + current_ver_code) > 0);
        }

        public IQueryable<SystemRegistration> Search(string cst_code, string sys_code, string ver_code)
        {
            if (string.IsNullOrWhiteSpace(cst_code + sys_code + ver_code))
                return context.SystemRegistrations;
            return context.SystemRegistrations.Where(sys => sys.Syr_Cst_Code.Contains(cst_code) &&
                                                            sys.Syr_Sys_Code.Contains(sys_code) &&
                                                            sys.Syr_Ver_Code.Contains(ver_code));
        }
        
    }
}
