namespace DownloadOrganizer;
public class Worker(ILogger<Worker> logger) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;

    private int _filesMovedCount = 0;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var (isFolderValid, downloadFolderPath) = GetPcDownloadsFolder();
            if (!isFolderValid)
            {
                _logger.LogWarning("The folder is not valid.");
                return;
            }

            _filesMovedCount = 0; // Reset the amount of files moved for logging.

            var success = OrganizeFiles(downloadFolderPath);
            if (success)
                await WriteLogFileAsync($"{_filesMovedCount} Files organized successfully.");
            else
                await WriteLogFileAsync("Failed to organize files.");

            int dailyMilliseconds = 24 * 60 * 60 * 1000;
            await Task.Delay(dailyMilliseconds, stoppingToken);
        }
    }

    private async Task WriteLogFileAsync(string message)
    {
        var logFilePath = Path.Combine("C:\\", "DownloadOrganizerLog.txt");
        using (var streamWriter = new StreamWriter(logFilePath, true))
        {
            await streamWriter.WriteLineAsync($"{DateTime.Now}: {message}");
        }
    }

    private bool OrganizeFiles(string folderPath)
    {
        try
        {
            var files = Directory.GetFiles(folderPath);
            foreach (var file in files)
            {
                FileMovingProcess(file, folderPath);
            }
            _logger.LogInformation("Files organized successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while organizing files: {ex.Message}");
            return false;
        }
    }

    private void FileMovingProcess(string file, string folderPath)
    {
        var fileInfo = new FileInfo(file);
        var fileExtension = fileInfo.Extension.ToLower();
        var organizedFolderPath = Path.Combine(folderPath, "Organized", fileExtension.TrimStart('.'));
        Directory.CreateDirectory(organizedFolderPath);
        var destinationPath = Path.Combine(organizedFolderPath, fileInfo.Name);
        if (!File.Exists(destinationPath))
        {
            File.Move(file, destinationPath);
            _filesMovedCount++;
        }
    }

    private Tuple<bool, string> GetPcDownloadsFolder()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var downloadFolderPath = config["CustomConfig:DownloadFolderPath"];

        var currentUser = Environment.UserName;
        if (!Directory.Exists(downloadFolderPath))
        {
            downloadFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            downloadFolderPath = Path.Combine(downloadFolderPath, "Downloads");
        }

        if (Directory.Exists(downloadFolderPath))
            return Tuple.Create(true, downloadFolderPath);
        return Tuple.Create(false, string.Empty);
    }
}