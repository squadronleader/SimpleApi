using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using SimpleApi.Server.Extensions;
using System.Web;

/// <summary>
/// This class represents a simple own middleware component that uses json config files to respond to requests.
/// You can create a new route and response by simply adding a new .json file to the /Content folder. See HelloWorld.json for and example
/// </summary>
namespace SimpleApi.Server.Middleware
{
    public class SimpleRouter : OwinMiddleware
    {
        private readonly string _basePath;

        private Dictionary<string, EndpointConfiguration> _endpoints = new Dictionary<string, EndpointConfiguration>();

        public SimpleRouter(OwinMiddleware next, string basePath) : base(next)
        {
            _basePath = basePath;
            SetupEndpoints(basePath);
        }

        public override Task Invoke(IOwinContext context)
        {
            var request = context.Request;
            var response = context.Response;

            var requestPath = request.Path.ToString().ToLower();

            //Special route that allows end user refresh the endpoints
            if (requestPath == "/reloadendpoints")
            {
                RefreshEnpoints(_basePath);
            }

            var endpoint = _endpoints.GetOrDefault(requestPath);

            if (endpoint != null)
            {
                response.ContentType = endpoint.ContentType;
                response.AddHeaders(endpoint.Headers);
                return response.WriteAsync(endpoint.Body);
            }
            else
            {
                response.ContentType = "text/plain";
                return response.WriteAsync("Error no endpoint");
            }
            
            //return Next.Invoke(context);
        }

        private void RefreshEnpoints(string basePath)
        {
            _endpoints = new Dictionary<string, EndpointConfiguration>();
            SetupEndpoints(basePath);
        }

        private void SetupEndpoints(string configurationPath)
        {
            //Using this as Server.MapPath isn't available in owin app.
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, configurationPath);
            foreach (string file in Directory.EnumerateFiles(path, "*.json"))
            {
                var endpoint = JsonConvert.DeserializeObject<EndpointConfiguration>(File.ReadAllText(file));
                //endpoint key should be lowercase since its the url and its case insensitive.
                var key = endpoint.Url.ToLower();
                if (!_endpoints.ContainsKey(key))
                {
                    _endpoints.Add(key, endpoint);
                }
            }
        }

    }
}
