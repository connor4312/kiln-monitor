using Microsoft.WindowsAzure.Storage.Table;
using Peet.KilnMonitor.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peet.KilnMonitor.TableEntities
{
    internal class FiringEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the start time. We finagle this a little bit so that
        /// the row key sorts in ascending order, as table storage can't
        /// actually sort data in any other way.
        /// </summary>
        [IgnoreProperty]
        public DateTimeOffset StartedAt
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(int.MaxValue - int.Parse(this.RowKey));
            }

            set
            {
                this.RowKey = (int.MaxValue - value.ToUnixTimeSeconds()).ToString();
            }
        }

        /// <summary>
        /// Gets or sets the end time. May be omitted if the firing is ongoing.
        /// </summary>
        public long? EndedAt { get; set; }

        /// <summary>
        /// Gets or sets the human-readable kiln name.
        /// </summary>
        public string KilnName { get; set; }

        /// <summary>
        /// Gets or sets the kiln ID.
        /// </summary>
        public string KilnId
        {
            get { return this.PartitionKey; }
            set { this.PartitionKey = value; }
        }

        /// <summary>
        /// Gets the public contract.
        /// </summary>
        [IgnoreProperty]
        public Firing Contract
        {
            get
            {
                return new Firing()
                {
                    StartedAt = this.StartedAt,
                    EndedAt = this.EndedAt != null 
                        ? DateTimeOffset.FromUnixTimeSeconds(this.EndedAt.Value)
                        : (DateTimeOffset?)null,
                    KilnName = this.KilnName,
                    KilnId = this.KilnId
                };
            }
        }

        public static FiringEntity FromContract(Firing contract)
        {
            return new FiringEntity()
            {
                StartedAt = contract.StartedAt,
                EndedAt = contract.EndedAt?.ToUnixTimeSeconds() ?? null,
                KilnName = contract.KilnName,
                KilnId = contract.KilnId
            };
        }
    }
}
