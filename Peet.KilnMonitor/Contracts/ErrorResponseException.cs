namespace Peet.KilnMonitor.Contracts
{
    using System;
    using System.Net;

    /// <summary>
    /// Exception thrown when there's an error in the request.
    /// </summary>
    public class ErrorResponseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/> class.
        /// </summary>
        /// <param name="response">Response the error contains</param>
        public ErrorResponseException(ErrorResponse response)
        {
            this.Response = response;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/>
        /// class, forming the error response immediately from its arguments.
        /// </summary>
        /// <param name="errorCode">Error code to return</param>
        /// <param name="message">The error message</param>
        /// <param name="statusCode">Status code of the response</param>
        public ErrorResponseException(
            ErrorCode errorCode,
            string message = null,
            HttpStatusCode? statusCode = null)
            : this(new ErrorResponse(errorCode, message, statusCode))
        {
        }

        /// <summary>
        /// Gets the error response from this exception.
        /// </summary>
        public ErrorResponse Response { get; }
    }
}
