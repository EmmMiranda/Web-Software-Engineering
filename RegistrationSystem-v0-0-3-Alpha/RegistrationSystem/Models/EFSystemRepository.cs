using System.Collections.Generic;
using System.Linq;

namespace RegistrationSystem.Models
{
    public class EFSystemRepository : ISystemRepository
    {
        private ApplicationDbContext context;
        public IQueryable<System> Systems => context.Systems;

        public EFSystemRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public void AddSystem(System sys)
        {
            System db_entry_sys = context.Systems.Find(sys.Sys_Code);
            if (db_entry_sys == null)
            {
                context.Systems.Add(sys);
                context.SaveChanges();
            }
        }

        public System DeleteSystem(string sys_code)
        {
            System db_entry_sys = context.Systems.FirstOrDefault(s => s.Sys_Code == sys_code);
            if (db_entry_sys != null)
            {
                context.Systems.Remove(db_entry_sys);
                context.SaveChanges();
            }

            return db_entry_sys;
        }

        public System MoveFirst() => context.Systems.First();

        public System MovePrev(string current_sys_code)
        {
            if (string.IsNullOrWhiteSpace(current_sys_code) ||
                string.Compare(context.Systems.First().Sys_Code, current_sys_code) == 0)
                return context.Systems.First();

            return context.Systems.LastOrDefault(
                sys => string.Compare(sys.Sys_Code, current_sys_code) < 0);
        }

        public System MoveLast() => context.Systems.Last();

        public System MoveNext(string current_sys_code)
        {
            if (string.IsNullOrWhiteSpace(current_sys_code))
                return context.Systems.First();

            if(string.Compare(context.Systems.Last().Sys_Code, current_sys_code) == 0)
                return context.Systems.Last();

            return context.Systems.FirstOrDefault(
                sys => string.Compare(sys.Sys_Code, current_sys_code) > 0);
        }

        
    }
}
