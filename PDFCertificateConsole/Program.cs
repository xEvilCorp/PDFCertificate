
using Microsoft.Extensions.Configuration;
using PDFCertificateConsole;

public class Program
{
    static string? INPUTS_FOLDER;
    static string? OUTPUTS_FOLDER;
    static int attemptCount = 0;
    static int intervalTimeMs = 1000;
    static int maxNumberRetries = 4;

    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Program started!");

            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();
            INPUTS_FOLDER = config["InputFolder"];
            OUTPUTS_FOLDER = config["OutputFolder"];

            WatchForNewFiles();
        }
        catch (Exception exception)
        {

            Console.WriteLine($"Error: {exception.Message}");
        }
       
    }

    static void WatchForNewFiles()
    {
        Util.CreateFolder(INPUTS_FOLDER);

        FileSystemWatcher watcher = new FileSystemWatcher();
        watcher.Path = INPUTS_FOLDER;
        watcher.NotifyFilter = NotifyFilters.FileName;
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
        watcher.Filter = "*.json";
        watcher.Created += new FileSystemEventHandler(OnFileAdded);

        Console.WriteLine($"Watching {INPUTS_FOLDER} for new files...");

        Util.Wait();
    }


    static void OnFileAdded(object sender, FileSystemEventArgs e)
    {
        try
        {
            Console.WriteLine($"Loading configuration:  {e.Name}");
            var configPath = e.FullPath;
            var config = Certificate.LoadConfig(configPath);
            var userId = new DirectoryInfo(configPath).Parent.Name;
            var pdfName = $"{config.fileId}.pdf";
            var pdfInputPath = Path.Combine(INPUTS_FOLDER, userId, pdfName);

            Console.WriteLine($"File {pdfName} added.");

            var outputPath = Path.Combine(OUTPUTS_FOLDER, userId, pdfName);
            var certificate = Certificate.LoadCertificate(config.certificateName, config.certificatePassword);

            Signer.Sign(certificate, pdfInputPath, outputPath);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error: {exception.Message}.");
            attemptCount += 1;
            Console.WriteLine($"Trying again in {intervalTimeMs / 1000f} seconds... {attemptCount}");
            Thread.Sleep(intervalTimeMs);
            if (attemptCount < maxNumberRetries) OnFileAdded(sender, e);
            else Console.WriteLine("Maximum number of retry attempts achieved.");
        }
      
    }

}




