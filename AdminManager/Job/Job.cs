using Quartz;

namespace AdminManager.Job
{
    public class Job
    {
        protected static List<T> GetEventModels<T>(IJobExecutionContext context) where T : class, new()
        {
            var models = new List<T>();

            var key = context.JobDetail.Key;
            var dataMap = context.JobDetail.JobDataMap;
            if (dataMap.Count == 0)
            {
                Console.WriteLine($"Job: {key.Name} - {key.Group} - JobDataMap is empty");
                return null;
            }

            foreach (KeyValuePair<string, object> pair in dataMap)
            {
                if (pair.Value is not T m)
                {
                    Console.WriteLine($"Job: {key.Name} - {key.Group} - {typeof(T).Name} is null");
                    return models;
                }

                models.Add(m);
            }

            return models;
        }
    }
}
