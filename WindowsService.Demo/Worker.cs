namespace WindowsService.Demo
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private const string FolderToWatch = @"C:\WatchFolder";
        private const int DelayInMilliseconds = 10000;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!Directory.Exists(FolderToWatch)) Directory.CreateDirectory(FolderToWatch);

            while (!stoppingToken.IsCancellationRequested)
            {
                var files = Directory.GetFiles(FolderToWatch);

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < DateTime.Now.AddMinutes(-1))
                    {
                        _logger.LogInformation($"Deleting {fileInfo.Name}");
                        fileInfo.Delete();
                    }
                }

                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(DelayInMilliseconds, stoppingToken);
            }
        }
    }
}