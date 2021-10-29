using System;
using System.IO;

namespace Codetest.Repository
{
    public class Logger
    {
        public static void WriteLog(string FunctionName, string ErrorMessage)
        {
            try
            {
                string LogPath = $"C:\\Logs\\Pizza Palace\\";

                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }

                string LogFileName = string.Format("{0}{1}.txt", LogPath, DateTime.Now.ToString("yyyy-MMM-dd"));
                File.AppendAllTextAsync(LogFileName, string.Format("{0} - {1} => {2}", DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss"), FunctionName, ErrorMessage) + Environment.NewLine);
            }
            catch
            {

            }
        }
    }
}
