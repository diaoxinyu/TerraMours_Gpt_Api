using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using System.Security.Claims;
using TerraMours_Gpt_Api.Domains.GptDomain.IServices;

namespace TerraMours_Gpt_Api.Domains.GptDomain.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class KnowledgeController : ControllerBase
    {
        private readonly IKnowledgeService _knowledgeService;

        public KnowledgeController(IKnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IResult> GetList()
        {
            var userId = long.Parse(HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res = await _knowledgeService.GetList(userId);
            return Results.Ok(res);
        }
    }
}
