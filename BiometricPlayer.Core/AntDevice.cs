using System;
using System.Linq;
using ANT_Managed_Library;

namespace BiometricPlayer.Core
{
    /// <summary>
    /// ANT device.
    /// </summary>
    public class AntDevice : IAntDevice
    {
        readonly object locker = new object();
        ANT_Device device;
        bool isDisposed = false;
        byte[] networkKey;
        readonly byte[] defaultNetworkKey = new byte[8];
        AntChannel channel;

        /// <summary>
        /// Initialize device using configured parameters.
        /// </summary>
        public void Init()
        {
            lock (locker)
            {
                CheckDisposed();

                // alternative default: ANT_ReferenceLibrary.PortType.USB, 0, 57600, ANT_ReferenceLibrary.FramerType.basicANT
                device = new ANT_Device();
                
                Reset();

                // todo scope: support more than one network at a time
                device.setNetworkKey(0, NetworkKey);

                channel = new AntChannel(device.getChannel(0));
            }
        }

        /// <summary>
        /// Reset device.
        /// </summary>
        public void Reset()
        {
            lock (locker)
            {
                CheckInitialized();

                device.ResetSystem();
            }
        }

        /// <summary>
        /// Network key. Has to be 8 bytes length.
        /// </summary>
        public byte[] NetworkKey
        {
            get 
            {
                lock (locker)
                {
                    return (networkKey ?? defaultNetworkKey).ToArray();
                }
            }
            set
            {
                lock (locker)
                {
                    CheckDisposed();

                    if (device != null)
                    {
                        throw new InvalidOperationException("Already initialized");
                    }

                    if (value.Length != 8)
                    {
                        throw new ArgumentOutOfRangeException("value");
                    }

                    networkKey = value.ToArray();
                }
            }
        }

        /// <summary>
        /// ANT channel.
        /// </summary>
        // todo scope: support more than only one channel
        public virtual IAntChannel Channel
        {
            get
            {
                lock (locker)
                {
                    CheckInitialized();

                    return channel;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (locker)
            {
                CheckDisposed();
                
                var channelDisposable = (IDisposable)channel;
                if (channelDisposable != null)
                {
                    channelDisposable.Dispose();    
                }

                if (device != null)
                {
                    device.Dispose();
                }

                isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        void CheckInitialized()
        {
            CheckDisposed();

            if (device == null)
            {
                throw new InvalidOperationException("Device is not initialized.");
            }
        }

        void CheckDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(typeof (AntDevice).FullName);
            }
        }

        ~AntDevice()
        {
            lock (locker)
            {
                if (!isDisposed)
                {
                    Dispose();
                }
            }
        }
    }
}
