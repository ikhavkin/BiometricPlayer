using System;
using System.Reactive.Subjects;
using ANT_Managed_Library;

namespace BiometricPlayer.Core
{
    /// <summary>
    /// Channel of the <see cref="AntDevice"/>.
    /// </summary>
    public class AntChannel : IAntChannel, IDisposable
    {
        readonly object locker = new object();
        readonly ANT_Channel channel;
        readonly Subject<AntMessage> messageSubject = new Subject<AntMessage>();
        bool isOpened = false;
        byte channelRfFrequency;
        ushort deviceNumber;
        ushort channelPeriod;
        bool isDisposed;

        /// <summary>
        /// Constructs <see cref="AntChannel"/>.
        /// </summary>
        /// <param name="channel">Underlying <see cref="ANT_Channel"/> object from official ANT managed API.</param>
        internal AntChannel(ANT_Channel channel)
        {
            this.channel = channel;

            // todo: for now assigned to network 0 and channel type BASE_Slave_Receive_0x00 automatically, other hardcoded defaults
            channel.assignChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, 0);
            channel.setChannelSearchTimeout(255);
        }

        /// <summary>
        /// Open channel with set configuration parameters.
        /// </summary>
        public void Open()
        {
            lock (locker)
            {
                CheckDisposed();
                CheckOpened();

                channel.setChannelID(DeviceNumber, false, 0, 0);
                channel.setChannelPeriod(ChannelPeriod);
                channel.setChannelFreq(ChannelRFFrequency);
                channel.channelResponse += OnChannelResponseRecieved;

                channel.openChannel();
                isOpened = true;
            }
        }

        void OnChannelResponseRecieved(ANT_Response response)
        {
            lock (locker)
            {
                messageSubject.OnNext(new AntMessage((byte) response.getMessageID()));
            }
        }

        /// <summary>
        /// Channel radio frequency, see ANT_SetChannelRFFreq.
        /// </summary>
        /// <remarks>
        /// Channel Frequency = 2400 MHz + value * 1.0 MHz 
        /// </remarks>
        public byte ChannelRFFrequency
        {
            get 
            {
                lock (locker)
                {
                    return channelRfFrequency;
                }
            }
            set
            {
                lock (locker)
                {
                    CheckOpened();
                    channelRfFrequency = value;
                }
            }
        }

        /// <summary>
        /// Channel messaging period, see ANT_SetChannelPeriod.
        /// </summary>
        /// <remarks>
        /// The channel messaging period in seconds * 32768. Maximum messaging period is ~2 seconds. 
        /// </remarks>
        public ushort ChannelPeriod
        {
            get 
            {
                lock (locker)
                {
                    return channelPeriod;
                }
            }
            set
            {
                lock (locker)
                {
                    CheckOpened();
                    channelPeriod = value;
                }
            }
        }

        /// <summary>
        /// Device number to recieve data from, 0 means any device. See ANT_SetChannelId.
        /// </summary>
        public ushort DeviceNumber
        {
            get
            {
                lock (locker)
                {
                    return deviceNumber;
                }
            }
            set
            {
                lock (locker)
                {
                    CheckOpened();
                    deviceNumber = value;
                }
            }
        }

        /// <summary>
        /// Messages recieved by this channel.
        /// </summary>
        public virtual IObservable<AntMessage> Events
        {
            get
            {
                lock (locker)
                {
                    // todo: implement
                    return messageSubject;
                }
            }
        }

        void CheckOpened()
        {
            if (isOpened)
            {
                throw new InvalidOperationException("Channel is already opened!");
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            lock (locker)
            {
                CheckDisposed();

                channel.closeChannel();
                messageSubject.OnCompleted();

                isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        void CheckDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(typeof (AntChannel).FullName);
            }
        }

        ~AntChannel()
        {
            lock (locker)
            {
                if (!isDisposed)
                {
                    ((IDisposable)this).Dispose();
                }
            }
        }
    }
}
