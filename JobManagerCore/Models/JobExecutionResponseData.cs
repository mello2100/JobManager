namespace JobManagerCore.Models
{
    public enum JobExecutionReponseStatus
    {
        OK = 0,
        GeneralError = 500,
    }

    public class JobExecutionResponseData
    {
        public string Message { get; }

        public string ExecutionResponseData { get; }

        public JobExecutionReponseStatus ExecutionStatus { get; }

        public JobExecutionResponseData(JobExecutionReponseStatus executionStatus, string executionResponseData, string message)
        {
            ExecutionStatus = executionStatus;
            ExecutionResponseData = executionResponseData;
            Message = message;
        }
    }
}