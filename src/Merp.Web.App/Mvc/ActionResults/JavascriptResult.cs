using Microsoft.AspNetCore.Mvc;

namespace Merp.Web.App.Mvc
{
    public class JavascriptResult : ContentResult
    {
        public JavascriptResult(string script)
        {
            Content = script;
            ContentType = "application/javascript";
        }
    }
}
