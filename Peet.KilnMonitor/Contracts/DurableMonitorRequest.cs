namespace Peet.KilnMonitor.Contracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Request sent to the durable function to monitor a firing.
    /// </summary>
    [DataContract]
    public sealed class DurableMonitorRequest
    {
        /// <summary>
        /// Gets or sets the auth token for the request.
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the auth token for the request.
        /// </summary>
        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}
