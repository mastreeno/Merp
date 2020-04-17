using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Web.Mvc
{
    public class LocalizationJsonResult : ActionResult
    {
        /// <summary>
        /// Get or set the resource's controller name
        /// </summary>
        public string ControllerResourceName { get; private set; }

        /// <summary>
        /// Get or set the resource's action name
        /// </summary>
        public string ActionResourceName { get; private set; }

        /// <summary>
        /// Get or set the request's culture
        /// </summary>
        public CultureInfo RequestCulture { get; private set; }

        private readonly Assembly _assembly;

        public LocalizationJsonResult(string controllerResourceName, string actionResourceName, CultureInfo requestCulture)
        {
            if (string.IsNullOrWhiteSpace(controllerResourceName))
            {
                throw new ArgumentException("value cannot be empty", nameof(controllerResourceName));
            }

            if (string.IsNullOrWhiteSpace(actionResourceName))
            {
                throw new ArgumentException("value cannot be empty", nameof(actionResourceName));
            }

            ControllerResourceName = controllerResourceName;
            ActionResourceName = actionResourceName;
            RequestCulture = requestCulture ?? throw new ArgumentNullException(nameof(requestCulture));

            _assembly = Assembly.GetEntryAssembly();
        }

        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var serializer = new JsonSerializer()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            using (var writer = new StringWriter())
            {
                var result = new Dictionary<string, object>();
                BuildSharedResources(result);
                BuildResourcesByResourceName(result);

                serializer.Serialize(writer, result);
                var json = writer.ToString();
                var stream = Encoding.UTF8.GetBytes(json);
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.Body.WriteAsync(stream, 0, stream.Length).Wait();
            }
        }

        private void BuildSharedResources(IDictionary<string, object> resources)
        {
            var sharedResourcesName = $"{_assembly.GetName().Name}.Resources.Shared";

            var resourceManager = new ResourceManager(sharedResourcesName, _assembly);
            foreach (DictionaryEntry resource in resourceManager.GetResourceSet(RequestCulture, true, true))
            {
                resources.Add(resource.Key.ToString(), resource.Value);
            }
        }

        private void BuildResourcesByResourceName(IDictionary<string, object> resources)
        {
            var resourceName = $"{_assembly.GetName().Name}.Resources.{ControllerResourceName}.{ActionResourceName}";

            var resourceManager = new ResourceManager(resourceName, _assembly);
            foreach (DictionaryEntry resource in resourceManager.GetResourceSet(RequestCulture, true, true))
            {
                if (!resources.ContainsKey(resource.Key.ToString()))
                {
                    resources.Add(resource.Key.ToString(), resource.Value);
                }
                else
                {
                    resources[resource.Key.ToString()] = resource.Value;
                }
            }
        }
    }
}
