using Andoromeda.Framework.EosNode;
using Andoromeda.Framework.Logger;
using Andoromeda.Kyubey.Incubator.Extensions;
using Andoromeda.Kyubey.Models;
using Newtonsoft.Json;
using Pomelo.AspNetCore.TimedJob;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Andoromeda.Kyubey.Incubator.Jobs
{
    public class RaisedJob : Job
    {
        [Invoke(Begin = "2018-11-01 0:00", Interval = 500, SkipWhileExecuting = true)]
        public void GetRaised(KyubeyContext db, ILogger logger, NodeApiInvoker nodeApiInvoker, TokenRepositoryFactory tokenRepositoryFactory)
        {
            try
            {
                var tokenRepository = tokenRepositoryFactory.CreateAsync("en").Result;
                foreach (var x in db.Tokens
                .Where(x => x.HasIncubation && x.HasContractExchange)
                .ToList())
                {
                    var token = tokenRepository.GetSingle(x.Id);

                    //if (token.Incubation.Begin_Time.HasValue && DateTime.UtcNow < TimeZoneInfo.ConvertTimeToUtc(token.Incubation.Begin_Time.Value))
                    //{
                    //    continue;
                    //}

                    //if (DateTime.UtcNow > TimeZoneInfo.ConvertTimeToUtc(token.Incubation.DeadLine))
                    //{
                    //    continue;
                    //}

                    var depot = token.Basic.Contract.Depot ?? token.Basic.Contract.Pricing ?? token.Basic.Contract.Transfer;

                    if (string.IsNullOrWhiteSpace(depot))
                        continue;

                    var seq = GetOrCreateRaisedSeq(token.Id, db);
                    logger.LogInfo($"Current incubator {token.Id} seq is " + seq);

                    var ret = nodeApiInvoker.GetActionsAsync(depot, seq).Result;
                    if (ret.actions.Count() == 0)
                    {
                        logger.LogInfo($"No new action in incubator {token.Id}");
                    }

                    if (ret.actions != null)
                    {
                        foreach (var act in ret.actions)
                        {
                            try
                            {
                                logger.LogInfo($"Handling incubator {token.Id} action log pos={act.account_action_seq}, act={act.action_trace.act.name}");

                                switch (act.action_trace.act.name)
                                {
                                    case "transfer":
                                        {
                                            var transferData = act.action_trace.act.data;

                                            if ((string)transferData.memo == token.Basic.Contract_Exchange_Info.Buy_Memo
                                                && ((string)transferData.quantity).GetTokenAssetType() == "EOS")
                                            {
                                                var quantity = ((string)transferData.quantity).GetTokenAssetValue();
                                                HandleRaiseLogAsync(db, (int)act.account_action_seq, token.Id, depot, (string)transferData.from, (string)transferData.to, quantity, act.action_trace.block_time).Wait();
                                                HandleRaiseCountAsync(db, token.Id, TimeZoneInfo.ConvertTimeToUtc(token.Incubation.Begin_Time ?? DateTime.MinValue), TimeZoneInfo.ConvertTimeToUtc(token.Incubation.DeadLine)).Wait();
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                        break;
                                    default:
                                        continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.LogError($"incubator token: {token.Id} req: {seq} error: {token.Id} {seq} {ex.ToString()}");
                            }
                        }
                    }

                    seq += ret.actions.Count();
                    UpdateRaisedSeq(token.Id, seq, db);
                }
            }
            catch (Exception e)
            {
                logger.LogError("Sync Exception:" + JsonConvert.SerializeObject(e));
            }
        }

        int GetOrCreateRaisedSeq(string token, KyubeyContext db)
        {
            var rowId = $"incubator_{token}_seq";
            var seqRow = db.Constants.Where(x => x.Id == rowId).FirstOrDefault();
            if (seqRow == null)
            {
                var newRow = new Constant()
                {
                    Id = rowId,
                    Value = "0"
                };

                db.Constants.Add(newRow);
                db.SaveChanges();
                return 0;
            }
            return Convert.ToInt32(seqRow.Value);
        }

        void UpdateRaisedSeq(string token, int seq, KyubeyContext db)
        {
            var rowId = $"incubator_{token}_seq";
            var row = db.Constants.FirstOrDefault(x => x.Id == rowId);
            row.Value = seq.ToString();
            db.SaveChanges();
        }

        async Task HandleRaiseLogAsync(KyubeyContext db, int seq, string token, string account, string from, string to, decimal quantity, DateTime time)
        {
            var row = db.RaiseLogs.Where(x => x.TokenId == token && x.Seq == seq).FirstOrDefault();
            if (row == null && from != account && to == account)
            {
                db.RaiseLogs.Add(new RaiseLog
                {
                    Account = from,
                    Amount = (double)quantity,
                    Timestamp = time,
                    TokenId = token,
                    Seq = seq
                });
                await db.SaveChangesAsync();
            }
        }

        async Task HandleRaiseCountAsync(KyubeyContext db, string token, DateTime startTime, DateTime endTime)
        {
            var currentRaisedCount = db.RaiseLogs.Where(x => x.TokenId == token && x.Timestamp >= startTime && x.Timestamp <= endTime).Select(x => x.Amount).Sum();
            var row = db.Tokens.FirstOrDefault(x => x.Id == token);
            if (row != null)
            {
                row.Raised = (decimal)currentRaisedCount;
                await db.SaveChangesAsync();
            }
        }
    }
}
