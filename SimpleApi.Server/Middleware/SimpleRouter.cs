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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

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

            if (endpoint != null)
            {
                var responseConfig = HandleJsonResponse(request, response, endpoint);
                await response.WriteBody(responseConfig);
            } 
            else
            {
                var noEndpointResponse = new ResponseConfiguration
                {
                    Body = "No endpoint defined",
                    ContentType = "plain/text",
                    HttpStatus = 404,
                };

                await WriteResponse(response, noEndpointResponse);
            }

        }

        private string ReadBody(Stream stream)
        {
            var bodyString = "";
            using (var memStream = new MemoryStream())
            {
                stream.CopyTo(memStream);
                memStream.Position = 0; //reset stream to start
                using (var reader = new StreamReader(memStream))
                {
                    bodyString = reader.ReadToEnd();
                }
            }

            return bodyString;
        }

        private async Task WriteResponse(IOwinResponse response, ResponseConfiguration config)
        {
            response.ContentType = config.ContentType;
            response.AddHeaders(config.Headers);
            response.StatusCode = config.HttpStatus;

            await response.WriteAsync(config.Body);
        }

        private ResponseConfiguration HandleJsonResponse(IOwinRequest request, IOwinResponse response, EndpointConfiguration endpoint)
        {
            //Testing reading body. Note can only read body once. So copy instead.
            try
            {
                var bodyString = ReadBody(request.Body);
                var jsonObj = JObject.Parse(bodyString);

                //If user has setup request validation then check here
                if (endpoint.ValidateRequest != null && !ValidateJsonSchema(jsonObj, endpoint))
                {
                    return endpoint.ErrorResponse;
                }

                if (endpoint.Responses.Count() == 0)
                {
                    return endpoint.DefaultResponse;
                }

                foreach (var responseConfig in endpoint.Responses)
                {
                    if (ValidateRules(jsonObj, responseConfig))
                    {
                        return responseConfig;
                    }
                }

                //If nothing else then just return the first response in the response array
                return endpoint.DefaultResponse;
            }
            catch (Exception ex)
            {
                //TODO log
                return endpoint.ErrorResponse;
            }

        }

        private bool ValidateJsonSchema(JObject jsonObj, EndpointConfiguration config)
        {
            var schemaString = JsonConvert.SerializeObject(config.ValidateRequest);
            JSchema schema = JSchema.Parse(schemaString);
            return jsonObj.IsValid(schema);
        }

        private bool ValidateRules(JObject jsonObj, ResponseConfiguration config)
        {
            if (config.Rules == null || config.Rules.Count == 0)
            {
                return false;
            }

            foreach (var rule in config.Rules)
            {
                var jsonValueObj = jsonObj.SelectToken(rule.PropertyPath);
                if (jsonValueObj == null)
                {
                    return false;
                }

                var jsonValue = jsonValueObj.Value<string>();
                if (jsonValue.Trim() == rule.Matches.Trim())
                {
                    return true;
                }
            }

            return false;
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
