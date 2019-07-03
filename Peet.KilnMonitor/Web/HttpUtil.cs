using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Peet.KilnMonitor.Contracts;

namespace Peet.KilnMonitor.Web
{
    /// <summary>
    /// Extensions for the asp.net <see cref="HttpRequest"/>.
    /// </summary>
    internal static class HttpUtil
    {
        private const string ContinuationTokenParam = "ct";
        private static readonly JsonSerializer Serializer = new JsonSerializer();

        /// <summary>
        /// Gets the user ID making the request.
        /// </summary>
        /// <param name="request">Incoming request</param>
        /// <returns>User ID</returns>
        public static string GetUserId(this HttpRequest request)
        {
            return ClaimsPrincipal.Current.Identity.Name;
        }

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
        /// <returns>Continuation token</returns>
        public static string GetContinuationToken(this HttpRequest request)
        {
            return request.Query.TryGetValue(HttpUtil.ContinuationTokenParam, out var continuation)
                ? (string)continuation
                : null;
        }

        /// <summary>
        /// Gets the continuation token in the response headers.
        /// </summary>
        /// <param name="token">Token to set</param>
        public static void SetContinuationToken(this HttpRequest request, string token)
        {
            request.HttpContext.Response.Headers.Add("Link", $"<{request.Path}?{ContinuationTokenParam}={token}>; rel=\"next\"");
        }
        /// <summary>
        /// Gets any continuation token presented in the request.
        /// </summary>
        /// <param name="request">Incoming request</param>
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
