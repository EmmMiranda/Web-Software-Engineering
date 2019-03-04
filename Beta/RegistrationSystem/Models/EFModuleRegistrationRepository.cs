using System.Linq;
using System;

namespace RegistrationSystem.Models
{
    public class EFModuleRegistrationRepository : IModuleRegistrationRepository
    {
        private ApplicationDbContext context;
        public IQueryable<ModuleRegistration> ModuleRegistrations
            => context.ModuleRegistrations;

        public EFModuleRegistrationRepository(ApplicationDbContext ctx) 
            => context = ctx;

        public void AddModuleRegistration(ModuleRegistration mor)
        {
            ModuleRegistration db_entry = context.ModuleRegistrations.Find(mor.Mor_Cst_Code,
                                                                           mor.Mor_Sys_Code,
                                                                           mor.Mor_Ver_Code,
                                                                           mor.Mor_Mod_Code);
            if (db_entry == null)
            {
                mor.Mor_CreatedDate = DateTime.Now;
                mor.Mor_CreatedTime = DateTime.Now;
                context.ModuleRegistrations.Add(mor);
                context.SaveChanges();
            }
            else
            {
                db_entry = mor;
                context.SaveChanges();
            }
        }

        public ModuleRegistration DeleteModuleRegistration(string cst_code, 
                                                           string sys_code,
                                                           string ver_code, 
                                                           string mod_code)
        {
            ModuleRegistration db_entry = context.ModuleRegistrations.FirstOrDefault(r => r.Mor_Cst_Code == cst_code &&
                                                                                          r.Mor_Sys_Code == sys_code &&
                                                                                          r.Mor_Ver_Code == ver_code &&
                                                                                          r.Mor_Mod_Code == mod_code);
            if (db_entry != null)
            {
                context.ModuleRegistrations.Remove(db_entry);
                context.SaveChanges();
            }

            return db_entry;
        }

        public IQueryable<ModuleRegistration> Search(string cst_code, 
                                                     string sys_code,
                                                     string ver_code, 
                                                     string mod_code)
        {
            if (string.IsNullOrWhiteSpace(cst_code + sys_code + ver_code + mod_code))
                return context.ModuleRegistrations;
            return context.ModuleRegistrations.Where(r => r.Mor_Cst_Code.Contains(cst_code) &&
                                                          r.Mor_Sys_Code.Contains(sys_code) &&
                                                          r.Mor_Ver_Code.Contains(ver_code) &&
                                                          r.Mor_Mod_Code.Contains(mod_code));
        }
    }
}
