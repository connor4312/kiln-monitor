namespace Peet.KilnMonitor.Contracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Inner object of the <see cref="AuthenticateRequest"/>
    /// </summary>
    [DataContract]
    public class AuthenticateRequest
    {
        /// <summary>
        /// Gets or sets the email to authenticate.
        /// </summary>
        [DataMember(Name = "email", IsRequired = true)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        [DataMember(Name = "password", IsRequired = true)]
        public string Password { get; set; }
    }
}
