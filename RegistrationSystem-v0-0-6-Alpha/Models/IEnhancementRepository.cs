using System.Linq;

namespace RegistrationSystem.Models
{
    public interface IEnhancementRepository
    {
        IQueryable<Enhancement> Enhancements { get; }

        void SaveEnhancement(Enhancement enh);
        Enhancement DeleteEnhancement(string sys_code, string mod_code, string enh_code);
        Enhancement MoveFirst();
        Enhancement MovePrev(string current_enh_code, string current_mod_code, string current_sys_code);
        Enhancement MoveLast();
        Enhancement MoveNext(string current_sys_code, string current_mod_code, string current_enh_code);
    }
}
