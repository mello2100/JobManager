namespace JobManagerCore.Models
{
    public class Job
    {
        public string JobId { get; }

        public string JobData { get; }

        public int TryExecutionCount { get; }

        public Job(string jobId, string jobData, int tryExecutionCount = 1)
        {
            JobId = jobId;
            JobData = jobData;
            TryExecutionCount = tryExecutionCount;
        }
    }
}