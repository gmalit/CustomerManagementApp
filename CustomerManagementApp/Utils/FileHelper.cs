using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CustomerManagementApp.Utils
{
    public static class FileHelper
    {
        public static void ExportToJson(List<Customer> customers, string filePath)
        {
            try
            {
                // Ensure the directory exists
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    LoggerHelper.Info($"Created directory: {directory}");
                }

                // Write the JSON to the full file path provided (no need to append a filename)
                File.WriteAllText(filePath, JsonConvert.SerializeObject(customers, Formatting.Indented));

                LoggerHelper.Info($"Successfully exported JSON to: {filePath}");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Failed to export JSON.", ex);
            }
        }
    }
}
