namespace Peet.KilnMonitor.Web
{
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Peet.KilnMonitor.Contracts;

    /// <summary>
    /// Extensions for the asp.net <see cref="HttpRequest"/>.
    /// </summary>
    internal static class HttpUtil
    {
        private static readonly JsonSerializer Serializer = new JsonSerializer();

        /// <summary>
        /// Gets any continuation token presented in the request.
        /// </summary>
        /// <param name="request">Incoming request</param>
        /// <returns>Continuation token</returns>
        public static T ReadBody<T>(this HttpRequest request)
        {
            try
            {
                using (var sr = new StreamReader(request.Body))
                using (var reader = new JsonTextReader(sr))
                {
                    return HttpUtil.Serializer.Deserialize<T>(reader);
                }
            }
            catch (JsonException e)
            {
                throw new ErrorResponseException(ErrorCode.InvalidRequestBody, e.Message);
            }
        }

        /// <summary>
        /// Gets any continuation token presented in the request.
        /// </summary>
        /// <param name="request">Incoming request</param>
        /// <param name="error">Error to write</param>
        /// <returns>Continuation token</returns>
        public static ActionResult WriteError(this HttpRequest request, ErrorResponse error)
        {
            if (error.Headers != null)
            {
                foreach (var pair in error.Headers)
                {
                    request.HttpContext.Response.Headers.Add(pair.Key, pair.Value);
                }
            }

            return new ObjectResult(error)
            {
                StatusCode = (int)error.StatusCode
            };
        }
    }
}
