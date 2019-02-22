using System.Linq;

namespace RegistrationSystem.Models
{
    public interface ISystemRegistrationRepository
    {
        IQueryable<SystemRegistration> SystemRegistrations { get; }

        void AddSystemRegistration(SystemRegistration sys);
        SystemRegistration DeleteSystemRegistration(string cst_code, string sys_code, string ver_code);
        SystemRegistration MoveFirst();
        SystemRegistration MoveNext(string current_cst_code, string current_sys_code, string current_ver_code);
        SystemRegistration MoveLast();
        SystemRegistration MovePrev(string current_cst_code, string current_sys_code, string current_ver_code);
        IQueryable<SystemRegistration> Search(string cst_code, string sys_code, string ver_code);
    }
}
