using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using SimpleApiServer.Extensions;


/// <summary>
/// This class represents a simple own middleware component that uses json config files to respond to requests.
/// You can create a new route and response by simply adding a new .json file to the /Content folder. See HelloWorld.json for and example
/// </summary>
namespace SimpleApiServer.Middleware
{
    public class SimpleRouter : OwinMiddleware
    {
        private readonly string _basePath;

        private readonly Dictionary<string, EndpointConfiguration> _endpoints = new Dictionary<string, EndpointConfiguration>();

        public SimpleRouter(OwinMiddleware next, string basePath) : base(next)
        {
            _basePath = basePath;
            SetupEndpoints(basePath);
        }

        public override Task Invoke(OwinRequest request, OwinResponse response)
        {
            var requestPath = request.Path.ToLower();

            var endpoint = _endpoints.GetOrDefault(requestPath);

            if(endpoint != null)
            {
                response.ContentType = endpoint.ContentType;
                response.AddHeaders(endpoint.Headers);
                response.WriteBody(endpoint.Body);
            }
            else
            {
                response.ContentType = "text/plain";
                response.WriteBody("Error no endpoint");
            }

            return Next.Invoke(request, response);
        }


        private void SetupEndpoints(string configurationPath)
        {
            foreach (string file in Directory.EnumerateFiles(configurationPath, "*.json"))
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
