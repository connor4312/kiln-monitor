using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Peet.KilnMonitor.Contracts
{
    /// <summary>
    /// Standard error response returned from the API.
    /// </summary>
    /// <typeparam name="T">Type of data returned in the response</typeparam>
    [DataContract]
    public class ErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="errorCode">Error code to return</param>
        /// <param name="message">The error message</param>
        /// <param name="statusCode">Status code of the response</param>
        public ErrorResponse(
            ErrorCode errorCode,
            string message,
            HttpStatusCode? statusCode = null)
        {
            this.ErrorCode = (uint)errorCode;
            this.Message = message;
            this.StatusCode = statusCode ?? HttpStatusCode.BadRequest;
        }

        /// <summary>
        /// Gets the error number
        /// </summary>
        [DataMember(Name = "errorCode", IsRequired = false)]
        public uint ErrorCode { get; }

        /// <summary>
        /// Gets or sets the human-readable error message.
        /// </summary>
        [DataMember(Name = "message", IsRequired = false)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the status code to use for this response.
        /// </summary>
        [IgnoreDataMember]
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the any additional headers to use for this response.
        /// </summary>
        [IgnoreDataMember]
        public IDictionary<string, string> Headers { get; set; }
    }
}
