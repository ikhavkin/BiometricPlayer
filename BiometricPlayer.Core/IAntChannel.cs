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

        /// <summary>
        /// Channel radio frequency, see ANT_SetChannelRFFreq.
        /// </summary>
        /// <remarks>
        /// Channel Frequency = 2400 MHz + value * 1.0 MHz 
        /// </remarks>
        byte ChannelRFFrequency { get; set; }

        /// <summary>
        /// Channel messaging period, see ANT_SetChannelPeriod.
        /// </summary>
        /// <remarks>
        /// The channel messaging period in seconds * 32768. Maximum messaging period is ~2 seconds. 
        /// </remarks>
        ushort ChannelPeriod { get; set; }

        /// <summary>
        /// Device number to recieve data from, 0 means any device. See ANT_SetChannelId.
        /// </summary>
        ushort DeviceNumber { get; set; }

        /// <summary>
        /// Open channel with set configuration parameters.
        /// </summary>
        void Open();
    }
}
