﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationSystem.Models
{
    public class EFEnhancementRepository : IEnhancementRepository
    {
        private ApplicationDbContext context;
        public IQueryable<Enhancement> Enhancements => context.Enhancements;

        public EFEnhancementRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public void SaveEnhancement(Enhancement enh)
        {
            var db_entry = context.Enhancements.Find(enh.Enh_Sys_Code,
                                                     enh.Enh_Mod_Code, enh.Enh_Code);
            if (db_entry == null)
            {
                enh.Enh_CreatedDate = DateTime.Now;
                enh.Enh_CreatedTime = DateTime.Now;
                context.Enhancements.Add(enh);
                context.SaveChanges();
            } else
            {
                db_entry.Enh_Name = enh.Enh_Name;
                db_entry.Enh_Description = enh.Enh_Description;
                db_entry.Enh_Version = enh.Enh_Version;
                context.SaveChanges();
            }
        }

        public Enhancement DeleteEnhancement(string sys_code, string mod_code, string enh_code)
        {
            var db_entry = context.Enhancements.FirstOrDefault(r => r.Enh_Sys_Code == sys_code &&
                                                                    r.Enh_Mod_Code == mod_code &&
                                                                    r.Enh_Code == enh_code);
            if (db_entry != null)
            {
                context.Enhancements.Remove(db_entry);
                context.SaveChanges();
            }

            return db_entry;
        }

        public Enhancement MoveFirst() => context.Enhancements.FirstOrDefault();

        public Enhancement MovePrev(string current_enh_code, string current_mod_code, string current_sys_code)
        {
            var first_entry = context.Enhancements.FirstOrDefault();

            if ((string.IsNullOrWhiteSpace(current_sys_code) ||
                 string.IsNullOrWhiteSpace(current_mod_code)) ||
                (string.Compare(first_entry.Enh_Sys_Code, current_sys_code) == 0 ||
                 string.Compare(first_entry.Enh_Mod_Code, current_mod_code) == 0))
                return first_entry;

            return context.Enhancements.LastOrDefault(
                enh => string.Compare(enh.Enh_Sys_Code + enh.Enh_Mod_Code,
                                      current_sys_code + current_mod_code) < 0);
        }

        public Enhancement MoveLast() => context.Enhancements.LastOrDefault();

        public Enhancement MoveNext(string current_sys_code, string current_mod_code, string current_enh_code)
        {
            if (string.IsNullOrWhiteSpace(current_sys_code) ||
                string.IsNullOrWhiteSpace(current_mod_code))
                return context.Enhancements.FirstOrDefault();

            var last_entry = context.Enhancements.LastOrDefault();
            if (string.Compare(last_entry.Enh_Sys_Code + last_entry.Enh_Mod_Code,
                               current_sys_code + current_mod_code) == 0)
                return last_entry;

            return context.Enhancements.FirstOrDefault(
                enh => string.Compare(enh.Enh_Sys_Code + enh.Enh_Mod_Code,
                                      current_sys_code + current_mod_code) > 0);
        }

    }
}
