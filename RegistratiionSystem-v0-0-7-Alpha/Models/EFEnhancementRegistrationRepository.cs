using System.Linq;
using System;

namespace RegistrationSystem.Models
{
    public class EFEnhancementRegistrationRepository : IEnhancementRegistrationReposity
    {
        private ApplicationDbContext context;
        public IQueryable<EnhancementRegistration> EnhancementRegistrations
            => context.EnhancementRegistrations;

        public EFEnhancementRegistrationRepository(ApplicationDbContext ctx) 
            => context = ctx;

        public void AddEnhancementRegistration(EnhancementRegistration er)
        {
            EnhancementRegistration db_entry = context.EnhancementRegistrations.Find(er.Enr_Cst_Code,
                                                                                     er.Enr_Sys_Code,
                                                                                     er.Enr_Ver_Code,
                                                                                     er.Enr_Mod_Code,
                                                                                     er.Enr_Enh_Code);
            if (db_entry == null)
            {
                er.Enr_CreatedDate = DateTime.Now;
                er.Enr_CreatedTime = DateTime.Now;
                context.EnhancementRegistrations.Add(er);
                context.SaveChanges();
            }
            else
            {
                db_entry.Enr_Eng_Password = er.Enr_Eng_Password;
                context.SaveChanges();
            }
        }

        public EnhancementRegistration DeleteEnhancementRegistration(string cst_code, 
                                                                     string sys_code,
                                                                     string ver_code, 
                                                                     string mod_code,
                                                                     string enh_code)
        {
            EnhancementRegistration db_entry = 
                context.EnhancementRegistrations.FirstOrDefault(r => r.Enr_Cst_Code == cst_code &&
                                                                     r.Enr_Sys_Code == sys_code &&
                                                                     r.Enr_Ver_Code == ver_code &&
                                                                     r.Enr_Mod_Code == mod_code &&
                                                                     r.Enr_Enh_Code == enh_code);
            if (db_entry != null)
            {
                context.EnhancementRegistrations.Remove(db_entry);
                context.SaveChanges();
            }

            return db_entry;
        }

        public IQueryable<EnhancementRegistration> Search(string cst_code, 
                                                          string sys_code,
                                                          string ver_code, 
                                                          string mod_code,
                                                          string enh_code)
        {
            if (string.IsNullOrWhiteSpace(cst_code + sys_code + ver_code + mod_code + enh_code))
                return context.EnhancementRegistrations;
            return context.EnhancementRegistrations.Where(r => r.Enr_Cst_Code.Contains(cst_code) &&
                                                               r.Enr_Sys_Code.Contains(sys_code) &&
                                                               r.Enr_Ver_Code.Contains(ver_code) &&
                                                               r.Enr_Mod_Code.Contains(mod_code) &&
                                                               r.Enr_Enh_Code.Contains(enh_code));
        }
    }
}
