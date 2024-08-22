using eSya.ApprovalProcess.DO;
using eSya.ApprovalProcess.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ApprovalProcess.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly IProcessRepository _ProcessRepository;

        public ProcessController(IProcessRepository ProcessRepository)
        {
            _ProcessRepository = ProcessRepository;
        }
        #region Approval Level Based
        /// <summary>
        /// Get Form Master List.
        /// UI Reffered - Approval Process, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetFormsForApproval()
        {
            var ds = await _ProcessRepository.GetFormsForApproval();
            return Ok(ds);
        }
        /// <summary>
        /// Get Approval Levels List.
        /// UI Reffered - Approval Process, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetApprovalLevelsbyCodeType(int codetype, int businesskey, int formId, int approvaltype)
        {
            var ds = await _ProcessRepository.GetApprovalLevelsbyCodeType(codetype, businesskey, formId, approvaltype);
            return Ok(ds);
        }
        /// <summary>
        /// Get Insert Or Update Approval Type.
        /// UI Reffered - Approval Process, 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateApprovalLevel(DO_ApprovalTypes obj)
        {
            var msg = await _ProcessRepository.InsertOrUpdateApprovalLevel(obj);
            return Ok(msg);
        }
        #endregion

        #region Approval Value Based
        /// <summary>
        /// Get Approval Values List.
        /// UI Reffered - Approval Process, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetApprovalValuesbyFormID(int businesskey, int formId)
        {
            var ds = await _ProcessRepository.GetApprovalValuesbyFormID(businesskey, formId);
            return Ok(ds);
        }
        /// <summary>
        /// Get Insert Or Update Approval Values.
        /// UI Reffered - Approval Process, 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateApprovalValueBased(DO_ApprovalTypes obj)
        {
            var msg = await _ProcessRepository.InsertOrUpdateApprovalValueBased(obj);
            return Ok(msg);
        }
        #endregion
    }
}
