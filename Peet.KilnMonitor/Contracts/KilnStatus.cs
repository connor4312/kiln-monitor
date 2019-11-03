namespace Peet.KilnMonitor.Contracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Status returned from the Bartinst API on the kiln.
    /// </summary>
    [DataContract]
    public class KilnStatus
    {
        /// <summary>
        /// Gets or sets a map of kiln firing data.
        /// </summary>
        [DataMember(Name = "firing")]
        public IDictionary<string, object> Firing { get; set; }

        /// <summary>
        /// Gets or sets the kiln ID.
        /// </summary>
        [DataMember(Name = "id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the kiln mode.
        /// </summary>
        [DataMember(Name = "mode")]
        public string Mode { get; set; }

        /// <summary>
        /// Gets or sets the kiln human readable name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the kiln operational mode.
        /// </summary>
        [DataMember(Name = "op_mode")]
        public string OperationalMode { get; set; }
    }
}
