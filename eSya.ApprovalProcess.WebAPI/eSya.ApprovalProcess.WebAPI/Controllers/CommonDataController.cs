using eSya.ApprovalProcess.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ApprovalProcess.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonDataController : ControllerBase
    {
        private readonly ICommonDataRepository _commondataRepository;
        public CommonDataController(ICommonDataRepository commondataRepository)
        {
            _commondataRepository = commondataRepository;
        } 
        /// <summary>
          /// Getting  Business Key.
          /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var ds = await _commondataRepository.GetBusinessKey();
            return Ok(ds);
        }
    }
}
