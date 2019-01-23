using System.Linq;

namespace RegistrationSystem.Models
{
    public interface IModuleRepository
    {
        IQueryable<Module> Modules { get; }

        void SaveModule(Module mod);
        Module DeleteModule(string mod_code, string sys_code);
        Module MoveFirst();
        Module MovePrev(string current_mod_code, string current_sys_code);
        Module MoveLast();
        Module MoveNext(string current_mod_code, string current_sys_code);
    }
}
