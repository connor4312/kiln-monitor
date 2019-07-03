using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Peet.KilnMonitor.Contracts
{
    /// <summary>
    /// Marks the start and end time of one kiln cycle.
    /// </summary>
    [DataContract]
    public class Firing
    {
        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        [DataMember(Name = "startedAt", IsRequired = true)]
        public DateTimeOffset StartedAt { get; set; }

        /// <summary>
        /// Gets or sets the end time. May be omitted if the firing is ongoing.
        /// </summary>
        [DataMember(Name = "endedAt", IsRequired = false)]
        public DateTimeOffset? EndedAt { get; set; }

        /// <summary>
        /// Gets or sets the human-readable kiln name.
        /// </summary>
        [DataMember(Name = "kilnName", IsRequired = true)]
        public string KilnName { get; set; }

        /// <summary>
        /// Gets or sets the kiln ID.
        /// </summary>
        [DataMember(Name = "kilnName", IsRequired = true)]
        public string KilnId { get; set; }
    }
}
