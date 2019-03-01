using System.Linq;
using System;

namespace RegistrationSystem.Models
{
    public class EFUpdatesRepository : IUpdatesRepository
    {
        private ApplicationDbContext context;
        public IQueryable<Updates> Updates => context.Updates;

        public EFUpdatesRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public void SaveUpdates(Updates upd)
        {
            Updates db_entry = context.Updates.Find(upd.Upd_Sys_Code, upd.Upd_Ver_Code, upd.Upd_Code);
            if (db_entry == null)
            {
                upd.Upd_CreatedDate = DateTime.Now;
                upd.Upd_CreatedTime = DateTime.Now;
                context.Updates.Add(upd);
                context.SaveChanges();
            } else {
                db_entry.Upd_Sys_Code = upd.Upd_Sys_Code;
                db_entry.Upd_Ver_Code = upd.Upd_Ver_Code;
                db_entry.Upd_Description = upd.Upd_Description;
                context.SaveChanges();
            }
        }

        public Updates DeleteUpdates(string sys_code, string ver_code, string upd_code)
        {
            Updates db_entry = context.Updates.FirstOrDefault(v => v.Upd_Code == upd_code && 
                                                                   v.Upd_Sys_Code == sys_code &&
                                                                   v.Upd_Ver_Code == ver_code);
            if (db_entry != null)
            {
                context.Updates.Remove(db_entry);
                context.SaveChanges();
            }

            return db_entry;
        }
    }
}
