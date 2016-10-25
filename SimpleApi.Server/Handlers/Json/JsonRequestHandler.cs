using Microsoft.Owin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleApi.Server.Extensions;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;

namespace SimpleApi.Server.Handlers.Json
{
    public interface IRequestHandler
    {
        bool CanHandleRequest(IOwinContext context);
        Task WriteResponse(IOwinContext context, EndpointConfiguration config);
    }

    public class JsonRequestHandler : IRequestHandler
    {
        public bool CanHandleRequest(IOwinContext context)
        {
            return !string.IsNullOrEmpty(context.Request.ContentType) && context.Request.ContentType.ToLower() == "application/json";
        }

        public async Task WriteResponse(IOwinContext context, EndpointConfiguration config)
        {
            var responseConfig = HandleJsonResponse(context.Request, context.Response, config);
            await context.Response.WriteBody(responseConfig);
        }

        private ResponseConfiguration HandleJsonResponse(IOwinRequest request, IOwinResponse response, EndpointConfiguration endpoint)
        {
            //Testing reading body. Note can only read body once. So copy instead.
            try
            {
                var bodyString = request.ReadBody()
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
    }
}
