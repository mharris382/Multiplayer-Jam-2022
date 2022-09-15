using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Game.core
{
    public static class Logger
    {
        private static StreamWriter activeLogger;
        private static string filePath;
        public static void NewLogFile(string filename)
        {
            string path =Directory.GetCurrentDirectory();
            Debug.Log(Directory.GetParent(path));
            path = $"{Directory.GetParent(path)}\\Logs";
            Debug.Assert(Directory.Exists(path), $"Missing Logs Folder: {path}");
            if(activeLogger !=null)activeLogger.Close();
            
            Debug.Log($"Started Logging: {filename}");
            activeLogger = new StreamWriter(File.Create(filePath =$"{path}\\{filename}.txt"));
        }

        private static Queue<string> logQueue = new Queue<string>();

        private static Task running;
        public static void Log2(string msg)
        {
            logQueue.Enqueue(msg);
            if (running == null)
            {
                Log();
            }
        }
        private static async Task Log()
        {
            while (logQueue.Count > 0)
            {
                var next = logQueue.Dequeue();
                await activeLogger.WriteLineAsync(next);
            }
        }

        public static void FinishLogging()
        {
            if(activeLogger!=null)
            {
                activeLogger.Close();
                Debug.Log($"Finished Logging: {filePath}");
                //System.Diagnostics.Process.Start("notepad.exe", filePath);
                System.Diagnostics.Process.Start(filePath);
            }
            
        }
    }
}