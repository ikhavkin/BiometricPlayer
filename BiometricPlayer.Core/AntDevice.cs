using System;
using ANT_Managed_Library;

namespace BiometricPlayer.Core
{
    public class AntDevice : IAntDevice
    {
        private readonly object locker = new object();
        private ANT_Device device;
        private bool isDisposed = false;

        public void Init()
        {
            lock (locker)
            {
                // alternative default: ANT_ReferenceLibrary.PortType.USB, 0, 57600, ANT_ReferenceLibrary.FramerType.basicANT
                device = new ANT_Device();
            }
        }

        public void Reset()
        {
            lock (locker)
            {
                if (device == null)
                {
                    throw new InvalidOperationException("Device is not initialized.");
                }
            }
        }

        public byte[] NetworkKey { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (locker)
            {
                if (isDisposed)
                {
                    throw new ObjectDisposedException(typeof (AntDevice).FullName);
                }

                if (device != null)
                {
                    device.Dispose();
                }

                isDisposed = true;
                GC.SuppressFinalize(this);
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