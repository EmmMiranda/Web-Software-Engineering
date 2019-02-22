using System.Linq;
using System;

namespace RegistrationSystem.Models
{
    public class EFModuleRepository : IModuleRepository
    {
        private ApplicationDbContext context;
        public IQueryable<Module> Modules => context.Modules;

        public EFModuleRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public void SaveModule(Module mod)
        {
            Module db_entry_mod = context.Modules.Find(mod.Mod_Code, mod.Mod_Sys_Code);
            if (db_entry_mod == null)
            {
                mod.Mod_CreatedDate = DateTime.Now;
                mod.Mod_CreatedTime = DateTime.Now;
                context.Modules.Add(mod);
                context.SaveChanges();
            } else {
                db_entry_mod.Mod_Code = mod.Mod_Code;
                db_entry_mod.Mod_Sys_Code = mod.Mod_Sys_Code;
                db_entry_mod.Mod_Name = mod.Mod_Name;
                db_entry_mod.Mod_Description = mod.Mod_Description;
                db_entry_mod.Mod_Version = mod.Mod_Version;
                context.SaveChanges();
            }
        }

        public Module DeleteModule(string mod_code, string sys_code)
        {
            Module db_entry_mod = context.Modules.FirstOrDefault(m => m.Mod_Code == mod_code && 
                                                                      m.Mod_Sys_Code == sys_code);
            if (db_entry_mod != null)
            {
                context.Modules.Remove(db_entry_mod);
                context.SaveChanges();
            }

            return db_entry_mod;
        }

        public Module MoveFirst() => context.Modules.First();

        public Module MovePrev(string current_mod_code, string current_sys_code)
        {
            if ((string.IsNullOrWhiteSpace(current_mod_code) && string.IsNullOrWhiteSpace(current_sys_code)) ||
                (string.Compare(context.Modules.First().Mod_Code, current_mod_code) == 0 && 
                 string.Compare(context.Modules.First().Mod_Sys_Code, current_sys_code) == 0))
                return context.Modules.First();

            return context.Modules.LastOrDefault(
                mod => string.Compare(mod.Mod_Code + mod.Mod_Sys_Code, 
                                      current_mod_code + current_sys_code) < 0);
        }

        public Module MoveLast() => context.Modules.Last();

        public Module MoveNext(string current_mod_code, string current_sys_code)
        {
            if (string.IsNullOrWhiteSpace(current_mod_code + current_sys_code))
                return context.Modules.First();

            if (string.Compare(context.Modules.Last().Mod_Code + context.Modules.Last().Mod_Sys_Code, 
                               current_mod_code + current_sys_code) == 0)
                return context.Modules.Last();

            return context.Modules.FirstOrDefault(
                mod => string.Compare(mod.Mod_Code + mod.Mod_Sys_Code, 
                                      current_mod_code + current_sys_code) > 0);
        }

        public IQueryable<Module> Search(string current_sys_code, string current_mod_code)
        {
            if (string.IsNullOrWhiteSpace(current_mod_code))
                return context.Modules.Where(mod => mod.Mod_Sys_Code == current_sys_code);
            return context.Modules.Where(mod => mod.Mod_Sys_Code == current_sys_code &&
                                                mod.Mod_Code.Contains(current_mod_code));
        }
    }
}
