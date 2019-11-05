namespace Peet.KilnMonitor.Contracts
{
    using System.Collections.Generic;

    /// <summary>
    /// Collection of known kiln operational modes.
    /// </summary>
    public static class KilnMode
    {
        public static readonly ISet<string> ActiveModes = new HashSet<string>()
        {
            KilnMode.Firing,
            KilnMode.Error,
            KilnMode.DelayStart,
            KilnMode.DelayedStart,
        };

        public static readonly ISet<string> AwaitingStart = new HashSet<string>()
        {
            KilnMode.DelayStart,
            KilnMode.DelayedStart,
        };

        public const string NotConnected = "Not Connected";
        public const string Complete = "Complete";
        public const string Idle = "Idle";
        public const string Firing = "Firing";
        public const string Error = "Error";
        public const string DelayStart = "Delay Start";
        public const string DelayedStart = "Delayed Start";
    }
}
