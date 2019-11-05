namespace Peet.KilnMonitor.Contracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Request sent to the durable function to monitor a firing.
    /// </summary>
    [DataContract]
    public sealed class StartMonitoringRequest
    {
        /// <summary>
        /// Gets or sets the email for the user.
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password for the user.
        /// </summary>
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}
