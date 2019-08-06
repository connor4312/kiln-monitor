using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Peet.KilnMonitor.Bartinst
{
    /// <summary>
    /// An authentication request in the Bartinst API.
    /// </summary>
    [DataContract]
    internal class AuthenticateRequest
    {
        /// <summary>
        /// Gets or sets the user details to authenticate.
        /// </summary>
        [DataMember(Name = "user", IsRequired = true)]
        public AuthenticateRequestUser User { get; set; }
    }
}
