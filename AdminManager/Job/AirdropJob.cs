using AdminManager.Common;
using Quartz;
using AdminManager.Model;
using AdminManager.Service;

namespace AdminManager.Job;

public class AirdropJob(IRedisService redisService, EventService eventService) : Job, IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var airdrops = GetEventModels<AirdropEventModel>(context);
        if (airdrops == null)
        {
            Console.WriteLine($"{DateTime.Now} AirdropEvent: No airdrops");
            return Task.FromResult(false);
        }

        var airdrop = airdrops[0];
        Console.WriteLine($"{DateTime.Now} AirdropEvent: {airdrop.No}, {airdrop.EventGameType}, {airdrop.RequireAmount}");

        var requiredAmount = eventService.GetAirdropAmount(airdrop.EventGameType).Result;
        if (requiredAmount < airdrop.RequireAmount)
        {
            Console.WriteLine($"{DateTime.Now} AirdropEvent: {airdrop.No} failed. RequiredAmount: {requiredAmount}, AirdropAmount: {airdrop.RequireAmount}");
            eventService.SetCompleteAirdropEvent(airdrop.No, EventResultType.Fail);
            return Task.FromResult(false);
        }

        redisService.ReqAirdropEvent(airdrop.EventGameType, airdrop.No, airdrop.UseAmount);
        eventService.SetCompleteAirdropEvent(airdrop.No, EventResultType.Request);

        return Task.FromResult(true);
    }
}