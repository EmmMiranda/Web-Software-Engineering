using System.Linq;

namespace RegistrationSystem.Models
{
    public interface IVersionRepository
    {
        IQueryable<Version> Versions { get; }

        void SaveVersion(Version mod);
        Version DeleteVersion(string current_sys_code, string current_ver_code);
        Version MoveFirst();
        Version MoveNext(string current_sys_code, string current_ver_code);
        Version MoveLast();
        Version MovePrev(string current_sys_code, string current_ver_code);
        IQueryable<Version> Search(string current_sys_code, string current_ver_code);
    }
}
