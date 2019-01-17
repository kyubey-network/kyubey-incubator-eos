using Andoromeda.Kyubey.Incubator.Models;
using Andoromeda.Kyubey.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("list")]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<ProjectManifest>>), 200)]
        [ProducesResponseType(typeof(ApiResult), 404)]
        public async Task<IActionResult> HomeProjectList(
            GetProjectListRequest request,
            [FromServices] KyubeyContext db,
            [FromServices] TokenRepositoryFactory tokenRepositoryFactory,
            CancellationToken cancellationToken
            )
        {
            //var tokens = await db.Tokens.ToListAsync();
            var tokenRepository = await tokenRepositoryFactory.CreateAsync(request.Lang);
            var kbyInfo = tokenRepository.EnumerateAll();
            var ProjectManifests = new List<ProjectManifest>();

            kbyInfo = kbyInfo.Where(x => x?.Incubation != null).ToList().Skip(request.Skip).Take(request.Take);
            var dbIncubations = await db.Tokens.Where(x =>
                x.HasIncubation && kbyInfo.FirstOrDefault(t => x.Id == t.Id).Incubation != null &&
                x.Status == TokenStatus.Active).ToListAsync();

            switch (request.status)
            {
                case "not started":
                    kbyInfo = kbyInfo.Where(x => (x.Incubation.Begin_Time ?? DateTime.MinValue) > DateTime.Now).ToList();
                    break;
                case "in progress":
                    kbyInfo = kbyInfo.Where(x => (x.Incubation.Begin_Time ?? DateTime.MinValue) < DateTime.Now && x.Incubation.DeadLine > DateTime.Now).ToList();
                    break;
                case "it's over":
                    kbyInfo = kbyInfo.Where(x => x.Incubation.DeadLine < DateTime.Now).ToList();
                    break;
            }

            switch (request.ranking)
            {
                case "latest":
                    kbyInfo = kbyInfo.OrderByDescending(x => (x.Incubation.Begin_Time ?? DateTime.MinValue));
                    break;
                case "money":
                    kbyInfo = kbyInfo.OrderByDescending(x => dbIncubations.FirstOrDefault(s => s.Id == x.Id)?.Raised ?? 0);
                    break;
                case "comment":
                    //No comment system yet
                    break;
            }

            foreach (var x in kbyInfo)
            {
                ProjectManifests.Add(new ProjectManifest
                {
                    Id = x.Id,
                    Introduction = tokenRepository.GetTokenDescription(x.Id, request.Lang),
                    Start_Time = x.Incubation.Begin_Time ?? DateTime.MinValue,
                    DeadLine = x.Incubation.DeadLine,
                    Target_Amount = dbIncubations.FirstOrDefault(s => s.Id == x.Id)?.Raised ?? 0,
                    Target_Credits = x.Incubation.Goal
                });
            }
            return ApiResult(ProjectManifests);
        }
    }
}
