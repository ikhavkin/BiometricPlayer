using System;

namespace BiometricPlayer.Core
{
    /// <summary>
    /// ANT channel.
    /// </summary>
    public interface IAntChannel
    {
        /// <summary>
        /// Messages recieved by this channel.
        /// </summary>
        IObservable<AntMessage> Events { get; }
    }
}
