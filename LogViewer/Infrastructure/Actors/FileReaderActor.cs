using LogViewer.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LogViewer.Infrastructure
{
    public class FileReaderActor : AbstractActor<LogFile>
    {

        public FileReaderActor()
        {
           
        }

        public override int ThreadsCount => 10;

        public override async Task HandleError(LogFile message, Exception ex)
        {
            Debug.WriteLine($"Errorr to read File:{message.FileInfo.Name} with error {ex.Message}");

            await Task.Delay(1000);

            await SendAsync(message);
        }

        public override async Task HandleMessage(LogFile message)
        {
            await message.ReadFileContentAsync();
        }
    }
}
