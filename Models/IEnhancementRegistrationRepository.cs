using System.Linq;

namespace RegistrationSystem.Models
{
    public interface IEnhancementRegistrationReposity
    {
        IQueryable<EnhancementRegistration> EnhancementRegistrations { get; }

        void AddEnhancementRegistration(EnhancementRegistration er);
        EnhancementRegistration DeleteEnhancementRegistration(string cst_code, string sys_code,
                                                              string ver_code, string mod_code,
                                                              string enh_code);
        IQueryable<EnhancementRegistration> Search(string cst_code, string sys_code,
                                                   string ver_code, string mod_code, string enh_code);
    }
}
