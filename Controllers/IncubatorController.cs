using Andoromeda.Kyubey.Incubator.Models;
using Andoromeda.Kyubey.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using static Andoromeda.Kyubey.Incubator.Repository.TokenRespository;

namespace Andoromeda.Kyubey.Incubator.Controllers
{
    [Route("api/v1/lang/{lang}/[controller]")]
    public class IncubatorController : BaseController
    {
        [HttpGet("tokens")]
        //[ProducesResponseType(typeof(ApiResult<IEnumerable<GetSlidesResponse>>), 200)]
        //[ProducesResponseType(typeof(ApiResult), 404)]
        public async Task<IActionResult> TokensAsync(
            GetPagingRequest request,
            [FromServices] KyubeyContext db,
            [FromServices] TokenRepositoryFactory tokenRepositoryFactory,
            CancellationToken cancellationToken
            )
        {
            var tokens = await db.Tokens.ToListAsync();
            var tokenRepository = await tokenRepositoryFactory.CreateAsync(request.Lang);
            var kbyInfo = tokenRepository.GetSingle("KBY");
            return ApiResult(kbyInfo);
        }
    }
}
