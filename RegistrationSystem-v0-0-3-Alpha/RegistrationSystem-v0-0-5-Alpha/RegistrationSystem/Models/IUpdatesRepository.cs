using System.Linq;

namespace RegistrationSystem.Models
{
    public interface IUpdatesRepository
    {
        IQueryable<Updates> Updates { get; }

        void SaveUpdates(Updates upd);
        Updates DeleteUpdates(string current_sys_code, string current_ver_code,
                              string current_upd_code);
    }
}
