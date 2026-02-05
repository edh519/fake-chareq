using System.IO;

namespace BusinessLayer.Helpers;

public static class ExportPathHelper
{
    public static string GetExportDirectory(string environmentName)
    {
        string serverBasePath = @"D:\AppData";

        return environmentName.ToLower() switch
        {
            "development" => Path.Combine(Directory.GetCurrentDirectory(), "InternalStorage", "exports"),

            "training" => Path.Combine(serverBasePath, "ChaReqTraining", "exports"),

            "production" => Path.Combine(serverBasePath, "ChaReq", "exports"),
            _ => Path.Combine(serverBasePath, "ChaReq", "exports"),
        };
    }

    public static bool DataExportFileExists(string environmentName, string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return false;
        }

        string filePath = Path.Combine(GetExportDirectory(environmentName), fileName);
        return File.Exists(filePath);
    }
}