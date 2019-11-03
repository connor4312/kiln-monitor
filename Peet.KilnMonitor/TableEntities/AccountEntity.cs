namespace Peet.KilnMonitor.TableEntities
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    ///     Storage for account information.
    /// </summary>
    public class AccountEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the Bartinst auth key for the user.
        /// </summary>
        public string AuthKey { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Google user ID this entity is associated with.
        /// </summary>
        public string UserId
        {
            get
            {
                return PartitionKey;
            }
            set
            {
                this.PartitionKey = value;
                this.RowKey = value;
            }
        }
    }
}
