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
        [HttpGet("list")]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<GetIncubatorListResponse>>), 200)]
        [ProducesResponseType(typeof(ApiResult), 404)]
        public async Task<IActionResult> List(
            GetIncubatorListRequest request,
            [FromServices] KyubeyContext db,
            [FromServices] TokenRepositoryFactory tokenRepositoryFactory,
            CancellationToken cancellationToken
            )
        {
            var tokenRepository = await tokenRepositoryFactory.CreateAsync(request.Lang);
            var tokenList = tokenRepository.EnumerateAll();
            var projectManifests = new List<GetIncubatorListResponse>();

            tokenList = tokenList.Where(x => x?.Incubation != null).ToList();
            var dbIncubations = await db.Tokens.Where(x =>
                x.HasIncubation && tokenList.FirstOrDefault(t => x.Id == t.Id).Incubation != null &&
                x.Status == TokenStatus.Active).ToListAsync(cancellationToken);

            switch (request.Status)
            {
                case "not_started":
                    tokenList = tokenList.Where(x => (x.Incubation.Begin_Time ?? DateTime.MinValue) > DateTime.Now).ToList();
                    break;
                case "in_progress":
                    tokenList = tokenList.Where(x => (x.Incubation.Begin_Time ?? DateTime.MinValue) < DateTime.Now && x.Incubation.DeadLine > DateTime.Now).ToList();
                    break;
                case "over":
                    tokenList = tokenList.Where(x => x.Incubation.DeadLine < DateTime.Now).ToList();
                    break;
            }

            switch (request.Ranking)
            {
                case "latest":
                    tokenList = tokenList.OrderByDescending(x => (x.Incubation.Begin_Time ?? DateTime.MinValue));
                    break;
                case "money":
                    tokenList = tokenList.OrderByDescending(x => dbIncubations.FirstOrDefault(s => s.Id == x.Id)?.Raised ?? 0);
                    break;
            }

            tokenList = tokenList.Skip(request.Skip).Take(request.Take);

            foreach (var x in tokenList)
            {
                projectManifests.Add(new GetIncubatorListResponse
                {
                    Id = x.Id,
                    Introduction = tokenRepository.GetTokenDescription(x.Id, request.Lang),
                    StartTime = x.Incubation.Begin_Time ?? DateTime.MinValue,
                    DeadLine = x.Incubation.DeadLine,
                    TargetAmount = dbIncubations.FirstOrDefault(s => s.Id == x.Id)?.Raised ?? 0,
                    TargetCredits = x.Incubation.Goal
                });
            }
            return ApiResult(projectManifests);
        }

        [HttpGet("list/total")]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<GetIncubatorQuantityResponse>>), 200)]
        [ProducesResponseType(typeof(ApiResult), 404)]
        public async Task<IActionResult> ListTotal(
            GetIncubatorListRequest request,
            [FromServices] KyubeyContext db,
            [FromServices] TokenRepositoryFactory tokenRepositoryFactory,
            CancellationToken cancellationToken
            )
        {
            var tokenRepository = await tokenRepositoryFactory.CreateAsync(request.Lang);
            var tokenList = tokenRepository.EnumerateAll();
            var incubatorList = new GetIncubatorQuantityResponse();

            tokenList = tokenList.Where(x => x?.Incubation != null).ToList();
            var dbIncubations = await db.Tokens.Where(x =>
                x.HasIncubation && tokenList.FirstOrDefault(t => x.Id == t.Id).Incubation != null &&
                x.Status == TokenStatus.Active).ToListAsync(cancellationToken);

            switch (request.Status)
            {
                case "not_started":
                    tokenList = tokenList.Where(x => (x.Incubation.Begin_Time ?? DateTime.MinValue) > DateTime.Now).ToList();
                    break;
                case "in_progress":
                    tokenList = tokenList.Where(x => (x.Incubation.Begin_Time ?? DateTime.MinValue) < DateTime.Now && x.Incubation.DeadLine > DateTime.Now).ToList();
                    break;
                case "over":
                    tokenList = tokenList.Where(x => x.Incubation.DeadLine < DateTime.Now).ToList();
                    break;
            }
            incubatorList.Total = tokenList.Count();
            return ApiResult(incubatorList);
        }
    }
}
