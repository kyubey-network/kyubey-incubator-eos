using Andoromeda.Framework.EosNode;
using Andoromeda.Framework.Logger;
using Andoromeda.Kyubey.Incubator.Models;
using Andoromeda.Kyubey.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Andoromeda.Kyubey.Incubator.Repository.TokenInfoRespository;

namespace Andoromeda.Kyubey.Incubator.Controllers
{
    [Route("api/v1/lang/{lang}/[controller]")]
    public class IncubatorController : BaseController
    {
        [HttpGet("list")]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<GetIncubatorPaginationResponse>>), 200)]
        [ProducesResponseType(typeof(ApiResult), 404)]
        public async Task<IActionResult> List(
            GetIncubatorListRequest request,
            [FromServices] KyubeyContext db,
            [FromServices] TokenRepositoryFactory tokenRepositoryFactory,
            CancellationToken cancellationToken
            )
        {
            var tokenRepository = await tokenRepositoryFactory.CreateAsync(request.Lang);


            var matchTokens = db.MatchReceipts.ToList().OrderByDescending(x => x.Time).GroupBy(x => x.TokenId).Select(x => new
            {
                Id = x.Key,
                Price = x.FirstOrDefault()?.UnitPrice
            }).ToList();
            var tokens = db.Tokens.OrderByDescending(x => x.Priority).ToList().Select(x => new
            {
                Symbol = x.Id,
                Contract = tokenRepository.GetSingle(x.Id).Basic.Contract?.Transfer,
                MaxPrice = (Math.Round((decimal)((x.NewDexAsk == 0 || x.NewDexAsk == null) ? matchTokens.FirstOrDefault(m => m.Id == x.Id)?.Price ?? 0 : x.NewDexAsk), 8) * 1.1m).ToString()
            }).ToList();

            var json = JsonConvert.SerializeObject(tokens);


            var tokenList = tokenRepository.EnumerateAll();
            var projectManifests = new GetIncubatorPaginationResponse();
            projectManifests.IncubatorList = new List<GetIncubatorListResponse>();

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

            projectManifests.Total = tokenList.Count();

            tokenList = tokenList.Skip(request.Skip).Take(request.Take);

            foreach (var x in tokenList)
            {
                projectManifests.IncubatorList.Add(new GetIncubatorListResponse
                {
                    Id = x.Id,
                    Introduction = tokenRepository.GetTokenDescription(x.Id, request.Lang),
                    StartTime = x.Incubation.Begin_Time ?? DateTime.MinValue,
                    DeadLine = x.Incubation.DeadLine,
                    TargetAmount = dbIncubations.FirstOrDefault(s => s.Id == x.Id)?.Raised ?? 0,
                    TargetCredits = x.Incubation.Goal
                });
            }
            return ApiResultOld(projectManifests);
        }

        [HttpGet("wording/{id}")]
        [ProducesResponseType(typeof(IEnumerable<GetWordingResponse>), 200)]
        public async Task<IActionResult> WordingAsync(
            string id,
            GetBaseRequest request,
            [FromServices] KyubeyContext db,
            [FromServices] TokenRepositoryFactory tokenRepositoryFactory,
            CancellationToken cancellationToken)
        {
            var dbToken = await db.Tokens.FirstOrDefaultAsync(x => x.Id == id && x.Status == TokenStatus.Active, cancellationToken);
            if (dbToken == null)
            {
                return ApiResult(404, "not found this token");
            }
            var tokenRepository = await tokenRepositoryFactory.CreateAsync(request.Lang);

            var banners = tokenRepository.GetTokenIncubationBannereRelativePaths(id, request.Lang).Select(x => $"/token_assets/" + x).ToList();

            var updates = tokenRepository.GetTokenIncubatorUpdates(id, request.Lang)?.OrderBy(x => x.Time)?.ToList();

            var response = new GetWordingResponse()
            {
                TokenId = id,
                Description = tokenRepository.GetTokenDescription(id, request.Lang),
                Detail = tokenRepository.GetTokenIncubationDetail(id, request.Lang),
                Updates = updates,
                Sliders = banners
            };

            return ApiResult(response);
        }

        [HttpGet("info/{id}")]
        [ProducesResponseType(typeof(IEnumerable<GetWordingResponse>), 200)]
        public async Task<IActionResult> InfoAsync(
            string id,
            [FromQuery] string username,
            GetBaseRequest request,
            [FromServices] KyubeyContext db,
            [FromServices] TokenRepositoryFactory tokenRepositoryFactory,
            [FromServices] NodeApiInvoker nodeApiInvoker,
            [FromServices] ILogger logger,
            CancellationToken cancellationToken)
        {
            var dbToken = await db.Tokens.FirstOrDefaultAsync(x => x.Id == id && x.Status == TokenStatus.Active, cancellationToken);
            if (dbToken == null)
            {
                return ApiResult(404, "not found this token");
            }

            var tokenRepository = await tokenRepositoryFactory.CreateAsync(request.Lang);
            var tokenInfo = tokenRepository.GetSingle(id);
            var supporterCount = await db.RaiseLogs.Where(x =>
                      (x.Timestamp > (tokenInfo.Incubation.Begin_Time ?? DateTime.MinValue)
                      && x.Timestamp < tokenInfo.Incubation.DeadLine) &&
                      x.TokenId == dbToken.Id && !x.Account.StartsWith("eosio.")).CountAsync();

            GetSymbolSupplyResponse symbolSupply = null;
            TokenContractPriceModel currentPrice = null;
            var eosBalance = 0.0;
            var tokenBalance = 0.0;
            try
            {
                symbolSupply = await nodeApiInvoker.GetSymbolSupplyAsync(tokenInfo?.Basic?.Contract?.Transfer, id, cancellationToken);
                currentPrice = await tokenRepository.GetContractPriceAsync(id);
                eosBalance = await nodeApiInvoker.GetCurrencyBalanceAsync(username, "eosio.token", "EOS", cancellationToken);
                tokenBalance = await nodeApiInvoker.GetCurrencyBalanceAsync(username, tokenInfo?.Basic?.Contract?.Transfer, id, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }



            var response = new GetIncubatorInfoResponse()
            {
                CurrentPrice = currentPrice?.BuyPrice ?? 0,
                EOSBalance = (decimal)eosBalance,
                TokenBalance = (decimal)tokenBalance,
                Contract = tokenInfo.Basic?.Contract?.Pricing ?? tokenInfo.Basic?.Contract?.Transfer,
                CurrentRaised = dbToken.Raised,
                IsFavorite = false,
                Protocol = tokenInfo.Basic?.Protocol,
                RemainingDay = tokenInfo?.Incubation?.DeadLine == null ? -999 : Math.Max((tokenInfo.Incubation.DeadLine - DateTime.Now).Days, 0),
                BeginTime = tokenInfo?.Incubation.Begin_Time,
                DeadLine = tokenInfo?.Incubation.DeadLine ?? DateTime.MaxValue,
                SupporterCount = supporterCount,
                Target = tokenInfo?.Incubation?.Goal ?? 0,
                TotalSupply = (decimal)(symbolSupply?.MaxSupply ?? 0)
            };

            return ApiResult(response);
        }
    }
}
