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
                var directory = Path.GetDirectoryName(filePath);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    LoggerHelper.Info($"Created directory: {directory}");
                }

                string fullPath = Path.Combine(filePath, "customer.json");
                File.WriteAllText(fullPath, JsonConvert.SerializeObject(customers, Formatting.Indented));

                LoggerHelper.Info($"Successfully exported JSON to: {fullPath}");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Failed to export JSON.", ex);
            }
        }
    }
}
