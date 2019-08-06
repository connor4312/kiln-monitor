using Microsoft.WindowsAzure.Storage.Table;

namespace Peet.KilnMonitor.TableEntities
{
    /// <summary>
    ///     Storage for account information.
    /// </summary>
    public class AccountEntity : TableEntity
    {
        /// <summary>
        ///     Gets or sets the user ID this entity is associated with.
        /// </summary>
        public string UserId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }

        /// <summary>
        ///     Gets or sets the user email.
        /// </summary>
        public string Email
        {
            get { return this.RowKey;}
            set { this.RowKey = value; }
        }

        /// <summary>
        ///     Gets or sets the Bartinst auth key for the user.
        /// </summary>
        public string AuthKey { get; set; }
    }
}