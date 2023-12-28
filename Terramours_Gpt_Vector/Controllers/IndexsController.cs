using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Terramours_Gpt_Vector.IService;
using Terramours_Gpt_Vector.Req;
using Terramours_Gpt_Vector.Res.Vector;

namespace Terramours_Gpt_Vector.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class IndexsController : ControllerBase
    {
        private readonly IVectorService _vectorService;

        public IndexsController(IVectorService vectorService)
        {
            _vectorService = vectorService;
        }
        
    }
}
