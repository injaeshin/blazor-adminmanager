using System.Text;
using AdminManager.Common;
using Quartz;
using Quartz.Impl.Matchers;

namespace AdminManager.Service;

public class ScheduleService(ISchedulerFactory schedulerFactory)
{
    private readonly IScheduler _scheduler = schedulerFactory.GetScheduler().Result;

    public async Task CreateScheduleJobAsync<T>(string[] identity, JobDataMap jobData, ScheduleConfig setting) where T : IJob
    {
        var jobKey = new JobKey($"{identity[0]}-{identity[1]}", $"{identity[0]}-grp");
        var triggerKey = new TriggerKey($"{identity[0]}-{identity[1]}", $"{identity[0]}-trg");

        await _scheduler.UnscheduleJob(triggerKey);

        var job = JobBuilder.Create<T>()
            .WithIdentity(jobKey)
            .SetJobData(jobData)
            .Build();

        var simpleScheduleBuilder = SimpleScheduleBuilder.Create();

        simpleScheduleBuilder.WithRepeatCount(setting.RepeatCount);
        
        if (setting.IntervalSeconds > 0)
            simpleScheduleBuilder.WithIntervalInSeconds(setting.IntervalSeconds).RepeatForever();

        var triggerBuilder = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .WithSchedule(simpleScheduleBuilder);

        if (setting.IsImmediate)
        {
            triggerBuilder.StartNow();
        }
        else
        {
          triggerBuilder.StartAt(setting.BeginDateTime).EndAt(setting.EndDateTime);
        }

        await _scheduler.ScheduleJob(job, triggerBuilder.Build());
    }

    public async Task<bool> DeleteJob(int eventNo, string key)
    {
        var jobKey = new JobKey($"{key}-{eventNo}", $"{key}-grp");
        return await _scheduler.DeleteJob(jobKey);
    }

    public async Task<string> GetJobsAndTriggers(string key)
    {
        var sb = new StringBuilder();

        var jobKey = new JobKey(key, $"{key}-grp");
        var jobs = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupContains(jobKey.Group));
        foreach (var j in jobs)
        {
            sb.Append($"Job: {j.Name}").Append(" - ");
            sb.Append("Trigger: ");
            var triggers = await _scheduler.GetTriggersOfJob(j);
            foreach (var t in triggers)
            {
                sb.Append($"{t.Key}").Append(" ");
            }
        }

        if (sb.Length == 0)
            sb.Append("scheduler empty");

        return sb.ToString();
    }
}