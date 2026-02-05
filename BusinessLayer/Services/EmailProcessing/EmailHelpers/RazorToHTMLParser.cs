using RazorLight;
using System;
using System.Threading.Tasks;

namespace BusinessLayer.Services.EmailProcessing.EmailHelpers
{
    public class RazorToHtmlParser
    {
        private readonly RazorLightEngine _razorEngine;
        public RazorToHtmlParser()
        {

            _razorEngine = new RazorLightEngineBuilder()
                .UseFileSystemProject(WorkingEnvironment.RazorEmailTemplatesDirectory)
                .UseMemoryCachingProvider()
                .Build();

        }
        public Task<string> RenderHtmlStringAsync<T>(string viewName, T model)
        {
            try
            {
                return _razorEngine.CompileRenderAsync(viewName, model);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
