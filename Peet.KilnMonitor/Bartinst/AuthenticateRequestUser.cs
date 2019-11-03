namespace Peet.KilnMonitor.Bartinst
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Inner object of the <see cref="AuthenticateRequest"/>
    /// </summary>
    [DataContract]
    internal class AuthenticateRequestUser
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
