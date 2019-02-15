using System.Linq;
using System;

namespace RegistrationSystem.Models
{
    public class EFVersionRepository : IVersionRepository
    {
        private ApplicationDbContext context;
        public IQueryable<Version> Versions => context.Versions;

        public EFVersionRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public void SaveVersion(Version ver)
        {
            Version db_entry = context.Versions.Find(ver.Ver_Sys_Code, ver.Ver_Code);
            if (db_entry == null)
            {
                ver.Ver_CreatedDate = DateTime.Now;
                ver.Ver_CreatedTime = DateTime.Now;
                context.Versions.Add(ver);
                context.SaveChanges();
            } else {
                db_entry.Ver_Sys_Code = ver.Ver_Sys_Code;
                db_entry.Ver_Code = ver.Ver_Code;
                db_entry.Ver_Description = ver.Ver_Description;
                context.SaveChanges();
            }
        }

        public Version DeleteVersion(string sys_code, string ver_code)
        {
            Version db_entry = context.Versions.FirstOrDefault(v => v.Ver_Code == ver_code && 
                                                                   v.Ver_Sys_Code == sys_code);
            if (db_entry != null)
            {
                context.Versions.Remove(db_entry);
                context.SaveChanges();
            }

            return db_entry;
        }

        public Version MoveFirst() => context.Versions.First();

        public Version MovePrev(string current_sys_code, string current_ver_code)
        {
            if ((string.IsNullOrWhiteSpace(current_sys_code) && string.IsNullOrWhiteSpace(current_ver_code)) ||
                (string.Compare(context.Versions.First().Ver_Code, current_sys_code) == 0 && 
                 string.Compare(context.Versions.First().Ver_Sys_Code, current_ver_code) == 0))
                return context.Versions.First();

            return context.Versions.LastOrDefault(
                mod => string.Compare(mod.Ver_Sys_Code + mod.Ver_Code, 
                                      current_sys_code + current_ver_code) < 0);
        }

        public Version MoveLast() => context.Versions.Last();

        public Version MoveNext(string current_sys_code, string current_ver_code)
        {
            if (string.IsNullOrWhiteSpace(current_sys_code + current_ver_code))
                return context.Versions.First();

            if (string.Compare(context.Modules.Last().Mod_Code + context.Modules.Last().Mod_Sys_Code, 
                               current_sys_code + current_ver_code) == 0)
                return context.Versions.Last();

            return context.Versions.FirstOrDefault(
                mod => string.Compare(mod.Ver_Sys_Code + mod.Ver_Code, 
                                      current_sys_code + current_ver_code) > 0);
        }

        public IQueryable<Version> Search(string current_sys_code, string current_ver_code)
        {
            if (string.IsNullOrWhiteSpace(current_ver_code))
                return context.Versions.Where(ver => ver.Ver_Sys_Code == current_sys_code);
            return context.Versions.Where(ver => ver.Ver_Sys_Code == current_sys_code &&
                                                 ver.Ver_Code.Contains(current_ver_code));
        }
    }
}
