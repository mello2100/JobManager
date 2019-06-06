namespace JobManagerCore.Models
{
    public class JobExecutionResponse
    {
        public Job Job { get; }

        public JobExecutionResponseData JobExecutionResponseData { get; }

        public JobExecutionResponse(Job job, JobExecutionResponseData jobExecutionResponseData)
        {
            Job = job;
            JobExecutionResponseData = jobExecutionResponseData;
        }
    }
}