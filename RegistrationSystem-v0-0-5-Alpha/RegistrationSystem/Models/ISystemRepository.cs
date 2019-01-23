using System.Linq;

namespace RegistrationSystem.Models
{
    public interface ISystemRepository
    {
        IQueryable<System> Systems { get; }

        void AddSystem(System sys);
        System DeleteSystem(string sys_code);
        System MoveFirst();
        System MoveNext(string current_sys_code);
        System MoveLast();
        System MovePrev(string current_sys_code);
    }
}
