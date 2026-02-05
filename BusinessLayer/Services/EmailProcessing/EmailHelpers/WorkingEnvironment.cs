using System;
using System.IO;

namespace BusinessLayer.Services.EmailProcessing.EmailHelpers
{
    public static class WorkingEnvironment
    {
        public static string AppDomainBaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

        public static string RazorEmailTemplatesDirectory => Path.Combine(AppDomainBaseDirectory, "Services", "EmailProcessing", "EmailTemplates");
    }
}
