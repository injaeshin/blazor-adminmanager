using AdminShared;
using AdminManager.Common;
using AdminManager.Model;
using AdminManager.Service;
using Quartz;

namespace AdminManager.Job;

public class MessageJob(IRedisService redisService) : Job, IJob
{
    public Task Execute(IJobExecutionContext context)
    { 
        var messages = GetEventModels<MessageModel>(context);
        if (messages == null)
        {
            Console.WriteLine($"{DateTime.Now} MessageEvent: No messages");
            return Task.FromResult(false);
        }

        Console.WriteLine($"{DateTime.Now} MessageEvent: {messages.Count} messages");
        
        var notifyMessages = new List<NotifyMessage>
        {
            Capacity = messages.Count
        };

        foreach (var message in messages)
        {
            notifyMessages.Add(new NotifyMessage
            {
                language = Convert.ToByte(message.Language),
                message = message.Message,
            });

            Console.WriteLine($"{DateTime.Now} Language: {message.Language.GetDescription()}, Message: {message.Message}");
        }

        redisService.ReqNotifyMessage(notifyMessages);
        Console.WriteLine($"{DateTime.Now} MessageEvent: {messages.Count} messages sent.");

        return Task.FromResult(true);
    }
}
