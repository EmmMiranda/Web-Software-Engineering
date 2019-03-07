using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RegistrationSystemApi.Controllers
{

    [Route("api/[controller]")]
    public class EnhancementRegistrationApiController : Controller
    {
        private IEnhancementRegistrationReposity enh_reg_repository;

        public EnhancementRegistrationApiController(IEnhancementRegistrationReposity ereg_repo)
             => enh_reg_repository = ereg_repo;

        [HttpGet]
        public IEnumerable<EnhancementRegistration> Get()
            => enh_reg_repository.EnhancementRegistrations;

        [HttpGet("{cst_code}/{sys_code}/{ver_code}/{mod_code}/")]
        public IEnumerable<EnhancementRegistration> Get(string cst_code,
                                                        string sys_code,
                                                        string ver_code,
                                                        string mod_code)
            => enh_reg_repository.EnhancementRegistrations.Where(r => r.Enr_Cst_Code == cst_code &&
                                                                      r.Enr_Sys_Code == sys_code &&
                                                                      r.Enr_Ver_Code == ver_code &&
                                                                      r.Enr_Mod_Code == mod_code);

        [HttpGet("{cst_code}/{sys_code}/{ver_code}/{mod_code}/{enh_code}")]
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

        [HttpPost]
        public EnhancementRegistration Post([FromBody]EnhancementRegistration enh_reg)
        {
            enh_reg_repository.AddEnhancementRegistration(enh_reg);
            return enh_reg_repository.EnhancementRegistrations.FirstOrDefault(r => r.Enr_Cst_Code == enh_reg.Enr_Cst_Code &&
                                                                                   r.Enr_Sys_Code == enh_reg.Enr_Sys_Code &&
                                                                                   r.Enr_Ver_Code == enh_reg.Enr_Ver_Code &&
                                                                                   r.Enr_Mod_Code == enh_reg.Enr_Mod_Code &&
                                                                                   r.Enr_Enh_Code == enh_reg.Enr_Enh_Code);
        }

        [HttpPut]
        public EnhancementRegistration Put([FromBody]EnhancementRegistration enh_reg)
        {
            enh_reg_repository.AddEnhancementRegistration(enh_reg);
            return enh_reg_repository.EnhancementRegistrations.FirstOrDefault(r => r.Enr_Cst_Code == enh_reg.Enr_Cst_Code &&
                                                                                   r.Enr_Sys_Code == enh_reg.Enr_Sys_Code &&
                                                                                   r.Enr_Ver_Code == enh_reg.Enr_Ver_Code &&
                                                                                   r.Enr_Mod_Code == enh_reg.Enr_Mod_Code &&
                                                                                   r.Enr_Enh_Code == enh_reg.Enr_Enh_Code);
        }

        [HttpPatch("{cst_code}/{sys_code}/{ver_code}/{mod_code}/{enh_code}")]
        public StatusCodeResult Patch(string cst_code,
                                      string sys_code,
                                      string ver_code,
                                      string mod_code,
                                      string enh_code,
                                      [FromBody]JsonPatchDocument<EnhancementRegistration> patch)
        {

            EnhancementRegistration enh_reg = Get(cst_code, sys_code, ver_code, mod_code, enh_code);
            if (enh_reg != null)
            {
                patch.ApplyTo(enh_reg);
                return Ok();
            }
            return NotFound();

        }

        [HttpDelete("{cst_code}/{sys_code}/{ver_code}/{mod_code}/{enh_code}")]
        public void Delete(string cst_code,
                           string sys_code,
                           string ver_code,
                           string mod_code,
                           string enh_code) => enh_reg_repository.DeleteEnhancementRegistration(cst_code,
                                                                                                sys_code,
                                                                                                ver_code,
                                                                                                mod_code,
                                                                                                enh_code);
    }
}