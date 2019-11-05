namespace Peet.KilnMonitor.Contracts
{
    /// <summary>
    /// Error codes returned from the service.
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// Invalid credentials in an authentication request.
        /// </summary>
        InvalidCredentials = 1000,

        /// <summary>
        /// Remote error in the Bartinst API.
        /// </summary>
        UnknownRemoteError,

        /// <summary>
        /// Invalid JSON body.
        /// </summary>
        InvalidRequestBody,

        /// <summary>
        /// User needs to accept terms of service.
        /// </summary>
        NeedsTOS,
    }
}
