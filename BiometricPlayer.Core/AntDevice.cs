using System;
using System.Linq;
using ANT_Managed_Library;

namespace BiometricPlayer.Core
{
    public class AntDevice : IAntDevice
    {
        readonly object locker = new object();
        ANT_Device device;
        bool isDisposed = false;
        byte[] networkKey;
        readonly byte[] defaultNetworkKey = new byte[8];

        public void Init()
        {
            lock (locker)
            {
                CheckDisposed();

                // alternative default: ANT_ReferenceLibrary.PortType.USB, 0, 57600, ANT_ReferenceLibrary.FramerType.basicANT
                device = new ANT_Device();
                // todo scope: support more than one network at a time
                device.setNetworkKey(0, NetworkKey);
            }
        }

        public void Reset()
        {
            lock (locker)
            {
                CheckInitialized();

                device.ResetSystem();
            }
        }

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

                    networkKey = value.ToArray();
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
