using System.Linq;

namespace RegistrationSystem.Models
{
    public interface IModuleRegistrationRepository
    {
        IQueryable<ModuleRegistration> ModuleRegistrations { get; }

        void AddModuleRegistration(ModuleRegistration mor);
        ModuleRegistration DeleteModuleRegistration(string cst_code, string sys_code, 
                                                    string ver_code, string mod_code);
        IQueryable<ModuleRegistration> Search(string cst_code, string sys_code, 
                                              string ver_code, string mod_code);
    }
}
