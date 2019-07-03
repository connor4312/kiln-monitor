using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Peet.KilnMonitor.Bartinst
{
    /// <summary>
    /// Response from the <see cref="AuthenticateRequest"/>
    /// </summary>
    [DataContract]
    public class AuthenticateResponse
    {
        /// <summary>
        /// Gets or sets the auth token.
        /// </summary>
        [DataMember(Name = "authentication_token", IsRequired = true)]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets whether the user needs to accept the terms of service.
        /// </summary>
        [DataMember(Name = "needs_to_accept_new_terms_and_conditions", IsRequired = false)]
        public bool NeedsToAcceptTos { get; set; }
    }
}
