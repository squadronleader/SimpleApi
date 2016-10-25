using Microsoft.Owin;
using SimpleApi.Server.Handlers.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleApi.Server.Extensions;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SimpleApi.Server.Middleware
{
    class RouterHandler : OwinMiddleware
    {
        private List<IRequestHandler> _handlers { get; set; } = new List<IRequestHandler>();
        private readonly string _basePath;
        private ResponseConfiguration _noEndpointResponse = new ResponseConfiguration
        {
            Body = "No endpoint defined",
            ContentType = "plain/text",
            HttpStatus = 404,
        };

        private Dictionary<string, EndpointConfiguration> _endpoints = new Dictionary<string, EndpointConfiguration>();

        public RouterHandler(OwinMiddleware next, List<IRequestHandler> handlers, string basePath) : base(next)
        {
            _handlers = handlers;
            _basePath = basePath;
            SetupEndpoints(basePath);
        }

        public async override Task Invoke(IOwinContext context)
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
            var handler = _handlers.FirstOrDefault(h => h.CanHandleRequest(context));

            if (endpoint != null && handler != null)
            {
                await handler.WriteResponse(context, endpoint);
            }
            else
            {
                await response.WriteBody(_noEndpointResponse);
            }
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
