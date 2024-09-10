using eSya.ApprovalProcess.DL.Repository;
using eSya.ApprovalProcess.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ApprovalProcess.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApproverController : ControllerBase
    {
        private readonly IApproverRepository _approverRepository;

        public ApproverController(IApproverRepository approverRepository)
        {
            _approverRepository = approverRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetApprovedFormsbyBusinesskey(int Businesskey)
        {
            var ds = await _approverRepository.GetApprovedFormsbyBusinesskey(Businesskey);
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetApproverUsresbyBusinesskey(int Businesskey, int FormID)
        {
            var ds = await _approverRepository.GetApproverUsresbyBusinesskey(Businesskey, FormID);
            return Ok(ds);
        }
    }
}
