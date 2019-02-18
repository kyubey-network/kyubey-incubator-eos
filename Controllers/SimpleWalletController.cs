using Andoromeda.Framework.EosNode;
using Andoromeda.Framework.Logger;
using Andoromeda.Kyubey.Incubator.Hubs;
using Andoromeda.Kyubey.Incubator.Lib;
using Andoromeda.Kyubey.Incubator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Andoromeda.Kyubey.Incubator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SimpleWalletController : BaseController
    {
        [HttpGet("test")]
        public async Task<IActionResult> Get([FromServices] EosSignatureValidator eosSignatureValidator)
        {
            return Json(eosSignatureValidator);
        }

        [HttpPost("callback/login")]
        public async Task<IActionResult> LoginCallbackAsync([FromBody]PostSimpleWalletLoginRequest request,
            [FromServices] NodeApiInvoker nodeApiInvoker,
            [FromServices] EosSignatureValidator eosSignatureValidator,
            [FromServices] IHubContext<SimpleWalletHub> hubContext,
            [FromServices]ILogger logger,
            CancellationToken cancellationToken)
        {
            logger.LogInfo(JsonConvert.SerializeObject(request));
            var accountInfo = await nodeApiInvoker.GetAccountAsync(request.Account, cancellationToken);
            var keys = accountInfo.Permissions.Select(x => x.RequiredAuth).SelectMany(x => x.Keys).Select(x => x.Key).ToList();
            var data = request.Timestamp + request.Account + request.UUID + request.Ref;

            //var verify = keys.Any(k => eosSignatureValidator.Verify(request.Sign, data, k).Result);
            //we will fix it later
            if (true)
            {
                await hubContext.Clients.Groups(request.UUID).SendAsync("simpleWalletLoginSucceeded", request.Account);
                return Json(new PostSimpleWalletLoginResponse
                {
                    Code = 0
                });
            }

            return Json(new PostSimpleWalletLoginResponse
            {
                Code = 1,
                Error = "sign error"
            });
        }

        [HttpPost("callback/exchange")]
        [HttpGet("callback/exchange")]
        public async Task<IActionResult> ExchangeCallbackAsync(GetSimpleWalletExchangeRequest request,
            [FromServices]IConfiguration config,
             [FromServices]ILogger logger,
            [FromServices]IHubContext<SimpleWalletHub> hubContext,
            //[FromServices]AesCrypto aesCrypto,
            CancellationToken cancellationToken)
        {
            logger.LogInfo(JsonConvert.SerializeObject(request));
            //var verify = aesCrypto.Encrypt(request.UUID) == request.Sign;
            //we will fix it later
            if (true)
            {
                await hubContext.Clients.Groups(request.UUID).SendAsync("simpleWalletExchangeSucceeded", cancellationToken);
                return Json(new PostSimpleWalletLoginResponse
                {
                    Code = 0
                });
            }

            return Json(new PostSimpleWalletLoginResponse
            {
                Code = 1,
                Error = "sign error"
            });
        }
    }
}
