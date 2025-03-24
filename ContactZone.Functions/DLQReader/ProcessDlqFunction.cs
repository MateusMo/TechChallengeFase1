using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GetContacts.Functions
{
    public class ProcessDlqFunction
    {
        private readonly ILogger<ProcessDlqFunction> _logger;
        private readonly string _rabbitMqHost;
        private readonly int _rabbitMqPort;
        private readonly string _rabbitMqUsername;
        private readonly string _rabbitMqPassword;
        private readonly string _dlqName;
        private readonly string _rabbitMqConnection;

        public ProcessDlqFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ProcessDlqFunction>();

            _rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "";
            int.TryParse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"), out int port);
            _rabbitMqPort = port;
            _rabbitMqUsername = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "";
            _rabbitMqPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "";
            _dlqName = Environment.GetEnvironmentVariable("RABBITMQ_DLQ_NAME") ?? "";
            _rabbitMqConnection = $"amqp://{_rabbitMqUsername}:{_rabbitMqPassword}@{_rabbitMqHost}:{_rabbitMqPort}";
        }

        [Function("ProcessDlqMessages")]
        public async Task Run(
            [RabbitMQTrigger(
                queueName: "%RABBITMQ_DLQ_NAME%",
                ConnectionStringSetting = "RabbitMQConnection"
            )] string message,
            IDictionary<string, object> headers,
            FunctionContext context)
        {
            _logger.LogInformation($"DLQ message received at: {DateTime.Now}");
            _logger.LogInformation($"Using DLQ: {_dlqName}");
            _logger.LogInformation($"Using connection: {_rabbitMqConnection}");

            try
            {
                _logger.LogInformation($"Processing DLQ message: {message}");

                if (headers != null && headers.TryGetValue("x-death", out var xdeath))
                {
                    _logger.LogInformation($"Failure information: {xdeath}");
                }

                string processedMessage = $"PROCESSED: {message} - Timestamp: {DateTime.UtcNow}";

                await Task.Delay(1000);

                _logger.LogInformation($"Message processed successfully: {processedMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing DLQ message: {message}");
                throw;
            }
        }
    }
}
