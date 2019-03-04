using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RegistrationSystemApi.Controllers
{

    [Route("api/[controller]")]
    public class SystemRegistrationApiController : Controller
    {
        private IEnhancementRegistrationReposity enh_reg_repository;

        public SystemRegistrationApiController(IEnhancementRegistrationReposity ereg_repo)
             => enh_reg_repository = ereg_repo;

        [HttpGet]
        public IEnumerable<EnhancementRegistration> Get()
            => enh_reg_repository.EnhancementRegistrations;

        [HttpGet("{cst_code}/{sys_code}/ver_code/mod_code/")]
        public IEnumerable<EnhancementRegistration> Get(string cst_code,
                                                        string sys_code,
                                                        string ver_code,
                                                        string mod_code)
            => enh_reg_repository.EnhancementRegistrations.Where(r => r.Enr_Cst_Code == cst_code &&
                                                                      r.Enr_Sys_Code == sys_code &&
                                                                      r.Enr_Ver_Code == ver_code &&
                                                                      r.Enr_Mod_Code == mod_code);

        [HttpGet("{cst_code}/{sys_code}/ver_code/mod_code/enh_code")]
        public EnhancementRegistration Get(string cst_code,
                                           string sys_code,
                                           string ver_code,
                                           string mod_code,
                                           string enh_code)
            => enh_reg_repository.EnhancementRegistrations.FirstOrDefault(r => r.Enr_Cst_Code == cst_code &&
                                                                          r.Enr_Sys_Code == sys_code &&
                                                                          r.Enr_Ver_Code == ver_code &&
                                                                          r.Enr_Mod_Code == mod_code &&
                                                                          r.Enr_Enh_Code == enh_code);



    }
}
